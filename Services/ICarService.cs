using MongoDB.Driver;
using SuperCarGarage.Models;

namespace SuperCarGarage.Services
{
    public interface ICarService
    {
        IEnumerable<Car> GetAllCars();
        Car? GetCarById(string id);

        void AddCar(Car newCar);

        void EditCar(Car updatedCar);

        void DeleteCar(Car carToDelete);
    }
}
