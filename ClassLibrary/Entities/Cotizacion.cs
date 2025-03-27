using System;

namespace LeaderReport.Models;

public class Cotizacion
{
    public int Id { get; set; }
    public string Numero { get; set; }
    public DateTime Fecha { get; set; }

    // Relación con Cliente
    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; }

    // Relación con Usuario (quién generó la cotización)
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }

    // Detalles de la cotización
    public ICollection<CotizacionDetalle> Detalles { get; set; }

    public decimal Total { get; set; }
}

