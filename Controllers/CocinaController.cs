using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TP06.Models;

namespace TP06.Controllers;

public class CocinaController : Controller
{
    [HttpGet]
    public IActionResult Tablero()
    {
        string? partidaIdTexto = HttpContext.Session.GetString("PartidaId");

        if (!int.TryParse(partidaIdTexto, out int partidaId))
        {
            return RedirectToAction("Inicio", "Home");
        }

        BD bd = new BD();
        List<Pedidos> pedidos = bd.ObtenerPedidosPendientes(partidaId);

        return View(pedidos);
    }

    [HttpPost]
    public IActionResult PurgarSistema(int[] ticketsSeleccionados)
    {
        string? partidaIdTexto = HttpContext.Session.GetString("PartidaId");

        if (!int.TryParse(partidaIdTexto, out int partidaId))
        {
            return RedirectToAction("Inicio", "Home");
        }

        BD bd = new BD();

        if (ticketsSeleccionados == null || ticketsSeleccionados.Length == 0)
        {
            bd.SumarError(partidaId);
            ViewBag.Mensaje = "Seleccioná al menos un pedido para purgar.";
            ViewBag.TipoMensaje = "warning";
            return View("Tablero", bd.ObtenerPedidosPendientes(partidaId));
        }

        // Validar que TODOS los seleccionados sean falsos
        if (!bd.TodosLosPedidosSonFalsos(partidaId, ticketsSeleccionados))
        {
            bd.SumarError(partidaId);
            ViewBag.Mensaje = "❌ Penalización: seleccionaste un pedido real. El sistema está más comprometido.";
            ViewBag.TipoMensaje = "danger";
            return View("Tablero", bd.ObtenerPedidosPendientes(partidaId));
        }

        // ✅ Los seleccionados son todos falsos: cancelarlos
        bd.CancelarPedidos(partidaId, ticketsSeleccionados);
        
        int cantidad = ticketsSeleccionados.Length;
        
        // Verificar si aún quedan pedidos falsos pendientes
        int falsosPendientes = bd.ContarPedidosFalsos(partidaId);
        
        if (falsosPendientes == 0)
        {
            // ✅ NO HAY MÁS FALSOS: Sistema completamente purificado
            bd.MarcarSalaResuelta(partidaId, 1);
            ViewBag.Mensaje = "🔥 ¡SISTEMA COMPLETAMENTE PURIFICADO! Los pedidos saboteados han sido eliminados. ¡Avanzando a la siguiente sala!";
            ViewBag.TipoMensaje = "success";
            
            // Redirigir automáticamente después de 2 segundos (con JS)
            return RedirectToAction("Investigar", "Directorio");
        }
        else
        {
            // Aún hay falsos: mostrar progreso sin números
            ViewBag.Mensaje = $"✓ Operación exitosa: {cantidad} pedido{(cantidad != 1 ? "s" : "")} purgado{(cantidad != 1 ? "s" : "")}. Continúa eliminando los sabotajes...";
            ViewBag.TipoMensaje = "success";
        }

        return View("Tablero", bd.ObtenerPedidosPendientes(partidaId));
    }
}
