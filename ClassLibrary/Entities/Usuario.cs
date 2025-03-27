using System;

namespace LeaderReport.Models;
public class Usuario
{
    public int Id { get; set; }
    public string DNI { get; set; }
    public string Nombre { get; set; }
    public string Email { get; set; }
    public string Direccion { get; set; }

    // Relación con ReportesGenerados
    public ICollection<ReporteGenerado>? ReportesGenerados { get; set; }
}
