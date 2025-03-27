using System;

namespace LeaderReport.Models;

public class Factura
{
    public int Id { get; set; }
    public string Numero { get; set; }
    public DateTime Fecha { get; set; }
    
    // Relación con Cliente
    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; }

    // Relación con Usuario (quién generó la factura)
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }

    // Detalles de la factura
    public ICollection<FacturaDetalle> Detalles { get; set; }

    public decimal Total { get; set; }
}

