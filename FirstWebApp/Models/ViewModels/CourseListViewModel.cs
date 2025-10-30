using FirstWebApp.Models.InputModels;

namespace FirstWebApp.Models.ViewModels
{
    public class CourseListViewModel : IPaginationInfo
    {
        public ListViewModel<CourseViewModel> Courses { get; set; }
        public CourseListInputModel Input {  get; set; }

        //implementare un interfaccia in maniera esplicita in sostanza vuol dire usare una tecnica che impedisce (in teoria) che vi siano conflitti di nomi

        #region Explicit Implementation of IPaginationInfo Interface
        int IPaginationInfo.CurrentPage { get => Input.Page; set => throw new NotImplementedException(); }
        int IPaginationInfo.TotalResult { get => Courses.TotalCount; set => throw new NotImplementedException(); }
        int IPaginationInfo.ResultsPerPage { get => Input.Limit; set => throw new NotImplementedException(); }
        string IPaginationInfo.Search { get => Input.Search; set => throw new NotImplementedException(); }
        string IPaginationInfo.OrderBy { get => Input.OrderBy; set => throw new NotImplementedException(); }
        bool IPaginationInfo.Ascending { get => Input.Ascending; set => throw new NotImplementedException(); }
        #endregion
    }
}
