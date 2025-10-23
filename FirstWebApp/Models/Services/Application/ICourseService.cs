using FirstWebApp.Models.ViewModels;

namespace FirstWebApp.Models.Services.Application
{
    public interface ICourseService
    {
        List<CourseViewModel>GetCourses();
        CourseDetailViewModel GetCourse(string id);
    }
}
