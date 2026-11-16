
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
            // Obtenemos el ID del usuario
            int idUsuario = Convert.ToInt32(HttpContext.Session.GetString("IdUsuario"));

            // Intentamos cancelar la reserva
            bool resultado = bd.CancelarReserva(idReserva, idUsuario);

            // Si no se pudo cancelar, enviamos un mensaje
            if (resultado == false)
            {
                TempData["Mensaje"] = "No se pudo cancelar la reserva. Debe pertenecer a tu usuario y faltar al menos 24 horas para que comience.";
            }

            return RedirectToAction("MisReservas");
        }
    }
}
