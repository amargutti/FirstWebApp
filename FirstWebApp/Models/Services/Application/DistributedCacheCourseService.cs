using FirstWebApp.Models.ViewModels;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text.Json.Serialization;

//! NON STO USANDO IN NESSUN PUNTO
//SOLO PER RIFERIMENTO A DISTRIBUTED CACHE E DISTRIBUZIONE SU SCALA ORIZZONTALE DELL'APPLICAZIONE

namespace FirstWebApp.Models.Services.Application
{
    public class DistributedCacheCourseService : ICachedCourseService
    {
        private readonly ICourseService courseService;
        private readonly IDistributedCache distributedCache;

        public DistributedCacheCourseService (ICourseService courseService, IDistributedCache distributedCache)
        {
            this.courseService = courseService;
            this.distributedCache = distributedCache;
        }

        public Task<CourseDetailViewModel> GetCourseAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CourseViewModel>> GetCoursesAsync(string search)
        {
            string key = $"Courses";
            string serializedObject = await distributedCache.GetStringAsync(key);

            if (serializedObject != null)
            {
                return Deserialize<List<CourseViewModel>>(serializedObject);
            }

            List<CourseViewModel> courses = await courseService.GetCoursesAsync();
            serializedObject = Serialize(courses);

            var cacheOptions = new DistributedCacheEntryOptions();
            cacheOptions.SetAbsoluteExpiration(TimeSpan.FromSeconds(60));

            await distributedCache.SetStringAsync(key, serializedObject, cacheOptions);
            return courses;
        }

        private string Serialize (object obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        private T Deserialize<T>(string serializedObject) {

            return JsonSerializer.Deserialize<T>(serializedObject);
        }
    }
}
