using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using Microsoft.Data.SqlClient;
using TP06.Models;
using System.Collections.Generic;
using System.Linq;

namespace TP06.Controllers;

public class DirectorioController : Controller
{
    private string _connectionString = @"Server=localhost;Database=TP06;Integrated Security=True;TrustServerCertificate=True;";

    [HttpGet]
    public IActionResult Investigar()
    {
        // Verificar seguridad: obtener PartidaId de Session
        string? partidaIdTexto = HttpContext.Session.GetString("PartidaId");

        if (!int.TryParse(partidaIdTexto, out int partidaId))
        {
            return RedirectToAction("Inicio", "Home");
        }

        // Verificar que el jugador haya completado la Sala 1
        using (SqlConnection conexion = new SqlConnection(_connectionString))
        {
            const string sqlVerify = @"SELECT COUNT(1) FROM ProgresoSala 
                                        WHERE PartidaId = @PartidaId AND SalaId = 1 AND Resuelto = 1";
            int completedSala1 = conexion.QueryFirstOrDefault<int>(sqlVerify, new { PartidaId = partidaId });

            if (completedSala1 == 0)
            {
                return RedirectToAction("Inicio", "Home");
            }
        }

        // Obtener empleados con sus jerarquías
        List<EmpleadoConJerarquia> empleados = ObtenerEmpleadosConJerarquia();
        
        // Obtener logs de auditoría
        List<LogsAuditoria> logs = ObtenerLogsAuditoria();

        var viewModel = new InvestigacionViewModel
        {
            Empleados = empleados,
            Logs = logs
        };

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult ValidarSospechoso(string legajo)
    {
        // Verificar seguridad: obtener PartidaId de Session
        string? partidaIdTexto = HttpContext.Session.GetString("PartidaId");

        if (!int.TryParse(partidaIdTexto, out int partidaId))
        {
            return RedirectToAction("Inicio", "Home");
        }

        using (SqlConnection conexion = new SqlConnection(_connectionString))
        {
            // Buscar empleado con ese legajo que sea Supervisor (Nivel 2)
            const string sql = @"SELECT COUNT(1) FROM Empleados e
                                 INNER JOIN Jerarquias j ON e.JerarquiaId = j.Id
                                 WHERE e.Legajo = @Legajo AND j.NombreRol = 'Supervisor' AND j.NivelAcceso = 2";

            int esValido = conexion.QueryFirstOrDefault<int>(sql, new { Legajo = legajo });

            if (esValido > 0)
            {
                // Éxito: actualizar ProgresoSala para Sala 2
                const string sqlUpdate = @"INSERT INTO ProgresoSala (PartidaId, SalaId, Intentos, Resuelto, FechaAcceso)
                                           VALUES (@PartidaId, 2, 1, 1, GETDATE())
                                           ON CONFLICT (PartidaId, SalaId) DO UPDATE SET Resuelto = 1, FechaAcceso = GETDATE()";

                try
                {
                    conexion.Execute(sqlUpdate, new { PartidaId = partidaId });
                }
                catch
                {
                    // SQL Server no tiene ON CONFLICT, usar approach diferente
                    const string sqlCheck = @"SELECT COUNT(1) FROM ProgresoSala 
                                             WHERE PartidaId = @PartidaId AND SalaId = 2";
                    int existe = conexion.QueryFirstOrDefault<int>(sqlCheck, new { PartidaId = partidaId });

                    if (existe > 0)
                    {
                        const string sqlUpdateExist = @"UPDATE ProgresoSala 
                                                       SET Resuelto = 1, FechaAcceso = GETDATE() 
                                                       WHERE PartidaId = @PartidaId AND SalaId = 2";
                        conexion.Execute(sqlUpdateExist, new { PartidaId = partidaId });
                    }
                    else
                    {
                        const string sqlInsert = @"INSERT INTO ProgresoSala (PartidaId, SalaId, Intentos, Resuelto, FechaAcceso)
                                                   VALUES (@PartidaId, 2, 1, 1, GETDATE())";
                        conexion.Execute(sqlInsert, new { PartidaId = partidaId });
                    }
                }

                return RedirectToAction("Reparar", "Inventario");
            }
            else
            {
                // Fallo: volver a la vista con error
                ViewBag.Error = "Credencial rechazada. El legajo no corresponde a un Supervisor de Nivel 2 o no existe.";
                
                var viewModel = new InvestigacionViewModel
                {
                    Empleados = ObtenerEmpleadosConJerarquia(),
                    Logs = ObtenerLogsAuditoria()
                };

                return View("Investigar", viewModel);
            }
        }
    }

    private List<EmpleadoConJerarquia> ObtenerEmpleadosConJerarquia()
    {
        using (SqlConnection conexion = new SqlConnection(_connectionString))
        {
            const string sql = @"SELECT e.Id, e.Legajo, e.Nombre, j.NombreRol, j.NivelAcceso
                                 FROM Empleados e
                                 INNER JOIN Jerarquias j ON e.JerarquiaId = j.Id
                                 ORDER BY e.Legajo";

            return conexion.Query<EmpleadoConJerarquia>(sql).ToList();
        }
    }

    private List<LogsAuditoria> ObtenerLogsAuditoria()
    {
        using (SqlConnection conexion = new SqlConnection(_connectionString))
        {
            const string sql = @"SELECT * FROM LogsAuditoria ORDER BY FechaHora DESC";

            return conexion.Query<LogsAuditoria>(sql).ToList();
        }
    }
}
