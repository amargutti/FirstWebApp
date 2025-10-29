using FirstWebApp.Models.Exceptions;
using FirstWebApp.Models.InputModels;
using FirstWebApp.Models.Options;
using FirstWebApp.Models.Services.Infrastructure;
using FirstWebApp.Models.ValueTypes;
using FirstWebApp.Models.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Data;

namespace FirstWebApp.Models.Services.Application
{
    public class AdoNetCourseService : ICourseService
    {
        private readonly ILogger<AdoNetCourseService> logger;
        private readonly IDatabaseAccessor db;
        private readonly IOptionsMonitor<CoursesOptions> courseOptions;

        public AdoNetCourseService(ILogger<AdoNetCourseService> logger, IDatabaseAccessor db, IOptionsMonitor<CoursesOptions> courseOptions)
        {
            this.logger = logger;
            this.db = db;
            this.courseOptions = courseOptions;
        }

        public async Task<List<CourseViewModel>> GetCoursesAsync(CourseListInputModel model)
        {
            //Decidere cosa estrarre dal db

            if (model.OrderBy == "CurrentPrice")
            {
                model.OrderBy = "CurrentPrice_Amount";
            }

            //WHERE Title LIKE '{"%" + search + "%"}' 
            string direction = model.Ascending ? "ASC" : "DESC";
            FormattableString query = @$"SELECT Id, Title, ImagePath, Author, Rating, FullPrice_Amount, 
            FullPrice_Currency, CurrentPrice_Amount, CurrentPrice_Currency 
            FROM Courses 
            WHERE Title LIKE '{"%" + model.Search + "%"}' 
            ORDER BY {(Sql )model.OrderBy} { (Sql) direction} 
            OFFSET {model.Offset} ROWS
            FETCH NEXT {model.Limit} 
            ROWS ONLY"; //i % per vedere se la singola parola è compresa nel titolo
            DataSet dataSet = await db.QueryAsync(query);
            var dataTable = dataSet.Tables[0];
            var courseList = new List<CourseViewModel>();
            foreach (DataRow courseRow in dataTable.Rows)
            {
                CourseViewModel course = CourseViewModel.FromDataRow(courseRow);
                courseList.Add(course);
            }
            return courseList;
        }

        public async Task<CourseDetailViewModel> GetCourseAsync(string id)
        {
            logger.LogInformation("Course {id} requested", id); //logging strutturato != interpolazione di stringhe

            //@ definisce una stringa su + righe
            FormattableString query = $@"SELECT Id, Title, Description, ImagePath, Author, Rating, FullPrice_Amount, FullPrice_Currency, CurrentPrice_Amount, CurrentPrice_Currency FROM Courses WHERE Id={id};
            SELECT Id, Title, Description, Duration FROM Lessons WHERE CourseId={id}";

            DataSet dataSet = await db.QueryAsync(query);

            //Course
            var courseTable = dataSet.Tables[0];
            if (courseTable.Rows.Count != 1)
            {
                logger.LogWarning("Course {id} not found!", id);
                throw new CourseNotFoundException(id);
            }

            var courseRow = courseTable.Rows[0];
            var courseDetailViewModel = CourseDetailViewModel.FromDataRow(courseRow);

            //Course Lesson

            var lessonDataTable = dataSet.Tables[1];
            foreach (DataRow lessonRow in lessonDataTable.Rows)
            {
                LessonViewModel lessonViewModel = LessonViewModel.FromDataRow(lessonRow);
                courseDetailViewModel.Lessons.Add(lessonViewModel);
            }

            return courseDetailViewModel;
        }
    }
}
