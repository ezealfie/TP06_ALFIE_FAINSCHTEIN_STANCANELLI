/* Tabla: Inventario
Id (int)
PartidaId (int)
Insumo (string)
CantidadActual (int)
EnergiaSabotaje (double)
Proveedor (string) // visual, no persistido en la tabla actual
*/
public class Inventario
{
    public int Id { get; set; }
    public int PartidaId { get; set; }
    public string Insumo { get; set; }
    public int CantidadActual { get; set; }
    public double EnergiaSabotaje { get; set; }
    public string Proveedor { get; set; }

    public Inventario(int id, int partidaId, string insumo, int cantidadActual, double energiaSabotaje, string proveedor)
    {
        Id = id;
        PartidaId = partidaId;
        Insumo = insumo;
        CantidadActual = cantidadActual;
        EnergiaSabotaje = energiaSabotaje;
        Proveedor = proveedor;
    }

    public Inventario()
    {
        Insumo = string.Empty;
        Proveedor = string.Empty;
    }
}
