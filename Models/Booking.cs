using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SuperCarGarage.Models
{
    [Collection("bookings")]
    public class Booking
    {
        public ObjectId Id { get; set; }

        public ObjectId CarId { get; set; }

        public string CarModel { get; set; }

        [Required(ErrorMessage = "The start date is required to make this booking")]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "The end date is required to make this booking")]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
    }

}
