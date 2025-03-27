using System;

namespace LeaderReport.Models;

public class CotizacionDetalle
{
    public int Id { get; set; }

    // Relación con la cotización
    public int CotizacionId { get; set; }
    public Cotizacion Cotizacion { get; set; }

    // Relación con Presentación del producto
    public int PresentacionId { get; set; }
    public Presentacion Presentacion { get; set; }

    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Total { get; set; }
}
