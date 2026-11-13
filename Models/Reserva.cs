namespace tpfinal.Models;

public class Reserva
{
    public int Id {get; set;}
    public int IdUsuario {get; set;}
    public int IdEspacio {get; set;}
    public DateTime FechaInicio {get; set;}
    public DateTime FechaCancelacion {get; set;}
    public bool cancelado {get; set;}
    public int CantidadHoras { get; set; }
}