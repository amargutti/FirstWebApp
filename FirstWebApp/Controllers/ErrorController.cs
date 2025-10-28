using FirstWebApp.Models.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FirstWebApp.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            switch(feature.Error)
            {
                case CourseNotFoundException exc:
                    Response.StatusCode = 404;
                    ViewData["Title"] = "Corso Non Trovato";
                    return View("CourseNotFound");
                
                default:
                    ViewData["Title"] = "Corso Non Trovato";
                    return View("");
            }
        }
    }
}
