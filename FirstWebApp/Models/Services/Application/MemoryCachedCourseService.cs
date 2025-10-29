using FirstWebApp.Models.InputModels;
using FirstWebApp.Models.ViewModels;
using Microsoft.Extensions.Caching.Memory;

namespace FirstWebApp.Models.Services.Application
{
    public class MemoryCachedCourseService : ICachedCourseService
    {
        private readonly IMemoryCache memoryCache;
        private readonly ICourseService courseService;
        
        public MemoryCachedCourseService(ICourseService courseService, IMemoryCache memoryCache)
        {
            this.courseService = courseService;
            this.memoryCache = memoryCache;
        }

        //TODO: ricordati di usare memoryCache.Remove($"Course {id}") quando aggiorni i campi del corso

        public Task<CourseDetailViewModel> GetCourseAsync(string id)
        {
            return memoryCache.GetOrCreateAsync($"Course {id}", cacheEntry =>
            {
                cacheEntry.SetSize(1);
                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(60));
                return courseService.GetCourseAsync(id);
            });
        }

        public Task<List<CourseViewModel>> GetCoursesAsync(CourseListInputModel model)
        {
            return memoryCache.GetOrCreate($"Courses{model.Search}-{model.Page}-{model.OrderBy}-{model.Ascending}", cacheEntry =>
            {
                cacheEntry.SetSize(1);
                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(60));
                return courseService.GetCoursesAsync(model);
            });
        }
    }
}
