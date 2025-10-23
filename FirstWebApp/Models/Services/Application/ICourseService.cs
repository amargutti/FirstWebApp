using FirstWebApp.Models.ViewModels;

namespace FirstWebApp.Models.Services.Application
{
    public interface ICourseService
    {
        Task<List<CourseViewModel>>GetCoursesAsync();
        Task<CourseDetailViewModel> GetCourseAsync(string id);
    }
}
