using FirstWebApp.Models.EF_Models;
using FirstWebApp.Models.ViewModels;

namespace FirstWebApp.Models.Services.Application
{
    public class EfCoreCourseService : ICourseService
    {
        private readonly FirstWebAppDBContext dBContext;
        public EfCoreCourseService(FirstWebAppDBContext dBContext) { 
            this.dBContext = dBContext;
        }

        public Task<CourseDetailViewModel> GetCourseAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<CourseViewModel>> GetCoursesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
