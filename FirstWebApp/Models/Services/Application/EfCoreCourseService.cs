using FirstWebApp.Models.EF_Models;
using FirstWebApp.Models.InputModels;
using FirstWebApp.Models.Options;
using FirstWebApp.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FirstWebApp.Models.Services.Application
{
    public class EfCoreCourseService : ICourseService
    {
        private readonly FirstWebAppDBContext dBContext;
        private readonly IOptionsMonitor<CoursesOptions> coursesOptions;

        public EfCoreCourseService(FirstWebAppDBContext dBContext, IOptionsMonitor<CoursesOptions> coursesOptions)
        {
            this.dBContext = dBContext;
            this.coursesOptions = coursesOptions;
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

        public async Task<ListViewModel<CourseViewModel>> GetCoursesAsync(CourseListInputModel model)
        {
            IQueryable<Course> baseQuery = dBContext.Courses;

            //prima sanitizzare i parametri come su ADO.NET

            switch (model.OrderBy)
            {
                case "Title":
                    if (model.Ascending)
                    {
                        baseQuery = baseQuery.OrderBy(course => course.Title);
                    }
                    else
                    {
                        baseQuery = baseQuery.OrderByDescending(course => course.Title);
                    }
                    break;
                case "Rating":
                    if (model.Ascending)
                    {
                        baseQuery = baseQuery.OrderBy(course => course.Rating);
                    }
                    else
                    {
                        baseQuery = baseQuery.OrderByDescending(course => course.Rating);
                    }
                    break;
                case "CurrentPrice":
                    if (model.Ascending)
                    {
                        baseQuery = baseQuery.OrderBy(course => course.CurrentPrice.Amount);
                    }
                    else
                    {
                        baseQuery = baseQuery.OrderByDescending(course => course.CurrentPrice.Amount);
                    }
                    break;
            }

            IQueryable<CourseViewModel> queyrLinq = baseQuery.AsNoTracking()
                .Where(course => course.Title.Contains(model.Search))
                .Select(course => new CourseViewModel
                { //compongo la query
                    Id = course.Id,
                    Title = course.Title,
                    Author = course.Author,
                    Rating = course.Rating,
                    CurrentPrice = course.CurrentPrice,
                    FullPrice = course.FullPrice,
                    ImagePath = course.ImagePath,
                }); //rendo solo leggibile questa query

            var sql = queyrLinq.ToQueryString(); //durante il debugging visualizzi la Query in SQL Vanilla

            List<CourseViewModel> courses = await queyrLinq.ToListAsync(); //invoco la query che viene eseguita
            int TotalCount = await queyrLinq.Skip(model.Offset).Take(model.Limit).CountAsync();

            ListViewModel<CourseViewModel> result = new ListViewModel<CourseViewModel>
            {
                Results = courses,
                TotalCount = TotalCount
            };

            return result;
        }

        public async Task<List<CourseViewModel>> GetBestRatingCoursesAsync()
        {
            CourseListInputModel inputModel = new(
                search: "",
                page: 1,
                orderby: "Rating",
                ascending: false,
                limit: coursesOptions.CurrentValue.InHome,
                orderOptions: coursesOptions.CurrentValue.Order);

            ListViewModel<CourseViewModel> result = await GetCoursesAsync(inputModel);
            return result.Results;
        }

        public async Task<List<CourseViewModel>> GetMostRecentCoursesAsync()
        {
            CourseListInputModel inputModel = new(
                search: "",
                page: 1,
                orderby: "Id",
                ascending: false,
                limit: coursesOptions.CurrentValue.InHome,
                orderOptions: coursesOptions.CurrentValue.Order);

            ListViewModel<CourseViewModel> result = await GetCoursesAsync(inputModel);
            return result.Results;
        }
    }
}
