﻿using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Edit(string id)
        {
            if(id == null)
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
    }
}
