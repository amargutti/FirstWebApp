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

        public Task<CourseDetailViewModel> GetCourseAsync(string id)
        {
            return memoryCache.GetOrCreateAsync($"Course {id}", cacheEntry =>
            {
                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(60));
                return courseService.GetCourseAsync(id);
            });
        }

        public Task<List<CourseViewModel>> GetCoursesAsync()
        {
            return memoryCache.GetOrCreate($"Courses", cacheEntry =>
            {
                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(60));
                return courseService.GetCoursesAsync();
            });
        }
    }
}
