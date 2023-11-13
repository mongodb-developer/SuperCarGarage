using Microsoft.AspNetCore.Mvc;
using Moq;
using SuperCarGarage.Controllers;
using SuperCarGarage.Models;
using SuperCarGarage.Services;
using SuperCarGarage.Tests.Mocks;
using SuperCarGarage.ViewModels;
using MongoDB.Bson;

namespace SuperCarGarage.Tests.Controllers
{
    public class CarControllerTests
    {
        Mock<ICarService> _mockCarService;
        CarController _carController;

        public CarControllerTests()
        {
            // Shared Arrange
            _mockCarService = MockServices.GetCarService();
            _carController = new CarController(_mockCarService.Object);
        }

        [Fact]
        public void Index_ReturnsAllCars()
        {
            // Act
            var result = _carController.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var carListViewModel = Assert.IsAssignableFrom<CarListViewModel>(viewResult.Model);
            Assert.Equal(4, carListViewModel.Cars.Count());
        }

        [Fact]
        public void Add_InvalidModelState_ReturnsView()
        {
            // Arrange
            _carController.ModelState.AddModelError("Key", "Error");

            // Act
            var result = _carController.Add(new CarAddViewModel());

            // Assert 
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Add_ValidModelState_AddsCarAndRedirects()
        {
            // Arrange
            var model = new CarAddViewModel()
            {
                Car = new Car()
                {
                    Model = "VW Golf",
                    NumberPlate = "MM72 IOS",
                    Location = "Manchester Hardman Square",
                    IsBooked = true,
                }
            };

            // Act
            var result = _carController.Add(model);

            // Assert
            _mockCarService.Verify(s => s.AddCar(It.IsAny<Car>()), Times.Once());
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void Edit_IdIsEmpty_ReturnsNotFound()
        {
            // Act
            var result = _carController.Edit(string.Empty);
            
            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Edit_IdIsValid_ReturnsViewWithSelectedCar()
        {
            // Arrange
            var carId = new ObjectId();
            var selectedCar = new Car()
            {
                Id = carId
            };
            _mockCarService.Setup(service => service.GetCarById(carId.ToString())).Returns(selectedCar);
            
            // Act
            var result = _carController.Edit(carId.ToString());
            
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(selectedCar, viewResult.ViewData.Model);
        }

        [Fact]
        public void Edit_ModelStateValid_RedirectsToIndex()
        {
            // Arrange
            var updatedCar = new Car() { Id = new ObjectId() };
            _mockCarService.Setup(service => service.EditCar(updatedCar));

            // Act
            var result = _carController.Edit(updatedCar);
            
            // Assert
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void Edit_ModelStateInvalid_ReturnsBadRequest()
        {
            // Arrange
            var updatedCar = new Car() { Id = new ObjectId() };
            _carController.ModelState.AddModelError("Key", "Error");
            
            // Act
            var result = _carController.Edit(updatedCar);
            
            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Edit_UpdateFails_ReturnsViewWithModel()
        {
            // Arrange
            var updatedCar = new Car() { Id = new ObjectId() };
            _mockCarService.Setup(service => service.EditCar(updatedCar))
                .Throws(new Exception("Update failed"));
            
            // Act
            var result = _carController.Edit(updatedCar);
            
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(updatedCar, viewResult.ViewData.Model);
        }
        
        [Fact]
        public void Delete_IdIsEmpty_ReturnsNotFound()
        {
            // Act
            var result = _carController.Delete(string.Empty);
            
            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Delete_IdIsValid_ReturnsViewWithSelectedCar()
        {
            // Arrange
            var carId = new ObjectId();
            var selectedCar = new Car()
            {
                Id = carId
            };
            _mockCarService.Setup(service => service.GetCarById(carId.ToString())).Returns(selectedCar);
            
            // Act
            var result = _carController.Delete(carId.ToString());
            
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(selectedCar, viewResult.ViewData.Model);
        }

        [Fact]
        public void Delete_IdIsInvalid_ReturnsViewWithErrorMessage()
        {
            // Arrange
            var car = new Car()
            {
                Id = ObjectId.Empty
            };
            
            // Act
            var result = _carController.Delete(car);
            
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Deleting the car failed, invalid ID!", viewResult.ViewData["ErrorMessage"]);
        }

        [Fact]
        public void Delete_DeleteSucceeds_RedirectsToIndex()
        {
            // Arrange
            var car = new Car()
            {
                Id = new ObjectId()
            };
            _mockCarService.Setup(service => service.DeleteCar(car));
            
            // Act
            var result = _carController.Delete(car);
            
            // Assert
         Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Delete_DeleteFails_ReturnsViewWithErrorMessage()
        {
            // Arrange
            var car = new Car()
            {
                Id = new ObjectId()
            };
            _mockCarService.Setup(service => service.DeleteCar(car))
                .Throws<Exception>();
            
            // Act
            var result = _carController.Delete(car);
            
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Contains("Deleting the car failed", viewResult.ViewData["ErrorMessage"] as string);
        }
    }
}
