using System.Diagnostics;
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
        return RedirectToAction("Inicio", "Home");

    }
  public IActionResult Inicio()
    {
        return View();
    }
    [HttpPost]
    public IActionResult ComenzarJuego(string nombreJugador, DateTime fechaHora)
    {
        BD bd = new BD();
        bd.IniciarPartida(new Partidas(nombreJugador, fechaHora));
        return View();
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
