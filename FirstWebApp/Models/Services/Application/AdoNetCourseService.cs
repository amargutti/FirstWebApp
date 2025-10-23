using FirstWebApp.Models.Services.Infrastructure;
using FirstWebApp.Models.ViewModels;
using System.Data;

namespace FirstWebApp.Models.Services.Application
{
    public class AdoNetCourseService : ICourseService
    {
        private readonly IDatabaseAccessor db;
        public AdoNetCourseService(IDatabaseAccessor db)
        {
            this.db = db;
        }

        public List<CourseViewModel> GetCourses()
        {
            string query = "SELECT Id, Title, ImagePath, Author, Rating, FullPrice_Amount, FullPrice_Currency, CurrentPrice_Amount, CurrentPrice_Currency  FROM Courses";
            DataSet dataSet = db.Query(query);
            var dataTable = dataSet.Tables[0];
            var courseList = new List<CourseViewModel>();
            foreach(DataRow courseRow in dataTable.Rows)
            {
               CourseViewModel course =CourseViewModel.FromDataRow(courseRow);
                courseList.Add(course);
            }
            return courseList;
        }

        public CourseDetailViewModel GetCourse(string id)
        {
            //@ definisce una stringa su + righe
            string query = $@"SELECT Id, Title, Description, ImagePath, Author, Rating, FullPrice_Amount, FullPrice_Currency, CurrentPrice_Amount, CurrentPrice_Currency FROM Courses WHERE Id={id};
            SELECT Id, Title, Description, Duration FROM Lessons WHERE CourseId={id}";

            DataSet dataSet = db.Query(query);

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
