using FirstWebApp.Models.Options;
using FirstWebApp.Models.Services.Infrastructure;
using FirstWebApp.Models.ViewModels;
using Microsoft.Extensions.Options;
using System.Data;

namespace FirstWebApp.Models.Services.Application
{
    public class AdoNetCourseService : ICourseService
    {
        private readonly IDatabaseAccessor db;
        private readonly IOptionsMonitor<CoursesOptions> courseOptions;
        public AdoNetCourseService(IDatabaseAccessor db, IOptionsMonitor<CoursesOptions> courseOptions)
        {
            this.db = db;
            this.courseOptions = courseOptions;
        }

        public async Task<List<CourseViewModel>> GetCoursesAsync()
        {
            string query = "SELECT Id, Title, ImagePath, Author, Rating, FullPrice_Amount, FullPrice_Currency, CurrentPrice_Amount, CurrentPrice_Currency  FROM Courses";
            DataSet dataSet = await db.QueryAsync(query);
            var dataTable = dataSet.Tables[0];
            var courseList = new List<CourseViewModel>();
            foreach(DataRow courseRow in dataTable.Rows)
            {
               CourseViewModel course =CourseViewModel.FromDataRow(courseRow);
                courseList.Add(course);
            }
            return courseList;
        }

        public async Task<CourseDetailViewModel> GetCourseAsync(string id)
        {
            //@ definisce una stringa su + righe
            string query = $@"SELECT Id, Title, Description, ImagePath, Author, Rating, FullPrice_Amount, FullPrice_Currency, CurrentPrice_Amount, CurrentPrice_Currency FROM Courses WHERE Id={id};
            SELECT Id, Title, Description, Duration FROM Lessons WHERE CourseId={id}";

            DataSet dataSet = await db.QueryAsync(query);

            //Course
            var courseTable = dataSet.Tables[0];
            if(courseTable.Rows.Count != 1)
            {
                throw new InvalidOperationException($"Did not return exactly 1 row of Course {id}");
            }

            var courseRow = courseTable.Rows[0];
            var courseDetailViewModel = CourseDetailViewModel.FromDataRow(courseRow);

            //Course Lesson

            var lessonDataTable = dataSet.Tables[1];
            foreach(DataRow lessonRow in lessonDataTable.Rows)
            {
                LessonViewModel lessonViewModel = LessonViewModel.FromDataRow(lessonRow);
                courseDetailViewModel.Lessons.Add(lessonViewModel);
            }

            return courseDetailViewModel; 
        }
    }
}
