using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibreriaPW.Areas.Administrador.Controllers
{
    [Authorize]
    [Area("Administrador")]
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
    }
}
