using FirstWebApp.Models.Enums;
using FirstWebApp.Models.ValueTypes;
using FirstWebApp.Models.ViewModels;

namespace FirstWebApp.Models.Services.Application
{
    public class CourseService : ICourseService
    {
        public List<CourseViewModel> GetCourses() { 
            var courseList = new List<CourseViewModel>();
            var rand = new Random();

            for (int i = 1; i <= 20; i++)
            {
                var price = Convert.ToDecimal(rand.NextDouble() * 10 + 10);
                var course = new CourseViewModel
                {
                    Id = i,
                    Title = $"Corso {i}",
                    CurrentPrice = new Money(Currency.EUR, price),
                    FullPrice = new Money(Currency.EUR, price + Convert.ToDecimal(rand.NextDouble() > 0.5 ? price : price - 1)),
                    Author = "Nome cognome",
                    Rating = rand.NextDouble() * 5.0,
                    ImagePath = "/logo.svg",
                };
                courseList.Add(course);
            }
            return courseList;
        }

        public CourseDetailViewModel GetCourse(string id)
        {
            var rand = new Random();
            decimal price = Convert.ToDecimal(rand.NextDouble() * 10 + 10);
            var course = new CourseDetailViewModel
            {
                Id = Convert.ToInt32(id),
                Title = $"Corso {id}",
                CurrentPrice = new Money(Currency.EUR, price),
                FullPrice = new Money(Currency.EUR, price + Convert.ToDecimal(rand.NextDouble() > 0.5 ? price : price - 1)),
                Author = "Nome cognome",
                ImagePath = "/logo.svg",
                Rating = rand.Next(10, 50) / 5.0,
                Description = $"Descrizione del corso {id}",
                Lessons = new List<LessonViewModel>()
            };

            for(int i = 1; i <= 10; i++)
            {
                var lesson = new LessonViewModel
                {
                    Title = $"Lezione {i}",
                    Duration = TimeSpan.FromMinutes(rand.Next(5, 61))
                };
                course.Lessons.Add(lesson);
            }
            return course;
        }
    }
}
