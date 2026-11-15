using System.Diagnostics;
using tpfinal.Models;
using Microsoft.AspNetCore.Mvc;

namespace tpfinal.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}