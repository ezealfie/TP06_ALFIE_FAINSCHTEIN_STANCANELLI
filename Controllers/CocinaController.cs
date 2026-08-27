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
            ViewBag.Mensaje = "Seleccioná al menos un pedido.";
            ViewBag.TipoMensaje = "warning";
            return View("Tablero", bd.ObtenerPedidosPendientes(partidaId));
        }

        if (bd.TodosLosPedidosSonFalsos(partidaId, ticketsSeleccionados))
        {
            bd.CancelarPedidos(partidaId, ticketsSeleccionados);
            int cantidad = ticketsSeleccionados.Length;
            ViewBag.Mensaje = $"Éxito: se eliminaron {cantidad} pedido{(cantidad != 1 ? "s" : "")} falso{(cantidad != 1 ? "s" : "")}. Sistema parcialmente recuperado.";
            ViewBag.TipoMensaje = "success";
        }
        else
        {
            ViewBag.Mensaje = "Penalización: seleccionaste al menos un pedido real.";
            ViewBag.TipoMensaje = "danger";
        }

        return View("Tablero", bd.ObtenerPedidosPendientes(partidaId));
    }
}
