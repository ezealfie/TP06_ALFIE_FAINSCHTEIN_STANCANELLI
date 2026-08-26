/* Clase: LogsAuditoria
Id (int)
EmpleadoId (int)
Accion (string)
FechaHora (DateTime)	
*/
public class LogsAuditoria
{
    public int Id { get; set; }
    public int EmpleadoId { get; set; }
    public string Accion { get; set; }
    public DateTime FechaHora { get; set; }

    public LogsAuditoria(int id, int empleadoId, string accion, DateTime fechaHora)
    {
        Id = id;
        EmpleadoId = empleadoId;
        Accion = accion;
        FechaHora = fechaHora;
    }

    public LogsAuditoria()
    {

    }
}