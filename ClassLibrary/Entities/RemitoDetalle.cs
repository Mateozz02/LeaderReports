using System;

namespace LeaderReport.Models;

public class RemitoDetalle
{
    public int Id { get; set; }

    // Relación con el remito
    public int RemitoId { get; set; }
    public Remito Remito { get; set; }

    // Relación con Presentación del producto
    public int PresentacionId { get; set; }
    public Presentacion Presentacion { get; set; }

    public int Cantidad { get; set; }
}
