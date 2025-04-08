using System;

namespace LeaderReport.Models;

public class Remito
{
    public int Id { get; set; }
    public string Numero { get; set; }
    public DateTime Fecha { get; set; }

    // Relación con Cliente
    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; }

    // Relación con Usuario (quién generó el remito)
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }

    // Detalles del remito
    public ICollection<RemitoDetalle> Detalles { get; set; }
}
