public class VictoriaViewModel
{
    public Partidas PartidaActual { get; set; }
    public List<PartidaRanking> Ranking { get; set; }

    public VictoriaViewModel()
    {
        PartidaActual = new Partidas();
        Ranking = new List<PartidaRanking>();
    }
}
