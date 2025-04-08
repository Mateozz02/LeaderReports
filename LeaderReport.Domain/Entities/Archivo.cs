using System;

namespace LeaderReport.Domain.Entities;

public class Archivo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nombre { get; set; } = string.Empty;
        public string Ruta { get; set; } = string.Empty;
        public DateTime FechaSubida { get; set; } = DateTime.UtcNow;
    }
