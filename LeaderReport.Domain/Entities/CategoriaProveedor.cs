using System;

namespace LeaderReport.Models;
public class CategoriaProveedor
{
    public int Id { get; set; }
    public string Categoria { get; set; }

    // Relación con proveedores
    public ICollection<Proveedor> Proveedores { get; set; }
}

