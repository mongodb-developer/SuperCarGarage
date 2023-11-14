using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using SuperCarGarage.Models;
using SuperCarGarage.Services;
using SuperCarGarage.ViewModels;

namespace SuperCarGarage.Controllers
{
    public class CarController : Controller
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }
        public IActionResult Index()
        {
            CarListViewModel viewModel = new()
            {
                Cars = _carService.GetAllCars(),
            };
            return View(viewModel);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(CarAddViewModel carAddViewModel)
        {
            if(ModelState.IsValid)
            {
                Car newCar = new()
                {
                    Model = carAddViewModel.Car.Model,
                    Location = carAddViewModel.Car.Location,
                    NumberPlate = carAddViewModel.Car.NumberPlate
                };

                _carService.AddCar(newCar);
                return RedirectToAction("Index");
            }

            return View(carAddViewModel);         
        }

        public IActionResult Edit(ObjectId id)
        {
            if(id == null || id == ObjectId.Empty)
            {
                return NotFound();
            }

            var selectedCar = _carService.GetCarById(id);
            return View(selectedCar);
        }

        [HttpPost]
        public IActionResult Edit(Car car)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    _carService.EditCar(car);
                    return RedirectToAction("Index");
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Updating the car failed, please try again! Error: {ex.Message}");
            }

            return View(car);
        }

        public IActionResult Delete(ObjectId id) {
            if (id == null || id == ObjectId.Empty)
            {
                return NotFound();
            }

            var selectedCar = _carService.GetCarById(id);
            return View(selectedCar);
        }

        [HttpPost]
        public IActionResult Delete(Car car)
        {
            if (car.Id == null || car.Id == ObjectId.Empty)
            {
                ViewData["ErrorMessage"] = "Deleting the car failed, invalid ID!";
                return View();
            }

            try
            {
                _carService.DeleteCar(car);
                TempData["CarDeleted"] = "Car deleted successfully!";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = $"Deleting the car failed, please try again! Error: {ex.Message}";
            }

            var selectedCar = _carService.GetCarById(car.Id);
            return View(selectedCar);
        }        
    }
}
