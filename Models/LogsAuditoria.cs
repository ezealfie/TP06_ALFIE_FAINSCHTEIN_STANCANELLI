/* Clase: LogsAuditoria
Id (int)
EmpleadoId (int)
Legajo (string)
Accion (string)
FechaHora (DateTime)	
*/
public class LogsAuditoria
{
    public int Id { get; set; }
    public int EmpleadoId { get; set; }
    public string Legajo { get; set; }
    public string Accion { get; set; }
    public DateTime FechaHora { get; set; }

    public LogsAuditoria(int id, int empleadoId, string legajo, string accion, DateTime fechaHora)
    {
        Id = id;
        EmpleadoId = empleadoId;
        Legajo = legajo;
        Accion = accion;
        FechaHora = fechaHora;
    }

    public LogsAuditoria()
    {
        Legajo = string.Empty;
        Accion = string.Empty;
    }
}