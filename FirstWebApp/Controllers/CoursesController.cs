using FirstWebApp.Models.Services.Application;
using FirstWebApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

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
        public CoursesController(ICourseService courseService)
        {
            this.courseService = courseService;
        }

        public IActionResult Index()
        {
            List<CourseViewModel> courses = courseService.GetCourses();
            ViewData["Title"] = "Elenco Corsi";
            return View(courses); // volendo si potrebbe specificare il percorso della view come stringa anche con nome diverso dalla action
        }

        public IActionResult Details(string id)
        {
            CourseDetailViewModel course = courseService.GetCourse(id);
            ViewData["Title"] = course.Title; // esempio di ViewData
            return View(course);
        }

        public IActionResult Search(string title)
        {
            return Content($"Hai cercato {title}");
        }
    }
}
