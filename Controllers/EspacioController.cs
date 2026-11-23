
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
public IActionResult Reservar(int IdEspacio, string Fecha, string Hora, int CantidadHoras)
{
    string idUsuarioSesion = HttpContext.Session.GetString("IdUsuario");
    if (string.IsNullOrEmpty(idUsuarioSesion))
    {
        return RedirectToAction("Login", "Usuario");
    }

    int idUsuario = Convert.ToInt32(idUsuarioSesion);

    // Unimos la fecha y la hora
    DateTime fechaHoraInicio = DateTime.Parse(Fecha + "T" + Hora);

    // Creamos la reserva
    Reserva reserva = new Reserva();

    reserva.IdEspacio = IdEspacio;
    reserva.IdUsuario = idUsuario;
    reserva.FechaHoraInicio = fechaHoraInicio;
    reserva.CantidadHoras = CantidadHoras;

    Espacio espacio = bd.ObtenerDetalleEspacio(reserva.IdEspacio);

    if (espacio == null)
    {
        return RedirectToAction("Index");
    }

    if (reserva.FechaHoraInicio <= DateTime.Now)
    {
        ViewBag.Mensaje = "No podés realizar una reserva en una fecha u horario pasado.";
        return View("Detalle", espacio);
    }

    if (reserva.CantidadHoras < 1 || reserva.CantidadHoras > 4)
    {
        ViewBag.Mensaje = "La duración debe ser de entre 1 y 4 horas.";
        return View("Detalle", espacio);
    }

    List<Reserva> reservasDelDia = bd.ObtenerReservasDelDia(
        reserva.IdEspacio,
        reserva.FechaHoraInicio
    );

    DateTime fechaFin = reserva.FechaHoraInicio.AddHours(reserva.CantidadHoras);

    foreach (Reserva reservaExistente in reservasDelDia)
    {
        DateTime fechaFinExistente = reservaExistente.FechaHoraInicio.AddHours(
            reservaExistente.CantidadHoras
        );

        if (reserva.FechaHoraInicio < fechaFinExistente &&
            fechaFin > reservaExistente.FechaHoraInicio)
        {
            ViewBag.Mensaje = "El espacio ya está ocupado en ese horario.";
            return View("Detalle", espacio);
        }
    }

    List<Reserva> reservasUsuario = bd.ObtenerReservasUsuarioDelDia(
        idUsuario,
        reserva.FechaHoraInicio
    );

    if (reservasUsuario.Count >= 3)
    {
        ViewBag.Mensaje = "No podés realizar más de 3 reservas por día.";
        return View("Detalle", espacio);
    }

    bd.RegistrarReserva(reserva);

    return RedirectToAction("MisReservas", "Reservas");
}

}
}
