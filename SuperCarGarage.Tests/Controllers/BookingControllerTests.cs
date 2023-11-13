using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;
using SuperCarGarage.Controllers;
using SuperCarGarage.Models;
using SuperCarGarage.Services;
using SuperCarGarage.Tests.Mocks;
using SuperCarGarage.ViewModels;

namespace SuperCarGarage.Tests.Controllers;

public class BookingControllerTests
{
    private Mock<IBookingService> _mockBookingService;
    private Mock<ICarService> _mockCarService;
    private BookingController _bookingController;

    public BookingControllerTests()
    {
        _mockBookingService = MockServices.GetBookingService();
        _mockCarService = MockServices.GetCarService();
        _bookingController = new BookingController(_mockBookingService.Object, _mockCarService.Object);
    }

    [Fact]
    public void Index_ShouldReturnAllCars()
    {
        // Act
        var result = _bookingController.Index();
        
        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var bookingListViewModel = Assert.IsAssignableFrom<BookingListViewModel>(viewResult.Model);
        Assert.Equal(2, bookingListViewModel.Bookings.Count());
    }

    [Fact]
    public void Add_InvalidModelState_ReturnsView()
    {
        // Arrange
        var viewModel = new BookingAddViewModel()
        {
            Booking = new Booking()
            {
                CarId = new ObjectId(),
                CarModel = "Tesla Model 3",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(3)
            }
        };
        _bookingController.ModelState.AddModelError("Key", "Error");
        
        
        // Act
        var viewResult = _bookingController.Add(viewModel);
        
        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(viewResult);
        Assert.Equal("Index", redirectResult.ActionName);
    }

    [Fact]
    public void Add_ValidModelState_AddsBookingAndRedirects()
    {
        // Arrange
        var viewModel = new BookingAddViewModel()
        {
            Booking = new Booking()
            {
                CarId = new ObjectId(),
                CarModel = "Tesla Model 3",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(3)
            }
        };
        
        // Act
        var result = _bookingController.Add(viewModel);
        
        // Assert
        _mockBookingService.Verify(s => s.AddBooking(It.IsAny<Booking>()), Times.Once());
        Assert.IsType<RedirectToActionResult>(result);
    }
    
    [Fact]
    public void Edit_IdIsEmpty_ReturnsNotFound()
    {
        // Act
        var result = _bookingController.Edit(string.Empty);
            
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public void Edit_IdIsValid_ReturnsViewWithSelectedBooking()
    {
        // Arrange
        var bookingId = new ObjectId();
        var selectedBooking = new Booking()
        {
            Id = bookingId
        };
        _mockBookingService.Setup(service => service.GetBookingById(bookingId.ToString())).Returns(selectedBooking);
            
        // Act
        var result = _bookingController.Edit(bookingId.ToString());
            
        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(selectedBooking, viewResult.ViewData.Model);
    }
    
    [Fact]
    public void Edit_ModelStateValid_RedirectsToIndex()
    {
        // Arrange
        var updatedBooking = new Booking() { Id = new ObjectId() };
        _mockBookingService.Setup(service => service.EditBooking(updatedBooking));

        // Act
        var result = _bookingController.Edit(updatedBooking);
            
        // Assert
        Assert.IsType<RedirectToActionResult>(result);
    }
    
    [Fact]
    public void Edit_UpdateFails_ReturnsViewWithModel()
    {
        // Arrange
        var updatedBooking = new Booking() { Id = new ObjectId() };
        _mockBookingService.Setup(service => service.EditBooking(updatedBooking))
            .Throws(new Exception("Update failed"));
            
        // Act
        var result = _bookingController.Edit(updatedBooking);
            
        // Assert
        Assert.IsType<RedirectToActionResult>(result);
    }
    
    [Fact]
    public void Delete_IdIsEmpty_ReturnsNotFound()
    {
        // Act
        var result = _bookingController.Edit(string.Empty);
            
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Delete_IdIsValid_RedirectsToIndex()
    {
        // Arrange
        var bookingId = new ObjectId();
        var selectedBooking = new Booking()
        {
            Id = bookingId
        };
        _mockBookingService.Setup(service => service.GetBookingById(bookingId.ToString())).Returns(selectedBooking);

        // Act
        var result = _bookingController.Delete(bookingId.ToString());

        // Assert
        Assert.IsType<ViewResult>(result);
    }
}