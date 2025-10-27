namespace FirstWebApp.Models.Options
{
    public partial class CoursesOptions
    {
        public long PerPage { get; set; }
        public CourseOrderOptions Order { get; set; }
    }

    public partial class CourseOrderOptions
    {
        public string By { get; set; }
        public bool Ascending { get; set; }
        public string[] Allow {  get; set; }
    }
}
