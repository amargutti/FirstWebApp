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

        public async Task<ListViewModel<CourseViewModel>> GetCoursesAsync(CourseListInputModel model)
        {
            //Decidere cosa estrarre dal db

            string orderby = model.OrderBy == "CurrentPrice" ? "CurrentPrice_Amount" : model.OrderBy;

            //WHERE Title LIKE '{"%" + search + "%"}' 
            string direction = model.Ascending ? "ASC" : "DESC";
            FormattableString query = @$"SELECT Id, Title, ImagePath, Author, Rating, FullPrice_Amount, 
            FullPrice_Currency, CurrentPrice_Amount, CurrentPrice_Currency 
            FROM Courses 
            WHERE Title LIKE '{"%" + model.Search + "%"}' 
            ORDER BY {(Sql) model.OrderBy} { (Sql) direction} 
            OFFSET {model.Offset} ROWS
            FETCH NEXT {model.Limit} 
            ROWS ONLY;
            SELECT COUNT (*) FROM Courses
            WHERE Title LIKE '{"%" + model.Search + "%"}'"; //i % per vedere se la singola parola è compresa nel titolo
            DataSet dataSet = await db.QueryAsync(query);
            var dataTable = dataSet.Tables[0];
            var courseList = new List<CourseViewModel>();
            foreach (DataRow courseRow in dataTable.Rows)
            {
                CourseViewModel course = CourseViewModel.FromDataRow(courseRow);
                courseList.Add(course);
            }

            ListViewModel<CourseViewModel> result = new ListViewModel<CourseViewModel>
            {
                Results = courseList,
                TotalCount = Convert.ToInt32(dataSet.Tables[1].Rows[0][0])
            };

            return result;
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

        public async Task<List<CourseViewModel>> GetMostRecentCoursesAsync()
        {
            CourseListInputModel inputmodel = new CourseListInputModel(
                search: "",
                page: 1,
                orderby: "id",
                ascending: false,
                limit: courseOptions.CurrentValue.InHome,
                orderOptions: courseOptions.CurrentValue.Order);

            ListViewModel<CourseViewModel> result = await GetCoursesAsync(inputmodel);
            return result.Results;
        }

        public async Task<List<CourseViewModel>> GetBestRatingCoursesAsync()
        {
            CourseListInputModel inputModel = new CourseListInputModel(
                search: "",
                page: 1,
                orderby: "Rating",
                ascending: true,
                limit: courseOptions.CurrentValue.InHome,
                orderOptions: courseOptions.CurrentValue.Order);

            ListViewModel<CourseViewModel> result = await GetCoursesAsync(inputModel);

            return result.Results;
        }
    }
}
