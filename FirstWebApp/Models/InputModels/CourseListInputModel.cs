using FirstWebApp.Customizations.ModelBinder;
using FirstWebApp.Models.Options;
using Microsoft.AspNetCore.Mvc;

namespace FirstWebApp.Models.InputModels
{
    [ModelBinder(BinderType = typeof(CourseListInputModelBinder))]
    public class CourseListInputModel
    {
        public string Search { get; }
        public int Page {  get; }
        public string OrderBy { get; set; }
        public bool Ascending { get; }
        public int Limit { get; }
        public int Offset { get; }

        public CourseListInputModel (string search, int page, string orderBy, bool ascending, CoursesOptions coursesOptions)
        {
            //inserire qui sanitizzazione
            var orderOptions = coursesOptions.Order;
            if(!orderOptions.Allow.Contains(orderBy))
            {
                orderBy = orderOptions.By;
                ascending = orderOptions.Ascending;
            }


            Search = search ?? "";
            Page = Math.Max(1, page);
            OrderBy = orderBy;
            Ascending = ascending;

            Limit = (int) coursesOptions.PerPage;
            Offset = (Page - 1) * Limit;
        }
    }
}
