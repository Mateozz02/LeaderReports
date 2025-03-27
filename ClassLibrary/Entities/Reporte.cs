using System;

namespace LeaderReport.Models;

public class Reporte
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public string Tipo { get; set; }
    public string ParametrosDefinidos { get; set; }

    // Relación con ReportesGenerados
    public ICollection<ReporteGenerado> ReportesGenerados { get; set; }
}
