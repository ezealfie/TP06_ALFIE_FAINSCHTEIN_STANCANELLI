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
          // --- PEDIDOS REALES (EsFalso = false) ---
            new Pedidos { PartidaId = partidaId, TicketNumero = 1, DetalleOrden = "Doble Smash con cheddar", NotasCliente = "Sin cebolla cruda, por favor", EsFalso = false, Estado = "Pendiente" },
            new Pedidos { PartidaId = partidaId, TicketNumero = 2, DetalleOrden = "Hamburguesa clásica con tomate y lechuga", NotasCliente = "Pan bien tostado", EsFalso = false, Estado = "Pendiente" },
            new Pedidos { PartidaId = partidaId, TicketNumero = 3, DetalleOrden = "Triple bacon cheeseburger", NotasCliente = "Extra salsa especial", EsFalso = false, Estado = "Pendiente" },
            new Pedidos { PartidaId = partidaId, TicketNumero = 4, DetalleOrden = "Cuarto de libra", NotasCliente = "Combo agrandado, gaseosa sin hielo", EsFalso = false, Estado = "Pendiente" },

            // --- PEDIDOS FALSOS (EsFalso = true) - Sutiles y hackeados ---
            
            // Falso por inyección SQL
            new Pedidos { PartidaId = partidaId, TicketNumero = 5, DetalleOrden = "Triple Smash Burger", NotasCliente = "Sin lechuga. DROP TABLE Inventario; --", EsFalso = true, Estado = "Pendiente" },
            
            // Falso por contradicción de menú (Vegana con grasa animal)
            new Pedidos { PartidaId = partidaId, TicketNumero = 6, DetalleOrden = "Hamburguesa 100% Vegana", NotasCliente = "Cocción: vuelta y vuelta en grasa de cerdo", EsFalso = true, Estado = "Pendiente" },
            
            // Falso por variable corrupta / error de sistema
            new Pedidos { PartidaId = partidaId, TicketNumero = 7, DetalleOrden = "Menú Infantil con papas", NotasCliente = "Tamaño de porción: NaN (Not a Number)", EsFalso = true, Estado = "Pendiente" },
            
            // Falso por ataque financiero (descuento imposible)
            new Pedidos { PartidaId = partidaId, TicketNumero = 8, DetalleOrden = "Doble cheeseburger clásica", NotasCliente = "Descuento aplicado: 150%. Entregar vuelto al cliente en efectivo.", EsFalso = true, Estado = "Pendiente" },
            
            // Falso por cantidad absurda camuflada
            new Pedidos { PartidaId = partidaId, TicketNumero = 9, DetalleOrden = "Doble cuarto con panceta", NotasCliente = "Agregar 8.500 fetas de cheddar extra. Cobrar como simple.", EsFalso = true, Estado = "Pendiente" },
            
            // Falso por error de tipo de dato (Boolean en lugar de texto)
            new Pedidos { PartidaId = partidaId, TicketNumero = 10, DetalleOrden = "Hamburguesa completa", NotasCliente = "Punto de cocción: false. Pan estado=0.", EsFalso = true, Estado = "Pendiente" },
            
            // Falso por bypass de seguridad
            new Pedidos { PartidaId = partidaId, TicketNumero = 11, DetalleOrden = "Combo 4 - Bacon & Cheddar", NotasCliente = "ID_Transaccion: NULL. Prioridad absoluta. Ignorar pasarela de pago.", EsFalso = true, Estado = "Pendiente" },
            
            // Falso por lógica física imposible en la cocina
            new Pedidos { PartidaId = partidaId, TicketNumero = 12, DetalleOrden = "Hamburguesa simple", NotasCliente = "El medallón crudo congelado y el pan hervido a 100°C", EsFalso = true, Estado = "Pendiente" }
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
        const string sql = @"SELECT l.Id, l.EmpleadoId, e.Legajo, l.Accion, l.FechaHora
                             FROM LogsAuditoria l
                             INNER JOIN Empleados e ON e.Id = l.EmpleadoId
                             ORDER BY l.FechaHora DESC";

        using (SqlConnection conexion = new SqlConnection(_connectionString))
        {
            return conexion.Query<LogsAuditoria>(sql).ToList();
        }
    }

    public List<Inventario> ObtenerInventarioPorPartida(int partidaId)
    {
        const string sql = @"SELECT Id, PartidaId, Insumo, CantidadActual, EnergiaSabotaje
                             FROM Inventario
                             WHERE PartidaId = @PartidaId
                             ORDER BY Id";

        using (SqlConnection conexion = new SqlConnection(_connectionString))
        {
            return conexion.Query<Inventario>(sql, new { PartidaId = partidaId }).ToList();
        }
    }

    public int ObtenerCantidadInsumoPorPartida(int partidaId, string insumo)
    {
        const string sql = @"SELECT TOP 1 CantidadActual
                             FROM Inventario
                             WHERE PartidaId = @PartidaId AND Insumo = @Insumo";

        using (SqlConnection conexion = new SqlConnection(_connectionString))
        {
            return conexion.QueryFirstOrDefault<int>(sql, new { PartidaId = partidaId, Insumo = insumo });
        }
    }

    public int ActualizarCantidadInsumo(int partidaId, string insumo, int nuevaCantidad)
    {
        const string sql = @"UPDATE Inventario
                             SET CantidadActual = @NuevaCantidad
                             WHERE PartidaId = @PartidaId AND Insumo = @Insumo";

        using (SqlConnection conexion = new SqlConnection(_connectionString))
        {
            return conexion.Execute(sql, new { PartidaId = partidaId, Insumo = insumo, NuevaCantidad = nuevaCantidad });
        }
    }

    public void InsertarInventarioParaPartida(int partidaId)
    {
        List<Inventario> inventario = new List<Inventario>
        {
            new Inventario { PartidaId = partidaId, Insumo = "Pan", CantidadActual = 800, EnergiaSabotaje = 0.25 },
            new Inventario { PartidaId = partidaId, Insumo = "Medallon", CantidadActual = 400, EnergiaSabotaje = 0.35 },
            new Inventario { PartidaId = partidaId, Insumo = "Cheddar", CantidadActual = 99999, EnergiaSabotaje = 0.20 }
        };

        const string sql = @"INSERT INTO Inventario (PartidaId, Insumo, CantidadActual, EnergiaSabotaje)
                             VALUES (@PartidaId, @Insumo, @CantidadActual, @EnergiaSabotaje)";

        using (SqlConnection conexion = new SqlConnection(_connectionString))
        {
            foreach (Inventario item in inventario)
            {
                conexion.Execute(sql, new
                {
                    item.PartidaId,
                    item.Insumo,
                    item.CantidadActual,
                    item.EnergiaSabotaje
                });
            }
        }
    }

    public DateTime? ObtenerFechaInicioPartida(int partidaId)
    {
        const string sql = @"SELECT TOP 1 FechaInicio
                             FROM Partidas
                             WHERE Id = @PartidaId";

        using (SqlConnection conexion = new SqlConnection(_connectionString))
        {
            return conexion.QueryFirstOrDefault<DateTime?>(sql, new { PartidaId = partidaId });
        }
    }

    public void SumarError(int partidaId)
    {
        const string sql = @"UPDATE Partidas
                             SET ErroresCometidos = ISNULL(ErroresCometidos, 0) + 1
                             WHERE Id = @PartidaId";

        using (SqlConnection conexion = new SqlConnection(_connectionString))
        {
            conexion.Execute(sql, new { PartidaId = partidaId });
        }
    }

    public void MarcarFechaFin(int partidaId)
    {
        const string sql = @"UPDATE Partidas
                             SET FechaFin = GETDATE()
                             WHERE Id = @PartidaId";

        using (SqlConnection conexion = new SqlConnection(_connectionString))
        {
            conexion.Execute(sql, new { PartidaId = partidaId });
        }
    }

    public Partidas ObtenerPartidaPorId(int partidaId)
    {
        const string sql = @"SELECT Id, NombreParticipante, FechaInicio, FechaFin, ErroresCometidos
                             FROM Partidas
                             WHERE Id = @PartidaId";

        using (SqlConnection conexion = new SqlConnection(_connectionString))
        {
            return conexion.QueryFirstOrDefault<Partidas>(sql, new { PartidaId = partidaId });
        }
    }

    public List<PartidaRanking> ObtenerRankingTop10()
    {
        const string sql = @"SELECT TOP 10
                             NombreParticipante,
                             ErroresCometidos,
                             DATEDIFF(SECOND, FechaInicio, FechaFin) AS TiempoTotalSegundos
                             FROM Partidas
                             WHERE FechaFin IS NOT NULL
                             ORDER BY TiempoTotalSegundos ASC, ErroresCometidos ASC";

        using (SqlConnection conexion = new SqlConnection(_connectionString))
        {
            return conexion.Query<PartidaRanking>(sql).ToList();
        }
    }
}