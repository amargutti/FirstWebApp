using FirstWebApp.Models.EF_Models;
using FirstWebApp.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<CourseViewModel>> GetCoursesAsync()
        {
            var courses =  dBContext.Courses.Select(course => new CourseViewModel
            {
                Id = course.Id,
                Title = course.Title,
                Author  = course.Author,
                Rating = course.Rating,
                CurrentPrice = course.CurrentPrice,
                FullPrice = course.FullPrice,
                ImagePath = course.ImagePath,
            }).AsNoTracking();


            var sql = courses.ToQueryString(); //durante il debugging visualizzi la Query in SQL Vanilla
            return await courses.ToListAsync();
        }
    }
}
