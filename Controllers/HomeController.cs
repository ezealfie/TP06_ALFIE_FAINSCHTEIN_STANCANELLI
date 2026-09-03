using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TP06.Models;

namespace TP06.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return RedirectToAction("Instrucciones", "Home");
    }

    public IActionResult Instrucciones()
    {
        return View();
    }

    public IActionResult Inicio()
    {
        return RedirectToAction("Instrucciones", "Home");
    }

    [HttpPost]
    public IActionResult ComenzarJuego(string nombreJugador, DateTime fechaHora)
    {
        if (string.IsNullOrWhiteSpace(nombreJugador))
        {
            return RedirectToAction("Instrucciones", "Home");
        }

        BD bd = new BD();
        int partidaId = bd.IniciarPartida(new Partidas(nombreJugador, fechaHora));
        bd.InsertarPedidosParaPartida(partidaId);
        bd.InsertarInventarioParaPartida(partidaId);

        HttpContext.Session.SetString("PartidaId", partidaId.ToString());
        HttpContext.Session.SetString("NombreJugador", nombreJugador);

        return RedirectToAction("Tablero", "Cocina");
    }

    public IActionResult Derrota()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View("Instrucciones");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
