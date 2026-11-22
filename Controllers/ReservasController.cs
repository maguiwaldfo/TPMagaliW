
using tpfinal.Models;
using Microsoft.AspNetCore.Mvc;

namespace tpfinal.Controllers
{
    public class ReservasController : Controller
    {
        BD bd = new BD();

        public IActionResult Index()
        {
            return RedirectToAction("MisReservas");
        }

        public IActionResult MisReservas()
        {
            // Obtenemos el ID del usuario
            int idUsuario = Convert.ToInt32(HttpContext.Session.GetString("IdUsuario"));

            // Buscamos sus próximas reservas
            List<Reserva> reservasProximas = bd.ObtenerReservasProximas(idUsuario);

            // Buscamos sus reservas pasadas
            List<Reserva> reservasPasadas = bd.ObtenerReservasPasadas(idUsuario);
           
            ViewBag.ReservasProximas = reservasProximas;
            ViewBag.ReservasPasadas = reservasPasadas;

            return View();
        }

        
   [HttpPost]
   public IActionResult Cancelar(int idReserva)
    {
    int idUsuario = Convert.ToInt32(HttpContext.Session.GetString("IdUsuario"));
    bool resultado = bd.CancelarReserva(idReserva, idUsuario);
    return Json(resultado);
    }

    }
}
