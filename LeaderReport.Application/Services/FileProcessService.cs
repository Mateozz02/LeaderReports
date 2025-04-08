using LeaderReport.Application.DTOs;
using LeaderReport.Application.Interfaces;
using LeaderReport.Domain.Entities;
using LeaderReport.Domain.Interfaces;
using LeaderReport.Infrastructure.Repositories.Generic;
using LeaderReport.Models;
using OfficeOpenXml;
using System.Globalization;

namespace LeaderReport.Application.Services;

public class FileProcessService : IFileProcessService
{
    private readonly IFileRepository _fileRepository;
    private readonly IGenericRepository<Producto> _genericProducto;
    private readonly IGenericRepository<Archivo> _genericArchivo;
    private readonly IGenericRepository<Marca> _genericMarca;
    private readonly IGenericRepository<Categoria> _genericCategoria;
    private readonly IGenericRepository<Presentacion> _genericPresentacion;

    public FileProcessService(IFileRepository fileRepository, IGenericRepository<Producto> genericProducto, IGenericRepository<Archivo> genericArchivo, IGenericRepository<Marca> genericMarca, IGenericRepository<Categoria> genericCategoria, IGenericRepository<Presentacion> genericPresentacion)
    {
        _fileRepository = fileRepository;
        _genericProducto = genericProducto;
        _genericArchivo = genericArchivo;
        _genericCategoria = genericCategoria;
        _genericMarca = genericMarca;
        _genericPresentacion = genericPresentacion;
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

        using var package = new ExcelPackage(new FileInfo(filePath));
        var worksheet = package.Workbook.Worksheets[0];

        int processedCount = 0;
        int errorsCount = 0;

        for (int row = 2; row <= worksheet.Dimension.Rows; row++)
        {
            try
            {
                string? codBarra = worksheet.Cells[row, 1].Text;
                string? nombreProducto = worksheet.Cells[row, 2].Text;
                string? marca = worksheet.Cells[row, 3].Text;
                string? categoria = worksheet.Cells[row, 4].Text;
                decimal precio = worksheet.Cells[row, 7].GetValue<decimal>();
                precio = Math.Round(precio, 2);


                if (string.IsNullOrWhiteSpace(nombreProducto))
                {
                    Console.WriteLine($"⚠ Error en la fila {row}: Datos inválidos.");
                    errorsCount++;
                    continue;
                }

                // 🔹 Buscar Marca y Categoría en BD o crear si no existen
                /*var marca = (await _genericMarca.GetAll()).FirstOrDefault(m => m.Nombre == nombreMarca)
            ?? new Marca { Nombre = nombreMarca };

                var categoria = (await _genericCategoria.GetAll()).FirstOrDefault(c => c.Nombre == nombreCategoria)
                                ?? new Categoria { Nombre = nombreCategoria };*/

                // 🔹 Buscar si el producto ya existe
                var productoExistente = (await _genericProducto.GetAll()).FirstOrDefault(p => p.Nombre == nombreProducto);

                if (productoExistente == null)
                {
                    // 🔹 Crear nuevo Producto
                    var nuevoProducto = new Producto
                    {
                        Cod_Barra = codBarra,
                        Nombre = nombreProducto,
                        Marca = new Marca{

                            Nombre = marca
                        },
                        Categoria = new Categoria{
                            Nombre = categoria
                        },
                        Presentaciones = new List<Presentacion>
                    {
                        new Presentacion
                        {
                            
                            Precio = precio
                        }
                    }
                    };

                    await _genericProducto.Add(nuevoProducto);
                    Console.WriteLine($"Producto guardado con ID: {nuevoProducto.Id}");
                    Console.WriteLine($"Precio guardado: {nuevoProducto.Presentaciones.First().Precio}");
                }
                else
                {
                    // 🔹 Agregar nueva Presentación si el producto ya existe
                    var nuevaPresentacion = new Presentacion
                    {
                        ProductoId = productoExistente.Id,
                        Precio = precio,

                    };

                    await _genericPresentacion.Add(nuevaPresentacion);
                    var presentacionGuardada = await _genericPresentacion.FirstOrDefault(p => p.Id == nuevaPresentacion.Id);
                    Console.WriteLine($"Presentación guardada con precio: {presentacionGuardada?.Precio}");
                    
                }

                processedCount++;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠ Error en la fila {row}: {ex.Message}");
                errorsCount++;
            }
        }

        return (processedCount, errorsCount);
    }

}
