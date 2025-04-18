using LeaderReport.Application.DTOs;
using LeaderReport.Application.Interfaces;
using LeaderReport.Domain.Entities;
using LeaderReport.Domain.Interfaces;
using LeaderReport.Infrastructure.Repositories.Generic;
using LeaderReport.Models;
using OfficeOpenXml;

namespace LeaderReport.Application.Services;

public class FileProcessService : IFileProcessService
{
    private readonly IFileRepository _fileRepository;
    private readonly IGenericRepository<Producto> _genericProducto;
    private readonly IGenericRepository<Archivo> _genericArchivo;
    private readonly IGenericRepository<Marca> _genericMarca;
    private readonly IGenericRepository<Categoria> _genericCategoria;
    
    private readonly IGenericRepository<Presentacion> _genericPresentacion;

    private readonly IGenericRepository<Proveedor> _genericProveedor;
    

    public FileProcessService(
        IFileRepository fileRepository,
        IGenericRepository<Producto> genericProducto,
        IGenericRepository<Archivo> genericArchivo,
        IGenericRepository<Marca> genericMarca,
        IGenericRepository<Categoria> genericCategoria,
        IGenericRepository<Presentacion> genericPresentacion,
        IGenericRepository<Proveedor> genericProveedor)
    {
        _fileRepository = fileRepository;
        _genericProducto = genericProducto;
        _genericArchivo = genericArchivo;
        _genericCategoria = genericCategoria;
        _genericMarca = genericMarca;
        _genericPresentacion = genericPresentacion;
        _genericProveedor = genericProveedor;
    }

    public async Task<FileProcessingResultDto> GuardarYProcesarArchivoAsync(byte[] fileData, string fileName)
    {
        // 1️⃣ Guarda el archivo en disco
        string filePath = await _fileRepository.GuardarArchivoAsync(fileData, fileName);
        var archivoExistente = (await _genericArchivo.GetAll()).FirstOrDefault(a => a.Nombre == fileName);
        // 2️⃣ Crea un objeto Archivo y guárdalo en la base de datos
        if(archivoExistente != null)
            throw new Exception ("Un archivo con ese nombre ya existe");
            
        var archivo = new Archivo
        {
            Nombre = fileName,
            Ruta = filePath,
            FechaSubida = DateTime.UtcNow
        };
        await _genericArchivo.Add(archivo);

        // 3️⃣ Si es un Excel, lo procesamos
        int processed = 0;
        int errors = 0;
        if (fileName.EndsWith(".xlsx") || fileName.EndsWith(".xls"))
        {
            (processed, errors) = await ProcesarExcelAsync(filePath);
        }

        return new FileProcessingResultDto
        {
            FileName = fileName,
            ProcessedCount = processed,
            ErrorsCount = errors
        };
    }

    private async Task<(int processed, int errors)> ProcesarExcelAsync(string filePath)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        int processedCount = 0;
        int errorsCount = 0;

        try
        {
            using var package = new ExcelPackage(new FileInfo(filePath));
            var worksheet = package.Workbook.Worksheets[0];

            // Crear diccionarios para cachear las categorías y marcas
            var categoriasCache = (await _genericCategoria.GetAll())
                .ToDictionary(c => c.Nombre.ToLower(), c => c);
            
            var marcasCache = (await _genericMarca.GetAll())
                .ToDictionary(m => m.Nombre.ToLower(), m => m);
            
            var proveedoresCache = (await _genericProveedor.GetAll())
                .ToDictionary(p => p.Nombre.ToLower(), p => p);

            for (int row = 2; row <= worksheet.Dimension.Rows; row++)
            {
                try
                {
                    string? nombreCategoria = worksheet.Cells[row, 4].Text?.Trim() ?? "";
                    string? nombreMarca = worksheet.Cells[row, 3].Text?.Trim() ?? "";
                    string? nombreProveedor = worksheet.Cells[row, 5].Text?.Trim() ?? "";

                    // Obtener o crear la categoría
                    Categoria categoria;
                    if (!categoriasCache.TryGetValue(nombreCategoria.ToLower(), out categoria))
                    {
                        categoria = new Categoria { Nombre = nombreCategoria };
                        await _genericCategoria.Add(categoria);
                        categoriasCache.Add(nombreCategoria.ToLower(), categoria);
                    }

                    // Obtener o crear la marca
                    Marca marca;
                    if (!marcasCache.TryGetValue(nombreMarca.ToLower(), out marca))
                    {
                        marca = new Marca { Nombre = nombreMarca };
                        await _genericMarca.Add(marca);
                        marcasCache.Add(nombreMarca.ToLower(), marca);
                    }
                    
                    // Obtener o crear el proveedor (mover esto antes de procesar el producto)
                    Proveedor proveedor;
                    if (string.IsNullOrWhiteSpace(nombreProveedor))
                    {
                        throw new ArgumentException("El nombre del proveedor no puede estar vacío");
                    }

                    if (!proveedoresCache.TryGetValue(nombreProveedor.ToLower(), out proveedor))
                    {
                        proveedor = new Proveedor 
                        { 
                            Nombre = nombreProveedor,
                            DNI = "",
                            Email = "",
                            Empresa = nombreProveedor
                        };
                        await _genericProveedor.Add(proveedor);
                        proveedoresCache.Add(nombreProveedor.ToLower(), proveedor);
                    }

                    // Procesar el producto
                    var codBarra = worksheet.Cells[row, 1].Text?.Trim();
                    var nombreProducto = worksheet.Cells[row, 2].Text?.Trim();
                    decimal precio = worksheet.Cells[row, 7].GetValue<decimal>();
                    precio = Math.Round(precio, 2);

                    // Verificar si el producto ya existe
                    var productoExistente = await _genericProducto.FirstOrDefault(p => p.Nombre.ToLower() == nombreProducto.ToLower());

                    if (productoExistente == null)
                    {
                        var nuevoProducto = new Producto
                        {
                            Cod_Barra = codBarra,
                            Nombre = nombreProducto,
                            CategoriaId = categoria.Id,
                            MarcaId = marca.Id,
                            ProveedorId = proveedor.Id  // Asignar el proveedor directamente en la creación
                        };

                        await _genericProducto.Add(nuevoProducto);

                        var nuevaPresentacion = new Presentacion
                        {
                            ProductoId = nuevoProducto.Id,
                            Precio = precio
                        };

                        await _genericPresentacion.Add(nuevaPresentacion);
                    }
                    else
                    {
                        // Actualizar categoría, marca y proveedor
                        productoExistente.CategoriaId = categoria.Id;
                        productoExistente.MarcaId = marca.Id;
                        productoExistente.ProveedorId = proveedor.Id;  // Actualizar también el proveedor
                        await _genericProducto.Update(productoExistente);
                        var presentacionExistente = await _genericPresentacion.FirstOrDefault(p => p.ProductoId == productoExistente.Id);

                        if (presentacionExistente == null)
                        {
                            // Agregar nueva presentación
                            var nuevaPresentacion = new Presentacion
                            {
                                ProductoId = productoExistente.Id,
                                Precio = precio
                            };

                            await _genericPresentacion.Add(nuevaPresentacion);
                        }
                        else
                        {
                            // Actualizar precio de la presentación existente
                            presentacionExistente.Precio = precio;
                            await _genericPresentacion.Update(presentacionExistente);
                        }
                    }

                    processedCount++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠ Error en la fila {row}: {ex.Message}");
                    errorsCount++;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error procesando el archivo: {ex.Message}");
            throw;
        }

        return (processedCount, errorsCount);
    }

}
