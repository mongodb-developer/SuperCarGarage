using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SuperCarGarage.Models
{
    [Collection("cars")]    
    public class Car
    {
       
        public ObjectId Id { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "You must provide the make and model")]
        [Display(Name = "Make and Model")]
        public string? Model { get; set; }

        [StringLength(10)]
        [Required(ErrorMessage = "The number plate is required to identify the vehicle")]
        public string NumberPlate { get; set; }

        [Required(ErrorMessage = "You must add the location of the car")]
        public string? Location { get; set; }
    }
}
