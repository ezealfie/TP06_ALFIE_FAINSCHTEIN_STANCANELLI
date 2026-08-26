/* Clase: Pedidos
Id (int)
PartidaId (int)
TicketNumero (int)
DetalleOrden (string)
NotasCliente (string)
EsFalso (bool) 
Estado (string) */

public class Pedidos
{
    public int Id { get; set; }
    public int PartidaId { get; set; }
    public int TicketNumero { get; set; }
    public string DetalleOrden { get; set; }
    public string NotasCliente { get; set; }
    public bool EsFalso { get; set; }
    public string Estado { get; set; }

    public Pedidos(int id, int partidaId, int ticketNumero, string detalleOrden, string notasCliente, bool esFalso, string estado)
    {
        Id = id;
        PartidaId = partidaId;
        TicketNumero = ticketNumero;
        DetalleOrden = detalleOrden;
        NotasCliente = notasCliente;
        EsFalso = esFalso;
        Estado = estado;
    }

    public Pedidos()
    {

    }
}