using System;

namespace LeaderReport.Models;

public class Cliente
{
    public int Id { get; set; }
    public string DNI { get; set; }
    public string Nombre { get; set; }
    public string Email { get; set; }
    public string Direccion { get; set; }
    public int? Fidelidad { get; set; }
}
