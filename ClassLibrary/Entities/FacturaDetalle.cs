using System;

namespace LeaderReport.Models;

public class FacturaDetalle
{
    public int Id { get; set; }

    // Relación con la factura
    public int FacturaId { get; set; }
    public Factura Factura { get; set; }

    // Relación con Presentación del producto
    public int PresentacionId { get; set; }
    public Presentacion Presentacion { get; set; }

    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Total { get; set; }
}
