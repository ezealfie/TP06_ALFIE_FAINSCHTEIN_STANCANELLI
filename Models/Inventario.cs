/* Tabla: Inventario
Id (int)
PartidaId (int)
Insumo (string)
CantidadActual (int)
EnergiaSabotaje (double)  

*/
public class Inventario
{
    public int Id { get; set; }
    public int PartidaId { get; set; }
    public string Insumo { get; set; }
    public int CantidadActual { get; set; }
    public double EnergiaSabotaje { get; set; }

    public Inventario(int id, int partidaId, string insumo, int cantidadActual, double energiaSabotaje)
    {
        Id = id;
        PartidaId = partidaId;
        Insumo = insumo;
        CantidadActual = cantidadActual;
        EnergiaSabotaje = energiaSabotaje;
    }

    public Inventario()
    {

    }
}
