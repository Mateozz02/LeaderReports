using System;

namespace LeaderReport.Models;

public class Categoria
{
    public int Id { get; set; }
    public string Nombre { get; set; }

    // Relación con productos
    public ICollection<Producto> Productos { get; set; }
}
