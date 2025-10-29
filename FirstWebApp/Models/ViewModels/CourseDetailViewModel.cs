
using FirstWebApp.Models.Enums;
using FirstWebApp.Models.ValueTypes;
using System.Data;

namespace FirstWebApp.Models.ViewModels
{
    public class CourseDetailViewModel : CourseViewModel
    {
        public string Description { get; set; }
        public List<LessonViewModel> Lessons { get; set; }
        public TimeSpan TotalDuration 
        { 
            get 
            {
                return Lessons != null ? TimeSpan.FromMinutes(Lessons.Sum(lesson => lesson.Duration.TotalMinutes)) : TimeSpan.Zero;
            }
        }
    public static CourseDetailViewModel FromDataRow(DataRow courseRow)
        {
            var courseViewModel = new CourseDetailViewModel
            {
                Id = (int)courseRow["Id"],
                //modo più sicuro per convertire in stringa ? cos^ anche se null ti restituisce stringa vuouta
                Title = "" + courseRow["Title"],
                Description = Convert.ToString(courseRow["Description"]),
                Author = (string)courseRow["Author"],
                //ImagePath = (string)courseRow["ImagePath"],
                Rating = Convert.ToDouble(courseRow["Rating"]),
                FullPrice = new Money(
                    Enum.Parse<Currency>(Convert.ToString(courseRow["FullPrice_Currency"])),
                    Convert.ToDecimal(courseRow["FullPrice_Amount"])
                ),
                CurrentPrice = new Money(
                    Enum.Parse<Currency>(Convert.ToString(courseRow["CurrentPrice_Currency"])),
                    Convert.ToDecimal(courseRow["CurrentPrice_Amount"])
                ),
                Lessons = new List<LessonViewModel>()
            };
            return courseViewModel;
        }
    };

}
