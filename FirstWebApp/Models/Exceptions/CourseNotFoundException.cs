namespace FirstWebApp.Models.Exceptions
{
    public class CourseNotFoundException : Exception
    {
        public CourseNotFoundException(string courseId) : base("Course {courseId} not found")
        {
        }
    }
}
