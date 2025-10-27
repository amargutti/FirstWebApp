using System.Data;

namespace FirstWebApp.Models.ViewModels
{
    public class LessonViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string DurationString { get; set; }
        public TimeSpan Duration { get {
                return DurationString != null ? TimeSpan.Parse(DurationString) : TimeSpan.Zero;
            } }
        public string Description { get; set; }

        public static LessonViewModel FromDataRow(DataRow lessonRow)
        {
            var durationString = Convert.ToString(lessonRow["Duration"]);
            var lessonViewModel = new LessonViewModel
            {
                Title = Convert.ToString(lessonRow["Title"]),
            };
            return lessonViewModel;
        }
    }
}