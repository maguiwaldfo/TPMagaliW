
using tpfinal.Models;
using Microsoft.AspNetCore.Mvc;

namespace tpfinal.Controllers
{
    public class EspaciosController : Controller
    {
        BD bd = new BD();

        public IActionResult Index()
        {
            // Obtenemos los primeros 8 espacios
            List<Espacio> espacios = bd.ObtenerEspacios(0);

            return View(espacios);
        }

        [HttpGet]
        public IActionResult VerMas(int ultimoId)
        {
            // Obtenemos los siguientes 8 espacios
            List<Espacio> espacios = bd.ObtenerEspacios(ultimoId);

            return PartialView("TarjetasEspacios", espacios);
        }

        public IActionResult Detalle(int id)
        {
            // Buscamos los datos del espacio
            Espacio espacio = bd.ObtenerDetalleEspacio(id);

            if (espacio == null)
            {
                return RedirectToAction("Index");
            }

            return View(espacio);
        }

        [HttpGet]
        public IActionResult OcupacionDelDia(int idEspacio, DateTime fecha)
        {
            // Buscamos las reservas del espacio en esa fecha
            List<Reserva> reservas = bd.ObtenerReservasDelDia(idEspacio, fecha);

            return Json(reservas);
        }

        [HttpPost]
        public IActionResult Reservar(Reserva reserva)
        {
            // Obtenemos el ID del usuario
            int idUsuario = Convert.ToInt32(HttpContext.Session.GetString("IdUsuario"));

            // Buscamos el espacio
            Espacio espacio = bd.ObtenerDetalleEspacio(reserva.IdEspacio);

            // Verificamos que el espacio exista y esté activo
            if (espacio == null)
            {
                return RedirectToAction("Index");
            }

            // Verificamos que la fecha sea futura
            if (reserva.FechaHoraInicio <= DateTime.Now)
            {
                ViewBag.Mensaje = "No podés realizar una reserva en una fecha u horario pasado.";
                return View("Detalle", espacio);
            }

            // Verificamos que la duración sea de 1 a 4 horas
            if (reserva.CantidadHoras < 1 || reserva.CantidadHoras > 4)
            {
                ViewBag.Mensaje = "La duración debe ser de entre 1 y 4 horas.";
                return View("Detalle", espacio);
            }

            // Obtenemos las reservas del espacio para ese día
            List<Reserva> reservasDelDia = bd.ObtenerReservasDelDia(
                reserva.IdEspacio,
                reserva.FechaHoraInicio
            );

            DateTime fechaFin = reserva.FechaHoraInicio.AddHours(reserva.CantidadHoras);

            foreach (Reserva reservaExistente in reservasDelDia)
            {
                // Calculamos cuándo termina la reserva existente
                DateTime fechaFinExistente = reservaExistente.FechaHoraInicio.AddHours(
                    reservaExistente.CantidadHoras
                );

                // Comprobamos si los horarios se superponen
                if (reserva.FechaHoraInicio < fechaFinExistente &&
                    fechaFin > reservaExistente.FechaHoraInicio)
                {
                    ViewBag.Mensaje = "El espacio ya está ocupado en ese horario.";
                    return View("Detalle", espacio);
                }
            }

            // Obtenemos las reservas del usuario para ese día
            List<Reserva> reservasUsuario = bd.ObtenerReservasUsuarioDelDia(
                idUsuario,
                reserva.FechaHoraInicio
            );

            // Verificamos que no tenga ya 3 reservas ese día
            if (reservasUsuario.Count >= 3)
            {
                ViewBag.Mensaje = "No podés realizar más de 3 reservas por día.";
                return View("Detalle", espacio);
            }

            reserva.IdUsuario = idUsuario;
            bd.RegistrarReserva(reserva);
            return RedirectToAction("MisReservas", "Reservas");
        }
    }
}
