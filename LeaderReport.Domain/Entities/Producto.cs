using System;

namespace LeaderReport.Models;

public class Producto
{
    public int Id { get; set; }
    public string? Cod_Barra { get; set; }
    public string? Cod_Proveedor {get; set;} 
    // Relación con Proveedor
    public int? ProveedorId { get; set; }
    public Proveedor? Proveedor { get; set; }
    
    public string Nombre { get; set; }

    // Relación con Marca
    public int? MarcaId { get; set; }
    public Marca? Marca { get; set; }

    // Relación con Categoría
    public int? CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }

    // Relación con Presentaciones
    public ICollection<Presentacion>? Presentaciones { get; set; }
}


