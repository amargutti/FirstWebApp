using FirstWebApp.Models.InputModels;
using FirstWebApp.Models.ViewModels;

namespace FirstWebApp.Models.Services.Application
{
    public interface ICourseService
    {
        Task<List<CourseViewModel>>GetCoursesAsync(CourseListInputModel model);
        Task<CourseDetailViewModel> GetCourseAsync(string id);
    }
}
