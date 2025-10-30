using FirstWebApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FirstWebApp.Customizations.ViewComponents
{
    public class PaginationBarViewComponent : ViewComponent
    {
        //public IViewComponentResult Invoke(CourseListViewModel model) { }

        //debolmente accoppiato grazie all'utilizzo dell'interfaccia
        public IViewComponentResult Invoke(IPaginationInfo model) //si DEVE chiamare Invoke o InvokeAsync
        {
            //Il numero di pagina corrente
            //Il numero di risultati totali
            //IL numero di risultati per pagina
            //Search, OrderBy, Ascending
            return View(model);
        }
    }
}
