using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using SuperCarGarage.Models;

namespace SuperCarGarage.Services
{
    public class BookingService : IBookingService
    {
        private readonly CarBookingDbContext _carDbContext;

        public BookingService(CarBookingDbContext carDBContext)
        {
            _carDbContext = carDBContext;
        }
        public void AddBooking(Booking newBooking)
        {
            var bookedCar = _carDbContext.Cars.FirstOrDefault(c => c.Id == newBooking.CarId);
            if (bookedCar == null)
            {
                throw new ArgumentException("The car to be booked cannot be found.");
            }

            newBooking.CarModel = bookedCar.Model;

            bookedCar.IsBooked = true;
            _carDbContext.Cars.Update(bookedCar);

            _carDbContext.Bookings.Add(newBooking);

            _carDbContext.ChangeTracker.DetectChanges();
            Console.WriteLine(_carDbContext.ChangeTracker.DebugView.LongView);

            _carDbContext.SaveChanges();
        }

        public void DeleteBooking(Booking booking)
        {
            var bookedCar = _carDbContext.Cars.FirstOrDefault(c => c.Id == booking.CarId);
            bookedCar.IsBooked = false;

            var bookingToDelete = _carDbContext.Bookings.FirstOrDefault(b => b.Id == booking.Id);

            if(bookingToDelete != null)
            {
                _carDbContext.Bookings.Remove(bookingToDelete);
                _carDbContext.Cars.Update(bookedCar);

                _carDbContext.ChangeTracker.DetectChanges();
                Console.WriteLine(_carDbContext.ChangeTracker.DebugView.LongView);

                _carDbContext.SaveChanges();
            }
            else
            {
                throw new ArgumentException("The booking to delete cannot be found.");
            }
        }

        public void EditBooking(Booking updatedBooking)
        {
           var bookingToUpdate = _carDbContext.Bookings.FirstOrDefault(b => b.Id == updatedBooking.Id);
           
            
            if (bookingToUpdate != null)
            {               
                bookingToUpdate.StartDate = updatedBooking.StartDate;
                bookingToUpdate.EndDate = updatedBooking.EndDate;
                

                _carDbContext.Bookings.Update(bookingToUpdate);

                _carDbContext.ChangeTracker.DetectChanges();
                _carDbContext.SaveChanges();

                Console.WriteLine(_carDbContext.ChangeTracker.DebugView.LongView);
            }  
            else 
            { 
                throw new ArgumentException("Booking to be updated cannot be found");
            }
            
        }

        public IEnumerable<Booking> GetAllBookings()
        {
            return _carDbContext.Bookings.OrderBy(b => b.StartDate).AsNoTracking().AsEnumerable<Booking>();
        }

        public Booking? GetBookingById(ObjectId id)
        {
            return _carDbContext.Bookings.AsNoTracking().FirstOrDefault(b => b.Id == id);
        }
        
    }
}
