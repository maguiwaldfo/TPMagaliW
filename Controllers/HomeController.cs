
using Microsoft.AspNetCore.Mvc;

namespace tpfinal.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("IdUsuario") == null)
            {
                return RedirectToAction("Login", "Usuario");
            }
            return RedirectToAction("Index", "Espacios");
        }
    }
}
