using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Projekat
{
    public class Locations
    {
        public Locations()
        {
            Cars = new HashSet<Cars>();
        }

        [Key]
        [Required]
        public string LocationID { get; set; }

        public string Address { get; set; }

        [Required]
        public string Manager { get; set; }

        [Range(1, 10000)]
        public int Capacity { get; set; }

        public string OpeningHours { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        [Range(0, 1000)]
        public int StaffCount { get; set; }

        // Navigation property back to Cars
        public virtual ICollection<Cars> Cars { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Location ID: {LocationID}");
            sb.AppendLine($"Address: {Address}");
            sb.AppendLine($"Manager: {Manager}");
            sb.AppendLine($"Capacity: {Capacity}");
            sb.AppendLine($"Opening Hours: {OpeningHours}");
            sb.AppendLine($"Phone Number: {PhoneNumber}");
            sb.AppendLine($"Email: {Email}");
            sb.AppendLine($"Staff Count: {StaffCount}");
            sb.AppendLine("Cars:");

            //foreach (var car in Cars)
            //{
            //    sb.AppendLine(car.ToString());
            //}
            if (Cars == null || !Cars.Any())
            {
                sb.AppendLine("  No cars available.");
            }
            else
            {
                foreach (var car in Cars)
                {
                    
                    sb.AppendLine($"{car.GetModelAndLicensePlate()}");
                }
            }

            return sb.ToString();
        }
        public static bool IsValidOpeningHours(string openingHours)
        {
            string pattern = @"^([01]?[0-9]|2[0-3]):[0-5][0-9] - ([01]?[0-9]|2[0-3]):[0-5][0-9]$";
            return Regex.IsMatch(openingHours, pattern);
        }
    }
}
