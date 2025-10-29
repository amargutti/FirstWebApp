using FirstWebApp.Models.InputModels;

namespace FirstWebApp.Models.ViewModels
{
    public class CourseListViewModel
    {
        public List<CourseViewModel> Courses { get; set; }
        public CourseListInputModel Input {  get; set; }
    }
}
