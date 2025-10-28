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

        public async Task<CourseDetailViewModel> GetCourseAsync(string id)
        {

            int numId = Convert.ToInt32(id);
            var course = dBContext.Courses.Where(course => course.Id == numId)
                               .Select(course => new CourseDetailViewModel
                               {
                                   Id = course.Id,
                                   Title = course.Title,
                                   Author = course.Author,
                                   Rating = course.Rating,
                                   CurrentPrice = course.CurrentPrice,
                                   FullPrice = course.FullPrice,
                                   ImagePath = course.ImagePath,
                                   Description = course.Description,
                                   Lessons = course.Lessons.Select(lesson => new LessonViewModel
                                   {
                                       Id = lesson.Id,
                                       Title = lesson.Title,
                                       DurationString = lesson.Duration,
                                       Description = lesson.Description,
                                   }).ToList()
                               }).AsNoTracking();

            //return await course.FirstOrDefaultAsync(); //restituisce null se l'elenco è vuoto e non solleva mai un'eccezione 
            //return await course.SingleOrDefaultAsync(); // Tollera il fatto che l'elenco sia vuoto e restituisce NULL in quel caso o per meglio dire il valore di default o se l'elenco contiene + di un elemento solleva un eccezione
            //return await course.FirstAsync(); //Restituisce il primo elemento, ma se l'elenco è vuoto lancia un eccezione
            return await course.SingleAsync();//Restituisce il primo elemento dell'elenco, ma se l'elenco ne contiene 0 o + di 1 allora solleva un'eccezione
        }

        public async Task<List<CourseViewModel>> GetCoursesAsync()
        {
            IQueryable<CourseViewModel> courses =  dBContext.Courses.AsNoTracking().Select(course => new CourseViewModel
            { //compongo la query
                Id = course.Id,
                Title = course.Title,
                Author  = course.Author,
                Rating = course.Rating,
                CurrentPrice = course.CurrentPrice,
                FullPrice = course.FullPrice,
                ImagePath = course.ImagePath,
            }); //rendo solo leggibile questa query


            var sql = courses.ToQueryString(); //durante il debugging visualizzi la Query in SQL Vanilla
            return await courses.ToListAsync(); //invoco la query che viene eseguita
        }
    }
}
