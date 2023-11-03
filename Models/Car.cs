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
        public string? Model { get; set; }
        public string? Location { get; set; }
        public bool IsBooked { get; set; } = false;
    }
}
