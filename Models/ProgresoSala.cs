/* Clase: ProgresoSala
Id (int)
PartidaId (int)
SalaId (int)
Intentos (int)
Resuelto (bool)
FechaAcceso (DateTime)
*/
public class ProgresoSala
{
    public int Id { get; set; }
    public int PartidaId { get; set; }
    public int SalaId { get; set; }
    public int Intentos { get; set; }
    public bool Resuelto { get; set; }
    public DateTime FechaAcceso { get; set; }

    public ProgresoSala(int id, int partidaId, int salaId, int intentos, bool resuelto, DateTime fechaAcceso)
    {
        Id = id;
        PartidaId = partidaId;
        SalaId = salaId;
        Intentos = intentos;
        Resuelto = resuelto;
        FechaAcceso = fechaAcceso;
    }

    public ProgresoSala()
    {

    }
}
