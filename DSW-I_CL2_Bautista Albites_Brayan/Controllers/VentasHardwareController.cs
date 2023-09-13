using Microsoft.AspNetCore.Mvc;

namespace CL1LaboPeruSAC.Controllers
{
    public class VentasHardwareController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MemoriaRAM()
        {
            return View();
        }
    }
}