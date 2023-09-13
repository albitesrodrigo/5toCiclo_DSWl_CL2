using Microsoft.AspNetCore.Mvc;

namespace CL1LaboPeruSAC.Controllers
{
    public class PerifericosController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "VentasHardware");
        }

        public IActionResult Perifericos()
        {
            return View();
        }
    }
}
