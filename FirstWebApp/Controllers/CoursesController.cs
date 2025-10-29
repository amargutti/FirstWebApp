using FirstWebApp.Models.Services.Application;
using FirstWebApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace FirstWebApp.Controllers
{
    public class CoursesController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
#if DEBUG
            Console.WriteLine(context.HttpContext.Request.Path);
#endif
            base.OnActionExecuting(context);
        }

        private readonly ICourseService courseService;
        public CoursesController(ICachedCourseService courseService)
        {
            this.courseService = courseService;
        }

        public async Task<IActionResult> Index(string search, int page, string orderby, bool ascending)
        {
            ViewData["Title"] = "Elenco Corsi";
            List<CourseViewModel> courses = await courseService.GetCoursesAsync(search, page, orderby, ascending);
            return View(courses); // volendo si potrebbe specificare il percorso della view come stringa anche con nome diverso dalla action
        }

        public async Task<IActionResult> Details(string id)
        {
            CourseDetailViewModel course = await courseService.GetCourseAsync(id);
            ViewData["Title"] = course.Title; // esempio di ViewData
            return View(course);
        }

        public IActionResult Search(string title)
        {
            return Content($"Hai cercato {title}");
        }
    }
}
