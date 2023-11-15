using MongoDB.Bson;
using SuperCarGarage.Models;

namespace SuperCarGarage.Services
{
    public interface ICarService
    {
        IEnumerable<Car> GetAllCars();
        Car? GetCarById(ObjectId id);

        void AddCar(Car newCar);

        void EditCar(Car updatedCar);

        void DeleteCar(Car carToDelete);
    }
}
