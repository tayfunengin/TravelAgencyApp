using Domain.Entities;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.CarService
{
    public class CarService : ICarService
    {
        private readonly IRepository<Car> repository;

        public CarService(IRepository<Car> repository)
        {
            this.repository = repository;
        }

        public string CreateCar(Car car)
        {
           return this.repository.Insert(car);
        }

        public string DeleteCar(int id)
        {
            return this.repository.Delete(id);
        }

        public Car GetCarById(int id)
        {
            return this.repository.GetById(id);
        }

        public IEnumerable<Car> GetCars()
        {
            return repository.GetAll();
        }

        public string UpdateCar(Car car)
        {
            return this.repository.Update(car);
        }
    }
}
