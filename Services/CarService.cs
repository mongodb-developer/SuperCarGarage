using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using SuperCarBookingSystem.Services;
using SuperCarGarage.Models;

namespace SuperCarGarage.Services
{
    public class CarService : ICarService
    {
        private readonly CarDbContext _carDbContext;
        public CarService(CarDbContext carDbContext)
        {
            _carDbContext = carDbContext;
        }
        public IEnumerable<Car> GetAllCars()
        {
            // MongoDB _id field can be used to order by chronological time
            return _carDbContext.Cars.OrderBy(c => c.Id).AsNoTracking().ToList();
        }

        public Car? GetCarById(string id)
        {
            return _carDbContext.Cars.Where(car => car.Id.ToString() == id).AsNoTracking().FirstOrDefault();
        }

        public void AddCar(Car car)
        {
            _carDbContext.Cars.Add(car);
            _carDbContext.SaveChanges();
        }

        public void EditCar(Car car)
        {
            var carToUpdate = _carDbContext.Cars.Where(c => c.Id == car.Id).FirstOrDefault();
            _carDbContext.Update(carToUpdate);
        }        

        public void DeleteCar(Car car)
        {
            var carToDelete = _carDbContext.Cars.Where(c => c.Id == car.Id).FirstOrDefault();

            if(carToDelete != null)
            {
                _carDbContext.Cars.Remove(carToDelete);
            }
        }
    }
}
