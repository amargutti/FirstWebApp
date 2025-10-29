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
        private IOptionsMonitor<CoursesOptions> courseOptions;

        public CourseListInputModelBinder (IOptionsMonitor<CoursesOptions> courseOptions)
        {
            this.courseOptions = courseOptions;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            //recuperiamo i valori grazie ai value binder
            string search = bindingContext.ValueProvider.GetValue("Search").FirstValue;
            int page = Convert.ToInt32(bindingContext.ValueProvider.GetValue("Page").FirstValue);
            string orderBy = bindingContext.ValueProvider.GetValue("OrderBy").FirstValue;
            bool ascending = Convert.ToBoolean(bindingContext.ValueProvider.GetValue("Ascending").FirstValue);

            //Creiamo l'istanza del CourseListInputModel
            var inputModel = new CourseListInputModel(search, page, orderBy, ascending, courseOptions.CurrentValue);

            //Impostiamo il risultato per notificare che la creazione è avvenuta con successo
            bindingContext.Result = ModelBindingResult.Success(inputModel);

            //Restituiamo un task completato
            return Task.CompletedTask;
        }
    }
}
