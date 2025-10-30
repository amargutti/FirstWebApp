using FirstWebApp.Models.Services.Application;
using FirstWebApp.Models.ViewModels;
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

        public async Task<IActionResult> Index([FromServices] ICachedCourseService courseService) // ho tutto l'interessa che il risultato stia in cache
        {
            ViewData["Title"] = "Benvenuto su MyCourse";

            List<CourseViewModel> bestRatingCourses = await courseService.GetBestRatingCoursesAsync();
            List<CourseViewModel> mostRecentCourses = await courseService.GetMostRecentCoursesAsync();

            HomeViewModel viewModel = new HomeViewModel
            {
                BestRatingCourses = bestRatingCourses,
                MostRecentCourses = mostRecentCourses
            };

            return View(viewModel);
        }
    }
}
