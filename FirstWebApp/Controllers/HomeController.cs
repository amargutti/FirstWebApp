using Microsoft.AspNetCore.Mvc;

namespace FirstWebApp.Controllers
{

    [ResponseCache(CacheProfileName = "Home")] //cosi ha effetto su tutte le action del controller
    
    public class HomeController : Controller
    {
        // il contentuo dell'action Index può essere tenuto in cache per 60 secondi e poi sarà eliminato
        // con Location si va a decide quali dispositivi possono avvalersi di questa cache (browser, proxy, load helper
        //[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
        [ResponseCache(CacheProfileName = "Home")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
