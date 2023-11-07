using Microsoft.AspNetCore.Mvc;
using SuperCarGarage.Models;
using SuperCarGarage.Services;
using SuperCarGarage.ViewModels;

namespace SuperCarGarage.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly ICarService _carService;        

        public BookingController(IBookingService bookingService, ICarService carService)
        {
            _bookingService = bookingService;
            _carService = carService;
        }

        public IActionResult Index()
        {
            BookingListViewModel viewModel = new BookingListViewModel()
            {
                Bookings = _bookingService.GetAllBookings()
            };
            return View(viewModel);
        }

        public IActionResult Add(string carId)
        {
            var selectedCar = _carService.GetCarById(carId);
            
            BookingAddViewModel bookingAddViewModel = new BookingAddViewModel();

            bookingAddViewModel.Booking = new Booking();
            bookingAddViewModel.Booking.CarId = selectedCar.Id;
            bookingAddViewModel.Booking.CarModel = selectedCar.Model;
            bookingAddViewModel.Booking.StartDate = DateTime.UtcNow;
            bookingAddViewModel.Booking.EndDate = DateTime.UtcNow.AddDays(1);

            return View(bookingAddViewModel);
        }

        [HttpPost]
        public IActionResult Add(BookingAddViewModel bookingAddViewModel)
        {
                Booking newBooking = new()
                {
                    CarId = bookingAddViewModel.Booking.CarId,                   
                    StartDate = bookingAddViewModel.Booking.StartDate,
                    EndDate = bookingAddViewModel.Booking.EndDate,
                };

                _bookingService.AddBooking(newBooking);
                return RedirectToAction("Index");   
        }

        public IActionResult Edit(string Id)
        {
            if(Id == null)
            {
                return NotFound();
            }

            var selectedBooking = _bookingService.GetBookingById(Id);
            return View(selectedBooking);
        }

        [HttpPost]
        public IActionResult Edit(Booking booking)
        {
            try
            {
                var existingBooking = _bookingService.GetBookingById(booking.Id.ToString());
                if (existingBooking != null)
                {
                    _bookingService.EditBooking(existingBooking);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", $"Booking with ID {booking.Id} does not exist!");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Updating the booking failed, please try again! Error: {ex.Message}");
            }

            return View(booking);
        }

        public IActionResult Delete(string Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var selectedBooking = _bookingService.GetBookingById(Id);
            return View(selectedBooking);
        }

        [HttpPost]
        public IActionResult Delete(Booking booking)
        {
            if(booking.Id == null)
            {
                ViewData["ErrorMessage"] = "Deleting the booking failed, invalid ID!";
                return View();
            }

            try
            {
                _bookingService.DeleteBooking(booking);
                TempData["BookingDeleted"] = "Booking deleted successfully";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = $"Deleting the booking failed, please try again! Error: {ex.Message}";
            }

            var selectedCar = _bookingService.GetBookingById(booking.Id.ToString());
            return View(selectedCar);
        }
    }
}
