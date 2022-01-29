using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.CarService
{
    public interface ICarService
    {
        IEnumerable<Car> GetCars();

        Car GetCarById(int id);

        string CreateCar(Car car);

        string UpdateCar(Car car);

        string DeleteCar(int id);
    }
}
