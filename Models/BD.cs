using Dapper;
using Microsoft.Data.SqlClient;

public class BD
{
    private string _connectionString = @"Server=localhost;Database=TP06;Integrated Security=True;TrustServerCertificate=True;";

    public int IniciarPartida(Partidas partida)
    {
        const string sqlInsert = @"INSERT INTO Partidas (NombreParticipante, FechaInicio) VALUES (@NombreParticipante, @FechaInicio)";
        const string sqlSelect = @"SELECT TOP 1 Id
                                   FROM Partidas
                                   WHERE NombreParticipante = @NombreParticipante AND FechaInicio = @FechaInicio
                                   ORDER BY Id DESC";

        using (SqlConnection conexion = new SqlConnection(_connectionString))
        {
            conexion.Execute(sqlInsert, new { partida.NombreParticipante, partida.FechaInicio });
            return conexion.QueryFirstOrDefault<int>(sqlSelect, new { partida.NombreParticipante, partida.FechaInicio });
        }
    }

    public List<Pedidos> ObtenerPedidosPendientes(int partidaId)
    {
        const string sql = @"SELECT Id, PartidaId, TicketNumero, DetalleOrden, NotasCliente, EsFalso, Estado
                             FROM Pedidos
                             WHERE PartidaId = @PartidaId AND Estado = 'Pendiente'";

        using (SqlConnection conexion = new SqlConnection(_connectionString))
        {
            return conexion.Query<Pedidos>(sql, new { PartidaId = partidaId }).ToList();
        }
    }

    public bool TodosLosPedidosSonFalsos(int partidaId, int[] ticketsSeleccionados)
    {
        if (ticketsSeleccionados == null || ticketsSeleccionados.Length == 0)
        {
            return false;
        }

        const string sql = @"SELECT COUNT(1)
                             FROM Pedidos
                             WHERE PartidaId = @PartidaId
                               AND Id IN @Ids
                               AND EsFalso = 1";

        using (SqlConnection conexion = new SqlConnection(_connectionString))
        {
            int cantidadFalsos = conexion.QueryFirstOrDefault<int>(sql, new { PartidaId = partidaId, Ids = ticketsSeleccionados });
            return cantidadFalsos == ticketsSeleccionados.Length;
        }
    }

    public int ContarPedidosFalsos(int partidaId)
    {
        const string sql = @"SELECT COUNT(1)
                             FROM Pedidos
                             WHERE PartidaId = @PartidaId
                               AND EsFalso = 1
                               AND Estado = 'Pendiente'";

        using (SqlConnection conexion = new SqlConnection(_connectionString))
        {
            return conexion.QueryFirstOrDefault<int>(sql, new { PartidaId = partidaId });
        }
    }

    public int CancelarPedidos(int partidaId, int[] ticketsSeleccionados)
    {
        const string sql = @"UPDATE Pedidos
                             SET Estado = 'Cancelado'
                             WHERE PartidaId = @PartidaId
                               AND Id IN @Ids";

        using (SqlConnection conexion = new SqlConnection(_connectionString))
        {
            return conexion.Execute(sql, new { PartidaId = partidaId, Ids = ticketsSeleccionados });
        }
    }

    public void InsertarPedidosParaPartida(int partidaId)
    {
        List<Pedidos> pedidos = new List<Pedidos>
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

        const string sql = @"INSERT INTO Pedidos (PartidaId, TicketNumero, DetalleOrden, NotasCliente, EsFalso, Estado) VALUES (@PartidaId, @TicketNumero, @DetalleOrden, @NotasCliente, @EsFalso, @Estado)";

        using (SqlConnection conexion = new SqlConnection(_connectionString))
        {
            foreach (Pedidos pedido in pedidos)
            {
                conexion.Execute(sql, new
                {
                    pedido.PartidaId,
                    pedido.TicketNumero,
                    pedido.DetalleOrden,
                    pedido.NotasCliente,
                    pedido.EsFalso,
                    pedido.Estado
                });
            }
        }
    }

    public bool MarcarSalaResuelta(int partidaId, int salaId)
    {
        const string sqlCheck = @"SELECT COUNT(1) FROM ProgresoSala 
                                 WHERE PartidaId = @PartidaId AND SalaId = @SalaId";
        
        using (SqlConnection conexion = new SqlConnection(_connectionString))
        {
            int existe = conexion.QueryFirstOrDefault<int>(sqlCheck, 
                new { PartidaId = partidaId, SalaId = salaId });

            if (existe > 0)
            {
                const string sqlUpdate = @"UPDATE ProgresoSala 
                                          SET Resuelto = 1, FechaAcceso = GETDATE() 
                                          WHERE PartidaId = @PartidaId AND SalaId = @SalaId";
                conexion.Execute(sqlUpdate, new { PartidaId = partidaId, SalaId = salaId });
            }
            else
            {
                const string sqlInsert = @"INSERT INTO ProgresoSala 
                                          (PartidaId, SalaId, Intentos, Resuelto, FechaAcceso)
                                          VALUES (@PartidaId, @SalaId, 1, 1, GETDATE())";
                conexion.Execute(sqlInsert, new { PartidaId = partidaId, SalaId = salaId });
            }

            return true;
        }
    }

    public bool VerificarSalaResuelta(int partidaId, int salaId)
    {
        const string sql = @"SELECT COUNT(1) FROM ProgresoSala 
                            WHERE PartidaId = @PartidaId AND SalaId = @SalaId AND Resuelto = 1";

        using (SqlConnection conexion = new SqlConnection(_connectionString))
        {
            int resultado = conexion.QueryFirstOrDefault<int>(sql, 
                new { PartidaId = partidaId, SalaId = salaId });
            return resultado > 0;
        }
    }

    public bool VerificarSupervisor(string legajo)
    {
        const string sql = @"SELECT COUNT(1) FROM Empleados e
                             INNER JOIN Jerarquias j ON e.JerarquiaId = j.Id
                             WHERE e.Legajo = @Legajo AND j.NombreRol = 'Supervisor' AND j.NivelAcceso = 2";

        using (SqlConnection conexion = new SqlConnection(_connectionString))
        {
            int resultado = conexion.QueryFirstOrDefault<int>(sql, new { Legajo = legajo });
            return resultado > 0;
        }
    }

    public List<EmpleadoConJerarquia> ObtenerEmpleadosConJerarquia()
    {
        const string sql = @"SELECT e.Id, e.Legajo, e.Nombre, j.NombreRol, j.NivelAcceso
                             FROM Empleados e
                             INNER JOIN Jerarquias j ON e.JerarquiaId = j.Id
                             ORDER BY e.Legajo";

        using (SqlConnection conexion = new SqlConnection(_connectionString))
        {
            return conexion.Query<EmpleadoConJerarquia>(sql).ToList();
        }
    }

    public List<LogsAuditoria> ObtenerLogsAuditoria()
    {
        const string sql = @"SELECT * FROM LogsAuditoria ORDER BY FechaHora DESC";

        using (SqlConnection conexion = new SqlConnection(_connectionString))
        {
            return conexion.Query<LogsAuditoria>(sql).ToList();
        }
    }
}