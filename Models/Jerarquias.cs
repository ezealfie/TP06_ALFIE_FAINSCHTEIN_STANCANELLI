/* Clase: Jerarquias
Id (int)
NombreRol (string)
NivelAcceso (int)
*/
public class Jerarquias
{
    public int Id { get; set; }
    public string NombreRol { get; set; }
    public int NivelAcceso { get; set; }

    public Jerarquias(int id, string nombreRol, int nivelAcceso)
    {
        Id = id;
        NombreRol = nombreRol;
        NivelAcceso = nivelAcceso;
    }

    public Jerarquias()
    {

    }
}