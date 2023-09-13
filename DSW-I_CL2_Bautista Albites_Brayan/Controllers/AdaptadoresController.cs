using Microsoft.AspNetCore.Mvc;

namespace CL1LaboPeruSAC.Controllers
{
    public class AdaptadoresController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "VentasHardware");
        }

        public IActionResult Adaptadores()
        {
            return View();
        }
    }
}
