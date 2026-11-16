
using tpfinal.Models;
using Microsoft.AspNetCore.Mvc;

namespace tpfinal.Controllers
{
    public class UsuarioController : Controller
    {
        BD bd = new BD();

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string nombreUsuario, string contrasena)
        {
            
            Usuario usuario = bd.IniciarSesion(nombreUsuario, contrasena);

            if (usuario != null)
            {
                HttpContext.Session.SetString("IdUsuario", usuario.Id.ToString());
                HttpContext.Session.SetString("NombreUsuario", usuario.NombreUsuario);

                return RedirectToAction("Index", "Espacios");
            }
            else
            {
                ViewBag.Mensaje = "El usuario o la contraseña son incorrectos.";
                return View();
            }
        }

        public IActionResult Registro()
        {
            return View();
        }

        
        [HttpPost]
        public IActionResult Registro(Usuario usuario)
        {
            
            bool existe = bd.FijarseSiExisteUsuario(usuario.NombreUsuario);

            if (existe == false)
            {
                // Registramos al usuario en la base de datos
                bd.RegistrarUsuario(usuario);

                // Lo manda al inicio de sesión
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.Mensaje = "Este nombre de usuario ya está registrado.";
                return View();
            }
        }


        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
