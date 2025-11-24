namespace ApiRegistrosDiarios.Models;

public class RegistroDiario
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public string Actividad { get; set; } = "";
    public string? Descripcion { get; set; }
    public decimal Horas { get; set; }
    public string Responsable { get; set; } = "";
    public string? Observaciones { get; set; }
}
