using System;

namespace LeaderReport.Models;

public class Presentacion
{
    public int Id { get; set; }
    public int ProductoId { get; set; }
    public Producto Producto { get; set; }
    public string Unidad { get; set; }
    public int Cantidad { get; set; }
    public int Stock { get; set; }
    public int Precio { get; set; }
}
