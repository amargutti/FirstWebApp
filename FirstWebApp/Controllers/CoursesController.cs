using FirstWebApp.Models.InputModels;
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

        public async Task<IActionResult> Index(CourseListInputModel model)
        {
            ViewData["Title"] = "Elenco Corsi";
            ListViewModel<CourseViewModel> courses = await courseService.GetCoursesAsync(model);
            
            CourseListViewModel viewModel = new CourseListViewModel
            {
                Courses = courses,
                Input = model
            };
            
            return View(viewModel); // volendo si potrebbe specificare il percorso della view come stringa anche con nome diverso dalla action
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

        public IActionResult Create ()
        {
            ViewData["Title"] = "Nuovo Corso";
            var inputModel = new CourseCreateInputModel();
            return View(inputModel);
        }

        [HttpPost] //Toglie l'ambiguità tra le due chiamate in quanto il browser saprà che questa deve essere lanciata solo quando la richiesta è di tipo POST
        public IActionResult Create(CourseCreateInputModel model) {
            //Coinvolgere un servizio applicativo che si occupi della creazione del corso
            return RedirectToAction(nameof(Index));
        }
    }
}
