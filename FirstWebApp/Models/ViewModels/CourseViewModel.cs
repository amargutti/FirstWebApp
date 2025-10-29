using FirstWebApp.Models.Enums;
using FirstWebApp.Models.ValueTypes;
using System.Data;

namespace FirstWebApp.Models.ViewModels
{
    public class CourseViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public string Author { get; set; }
        public double Rating { get; set; }
        public Money FullPrice { get; set; }
        public Money CurrentPrice { get; set; }

        public static CourseViewModel FromDataRow(DataRow courseRow)
        {
            var courseViewModel = new CourseViewModel
            {
                Id = (int)courseRow["Id"],
                //modo più sicuro per convertire in stringa ? cos^ anche se null ti restituisce stringa vuouta
                Title = "" + courseRow["Title"],
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
                )
            };
            return courseViewModel;
        }
    }
}