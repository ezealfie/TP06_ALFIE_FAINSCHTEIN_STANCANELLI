public class Partidas
{
    public int Id { get; set; }
    public string NombreParticipante { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public Partidas(int id, string nombreParticipante, DateTime fechaInicio, DateTime? fechaFin)
    {
        Id = id;
        NombreParticipante = nombreParticipante;
        FechaInicio = fechaInicio;
        FechaFin = fechaFin;
    }
    
    public Partidas(string nombreParticipante, DateTime fechaInicio)
    {
        NombreParticipante = nombreParticipante;
        FechaInicio = fechaInicio;
    }
    
    public Partidas()
    {

    }

}
