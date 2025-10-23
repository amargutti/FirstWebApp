using System.Data;

namespace FirstWebApp.Models.ViewModels
{
    public class LessonViewModel
    {
        public string Title { get; set; }
        public TimeSpan Duration { get; set; }

        public static LessonViewModel FromDataRow(DataRow lessonRow)
        {
            var durationString = Convert.ToString(lessonRow["Duration"]);
            var lessonViewModel = new LessonViewModel
            {
                Title = Convert.ToString(lessonRow["Title"]),
                Duration = durationString != null ? TimeSpan.Parse(durationString) : TimeSpan.Zero
            };
            return lessonViewModel;
        }
    }
}