using System;

namespace LeaderReport.Models;

public class ReporteGenerado
{
    public int Id { get; set; }
    
    // Relación con Reportes
    public int ReporteId { get; set; }
    public Reporte Reporte { get; set; }

    // Relación con Usuario
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }

    public DateTime FechaGeneracion { get; set; }
    public string ParametrosUsados { get; set; }
}
