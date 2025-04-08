using LeaderReport.Domain.Entities;
using LeaderReport.Models;
using Microsoft.EntityFrameworkCore;

namespace LeaderReport.Data;

public class LeaderReportContext : DbContext
{
    public LeaderReportContext()
    {
    }

    public LeaderReportContext(DbContextOptions<LeaderReportContext> options)
        : base(options)
    {
    }
    public DbSet<Marca> Marcas { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Producto> Productos { get; set; }
    public DbSet<Proveedor> Proveedores { get; set; }
    public DbSet<CategoriaProveedor> CategoriaProveedores { get; set; }
    public DbSet<Presentacion> Presentaciones { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Reporte> Reportes { get; set; }
    public DbSet<ReporteGenerado> ReportesGenerados { get; set; }
    public DbSet<Factura> Facturas { get; set; }
    public DbSet<FacturaDetalle> FacturaDetalles { get; set; }
    public DbSet<Cotizacion> Cotizaciones { get; set; }
    public DbSet<CotizacionDetalle> CotizacionDetalles { get; set; }
    public DbSet<Remito> Remitos { get; set; }
    public DbSet<RemitoDetalle> RemitosDetalles { get; set; }
    public DbSet<Archivo> Archivos { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configurar para Factura.Total
        modelBuilder.Entity<Factura>()
            .Property(f => f.Total)
            .HasColumnType("decimal(18,2)");

        // Configurar para FacturaDetalle
        modelBuilder.Entity<FacturaDetalle>()
            .Property(fd => fd.PrecioUnitario)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<FacturaDetalle>()
            .Property(fd => fd.Total)
            .HasColumnType("decimal(18,2)");

        // Configurar para Cotizacion.Total
        modelBuilder.Entity<Cotizacion>()
            .Property(c => c.Total)
            .HasColumnType("decimal(18,2)");

        // Configurar para CotizacionDetalle
        modelBuilder.Entity<CotizacionDetalle>()
            .Property(cd => cd.PrecioUnitario)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<CotizacionDetalle>()
            .Property(cd => cd.Total)
            .HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Presentacion>()
            .Property(cd => cd.Precio)
            .HasColumnType("decimal(18,2)");
    }

}
