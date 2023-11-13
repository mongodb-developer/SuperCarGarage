using MongoDB.Bson;
using Moq;
using SuperCarGarage.Models;
using SuperCarGarage.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperCarGarage.Tests.Mocks
{
    public  class MockServices
    {      
        public static Mock<ICarService> GetCarService()
        {
            var cars = new List<Car>
            {
                new Car
                {
                    Model = "VW Polo",
                    NumberPlate = "GK05 UEZ",
                    Location = "Westminster Abbey",
                    IsBooked = true,
                },
                 new Car
                {
                    Model = "VW Golf",
                    NumberPlate = "MM72 UIA IOS",
                    Location = "Manchester Hardman Square",
                    IsBooked = true,
                },
                 new Car
                {
                    Model = "VW Polo",
                    NumberPlate = "MN71 MHS",
                    Location = "Media City UK",
                    IsBooked = false,
                },
                 new Car
                {
                    Model = "Volvo V40",
                    NumberPlate = "KD12 WEN",
                    Location = "Newark Northgate",
                    IsBooked = false,
                },
            };
            var carToAdd = new Car
            {
                Model = "Toyota Corolla",
                NumberPlate = "YS15 GHA",
                Location = "Waterloo",
                IsBooked = false
            };

            var mockCarService = new Mock<ICarService>();
            mockCarService.Setup(service => service.GetAllCars()).Returns(cars);
            mockCarService.Setup(service => service.GetCarById(It.IsAny<string>())).Returns(cars[0]);
            mockCarService.Setup(service => service.AddCar(carToAdd));
            return mockCarService;
        }
    }
}
