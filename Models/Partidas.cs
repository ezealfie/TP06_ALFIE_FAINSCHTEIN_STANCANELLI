public class Partidas
{
    public int Id { get; set; }
    public string NombreParticipante { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public int ErroresCometidos { get; set; }

    public Partidas(int id, string nombreParticipante, DateTime fechaInicio, DateTime? fechaFin, int erroresCometidos)
    {
        Id = id;
        NombreParticipante = nombreParticipante;
        FechaInicio = fechaInicio;
        FechaFin = fechaFin;
        ErroresCometidos = erroresCometidos;
    }

    public Partidas(string nombreParticipante, DateTime fechaInicio)
    {
        NombreParticipante = nombreParticipante;
        FechaInicio = fechaInicio;
        ErroresCometidos = 0;
    }

    public Partidas()
    {
        NombreParticipante = string.Empty;
        ErroresCometidos = 0;
    }
}

public class PartidaRanking
{
    public string NombreParticipante { get; set; }
    public int ErroresCometidos { get; set; }
    public int TiempoTotalSegundos { get; set; }

    public PartidaRanking()
    {
        NombreParticipante = string.Empty;
    }
}
