using System;

namespace LeaderReport.Models;

public class Proveedor
{
    public int Id { get; set; }
    public string DNI { get; set; }
    public string Empresa { get; set; }
    public string Nombre { get; set; }
    public string Email { get; set; }
    public string? Direccion { get; set; }

    // Relación con CategoriaProveedor
    public int CategoriaProveedorId { get; set; }
    public CategoriaProveedor CategoriaProveedor { get; set; }

    // Relación con productos
    public ICollection<Producto>? Productos { get; set; }
}
