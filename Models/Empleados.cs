/* clase: Empleados
Id (int)
Legajo (string)
Nombre (string)
JerarquiaId (int)
*/
public class Empleados
{
    public int Id { get; set; }
    public string Legajo { get; set; }
    public string Nombre { get; set; }
    public int JerarquiaId { get; set; }

    public Empleados(int id, string legajo, string nombre, int jerarquiaId)
    {
        Id = id;
        Legajo = legajo;
        Nombre = nombre;
        JerarquiaId = jerarquiaId;
    }

    public Empleados()
    {

    }
}
