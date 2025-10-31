using FirstWebApp.Models.InputModels;
using FirstWebApp.Models.ViewModels;

namespace FirstWebApp.Models.Services.Application
{
    public interface ICourseService
    {
        Task<ListViewModel<CourseViewModel>>GetCoursesAsync(CourseListInputModel model);
        Task<CourseDetailViewModel> GetCourseAsync(string id);

        Task<List<CourseViewModel>> GetBestRatingCoursesAsync();
        Task<List<CourseViewModel>> GetMostRecentCoursesAsync();
        Task<CourseDetailViewModel> CreateCourseAsync(CourseCreateInputModel model);
    }
}
