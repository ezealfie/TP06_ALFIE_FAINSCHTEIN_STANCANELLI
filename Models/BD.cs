using Dapper;
using Microsoft.Data.SqlClient;

public class BD
{
    private readonly string _connectionString = @"Server=localhost;Database=TP06;Integrated Security=True;TrustServerCertificate=True;";

    public void IniciarPartida(Partidas partida)
    {
        const string sql = @"INSERT INTO Partidas (NombreParticipante, FechaInicio) VALUES (@NombreParticipante, @FechaInicio)";

        using (SqlConnection conexion = new SqlConnection(_connectionString))
        {
            conexion.Execute(sql, new { partida.NombreParticipante, partida.FechaInicio });
        }
    }

    public void InsertarPedidosParaPartida(int partidaId)
    {
        const string sql = @"
            INSERT INTO Pedidos (PartidaId, DetalleOrden, NotasCliente, EsFalso, Estado)
            VALUES (@PartidaId, @DetalleOrden, @NotasCliente, @EsFalso, @Estado);";

        var pedidos = new List<Pedidos>
        {
            new Pedidos { PartidaId = partidaId, TicketNumero = 1, DetalleOrden = "Doble con cheddar", NotasCliente = "Sin cebolla", EsFalso = false, Estado = "Pendiente" },
            new Pedidos { PartidaId = partidaId, TicketNumero = 2, DetalleOrden = "Hamburguesa clásica con tomate y lechuga", NotasCliente = "Pan bien tostado", EsFalso = false, Estado = "Pendiente" },
            new Pedidos { PartidaId = partidaId, TicketNumero = 3, DetalleOrden = "Triple bacon cheeseburger", NotasCliente = "Extra queso", EsFalso = false, Estado = "Pendiente" },
            new Pedidos { PartidaId = partidaId, TicketNumero = 4, DetalleOrden = "Hamburguesa completa con jamón y huevo", NotasCliente = "Sin pepinillos", EsFalso = false, Estado = "Pendiente" },
            new Pedidos { PartidaId = partidaId, TicketNumero = 5, DetalleOrden = "Hamburguesa con helado de pistacho y ruedas de bicicleta", NotasCliente = "Que venga con salsa de neón", EsFalso = true, Estado = "Pendiente" },
            new Pedidos { PartidaId = partidaId, TicketNumero = 6, DetalleOrden = "Doble pan, triple nube y papas dentro del vaso", NotasCliente = "Sin gravedad, por favor", EsFalso = true, Estado = "Pendiente" },
            new Pedidos { PartidaId = partidaId, TicketNumero = 7, DetalleOrden = "Burger de acero con queso cósmico y mayonesa lunar", NotasCliente = "Entregar en modo sigiloso", EsFalso = true, Estado = "Pendiente" },
            new Pedidos { PartidaId = partidaId, TicketNumero = 8, DetalleOrden = "Hamburguesa invertida con ketchup cuántico", NotasCliente = "A temperatura del tiempo", EsFalso = true, Estado = "Pendiente" },
            new Pedidos { PartidaId = partidaId, TicketNumero = 9, DetalleOrden = "Sándwich de hamburguesa relleno con cables USB", NotasCliente = "Necesito doble enchufe", EsFalso = true, Estado = "Pendiente" },
            new Pedidos { PartidaId = partidaId, TicketNumero = 10, DetalleOrden = "Burger de lava con anillos de cebolla teleportados", NotasCliente = "Que no se derrita el universo", EsFalso = true, Estado = "Pendiente" },
            new Pedidos { PartidaId = partidaId, TicketNumero = 11, DetalleOrden = "Hamburguesa de algodón de azúcar y motor diesel", NotasCliente = "Con energía ilimitada", EsFalso = true, Estado = "Pendiente" },
            new Pedidos { PartidaId = partidaId, TicketNumero = 12, DetalleOrden = "Mega burger con martillo neumático y pepinillos en órbita", NotasCliente = "Urgente: romper la realidad", EsFalso = true, Estado = "Pendiente" }
        };

        using (SqlConnection conexion = new SqlConnection(_connectionString))
        {
            conexion.Open();

            using (var transaction = conexion.BeginTransaction())
            {
                try
                {
                    conexion.Execute(sql, pedidos.Select(p => new
                    {
                        p.TicketNumero,
                        p.PartidaId,
                        p.DetalleOrden,
                        p.NotasCliente,
                        p.EsFalso,
                        p.Estado
                    }), transaction);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}