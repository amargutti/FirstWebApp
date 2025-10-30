using FirstWebApp.Models.InputModels;
using FirstWebApp.Models.Options;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace FirstWebApp.Customizations.ModelBinder
{

    /* Il value binder è colui che si occupa di recuperare i valori contenuti in delle chiavi trasmessi dall'utente
     tramite Query String,
        Il ModelBinding è colui che si occupa di convertire i dati, eseguire operazioni preliminari e poi restituirli
    con i tipi corretti.
     */
    public class CourseListInputModelBinder : IModelBinder
    {
        private readonly IOptionsMonitor<CoursesOptions> coursesOptions;
        public CourseListInputModelBinder(IOptionsMonitor<CoursesOptions> coursesOptions)
        {
            this.coursesOptions = coursesOptions;
        }
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            //Recuperiamo i valori grazie ai value provider
            string search = bindingContext.ValueProvider.GetValue("Search").FirstValue;
            string orderBy = bindingContext.ValueProvider.GetValue("OrderBy").FirstValue;
            int.TryParse(bindingContext.ValueProvider.GetValue("Page").FirstValue, out int page);
            bool.TryParse(bindingContext.ValueProvider.GetValue("Ascending").FirstValue, out bool ascending);

            //Creiamo l'istanza del CourseListInputModel
            CoursesOptions options = coursesOptions.CurrentValue;
            CourseListInputModel inputModel = new(search, page, orderBy, ascending, (int) options.PerPage, options.Order);

            //Impostiamo il risultato per notificare che la creazione è avvenuta con successo
            bindingContext.Result = ModelBindingResult.Success(inputModel);

            //Restituiamo un task completato
            return Task.CompletedTask;
        }
    }
}
