using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TP06.Models;

namespace TP06.Controllers;

public class DirectorioController : Controller
{
    [HttpGet]
    public IActionResult Investigar()
    {
        // Verificar seguridad: obtener PartidaId de Session
        string? partidaIdTexto = HttpContext.Session.GetString("PartidaId");

        if (!int.TryParse(partidaIdTexto, out int partidaId))
        {
            return RedirectToAction("Inicio", "Home");
        }

        // ✅ CANDADO DE SEGURIDAD: Verificar que Sala 1 esté resuelta
        BD bd = new BD();
        if (!bd.VerificarSalaResuelta(partidaId, 1))
        {
            return RedirectToAction("Tablero", "Cocina");
        }

        // Obtener empleados con sus jerarquías
        List<EmpleadoConJerarquia> empleados = bd.ObtenerEmpleadosConJerarquia();
        
        // Obtener logs de auditoría
        List<LogsAuditoria> logs = bd.ObtenerLogsAuditoria();

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

        BD bd = new BD();

        // Validar que sea un Supervisor nivel 2
        if (bd.VerificarSupervisor(legajo))
        {
            // ✅ ÉXITO: Actualizar ProgresoSala para Sala 2
            bd.MarcarSalaResuelta(partidaId, 2);

            // ✅ REDIRECCIÓN: A la Sala 3
            return RedirectToAction("Reparar", "Inventario");
        }
        else
        {
            // Fallo: volver a la vista con error
            ViewBag.Error = "Credencial rechazada. El legajo no corresponde a un Supervisor de Nivel 2 o no existe.";
            
            var viewModel = new InvestigacionViewModel
            {
                Empleados = bd.ObtenerEmpleadosConJerarquia(),
                Logs = bd.ObtenerLogsAuditoria()
            };

            return View("Investigar", viewModel);
        }
    }
}
