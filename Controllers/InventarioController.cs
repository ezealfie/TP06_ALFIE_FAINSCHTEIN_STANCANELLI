using Microsoft.AspNetCore.Mvc;
using TP06.Models;

namespace TP06.Controllers;

public class InventarioController : Controller
{
    [HttpGet]
    public IActionResult Reparar()
    {
        string? partidaIdTexto = HttpContext.Session.GetString("PartidaId");

        if (!int.TryParse(partidaIdTexto, out int partidaId))
        {
            return RedirectToAction("Inicio", "Home");
        }

        BD bd = new BD();

        if (!bd.VerificarSalaResuelta(partidaId, 2))
        {
            return RedirectToAction("Investigar", "Directorio");
        }

        List<Inventario> inventario = bd.ObtenerInventarioPorPartida(partidaId);

        foreach (Inventario item in inventario)
        {
            if (item.Insumo == "Pan")
            {
                item.Proveedor = "Harinas del Sur";
            }
            else if (item.Insumo == "Medallon")
            {
                item.Proveedor = "Carnes Premium";
            }
            else if (item.Insumo == "Cheddar")
            {
                item.Proveedor = "Lácteos Norte";
            }
            else
            {
                item.Proveedor = "Proveedor interno";
            }
        }

        return View(inventario);
    }

    [HttpPost]
    public IActionResult RestaurarSistema(int medallones, int cheddar)
    {
        string? partidaIdTexto = HttpContext.Session.GetString("PartidaId");

        if (!int.TryParse(partidaIdTexto, out int partidaId))
        {
            return RedirectToAction("Inicio", "Home");
        }

        BD bd = new BD();

        if (!bd.VerificarSalaResuelta(partidaId, 2))
        {
            return RedirectToAction("Investigar", "Directorio");
        }

        int pan = bd.ObtenerCantidadInsumoPorPartida(partidaId, "Pan");

        if (medallones == pan && cheddar == (medallones * 2))
        {
            bd.ActualizarCantidadInsumo(partidaId, "Medallon", medallones);
            bd.ActualizarCantidadInsumo(partidaId, "Cheddar", cheddar);
            bd.MarcarSalaResuelta(partidaId, 3);
            bd.MarcarFechaFin(partidaId);
            return RedirectToAction("Victoria", "Inventario");
        }

        bd.SumarError(partidaId);
        ViewBag.Error = "Sincronización fallida: el stock de Medallones y Cheddar no respeta la regla del local. Debe coincidir con el Pan y el Cheddar debe ser el doble de los Medallones.";

        List<Inventario> inventario = bd.ObtenerInventarioPorPartida(partidaId);
        foreach (Inventario item in inventario)
        {
            if (item.Insumo == "Pan")
            {
                item.Proveedor = "Harinas del Sur";
            }
            else if (item.Insumo == "Medallon")
            {
                item.Proveedor = "Carnes Premium";
            }
            else if (item.Insumo == "Cheddar")
            {
                item.Proveedor = "Lácteos Norte";
            }
            else
            {
                item.Proveedor = "Proveedor interno";
            }
        }

        return View("Reparar", inventario);
    }

    [HttpGet]
    public IActionResult Victoria()
    {
        string? partidaIdTexto = HttpContext.Session.GetString("PartidaId");

        if (!int.TryParse(partidaIdTexto, out int partidaId))
        {
            return RedirectToAction("Inicio", "Home");
        }

        BD bd = new BD();

        if (!bd.VerificarSalaResuelta(partidaId, 3))
        {
            return RedirectToAction("Reparar", "Inventario");
        }

        var partidaActual = bd.ObtenerPartidaPorId(partidaId);
        var viewModel = new VictoriaViewModel
        {
            PartidaActual = partidaActual,
            Ranking = bd.ObtenerRankingTop10()
        };

        return View(viewModel);
    }
}
