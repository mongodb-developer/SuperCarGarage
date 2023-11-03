using Microsoft.AspNetCore.Mvc;
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
            Car newCar = new()
            {
                Model = carAddViewModel.Car.Model,
                Location = carAddViewModel.Car.Location
            };

            _carService.AddCar(newCar);
            return RedirectToAction("Index");           
        }
    }
}
