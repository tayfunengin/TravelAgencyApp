using Domain.Entities;
using Domain.Enums;
using Repository;
using Service.CarService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Service.RentACarCompanyService
{
    public class RentACarCompanyService : IRentACarCompanyService
    {
        private readonly IRepository<RentACarCompany> repository;
        private readonly ICarService carService;

        public RentACarCompanyService(IRepository<RentACarCompany> repository, ICarService carService)
        {
            this.repository = repository;
            this.carService = carService;
        }

        public void AddCarsToRentACarCompany(int[] selectedIds, int rentACarCompanyId)
        {
            RentACarCompany rentACarCompanyUpdated = this.GetRentACarCompanyById(rentACarCompanyId);
            if (rentACarCompanyUpdated.Cars == null)
            {
                rentACarCompanyUpdated.Cars = new List<Car>();
            }
            foreach (var carId in selectedIds)
            {
                Car car = carService.GetCarById(carId);
                rentACarCompanyUpdated.Cars.Add(car);
            }
        }

        public string CreateRentACarCompany(RentACarCompany rentACarCompany, int[] carIds)
        {
            using (var transactionScope = new TransactionScope())
            {

                string result = this.repository.Insert(rentACarCompany);

                string type = result.Split(':')[0];
                var message = result.Split(':')[1];
                if (type == NotificationType.exception.ToString())
                    return result;
                else
                {
                    int maxRentACarCompanyId = this.GetLastestRentACarCompanyId();
                    this.AddCarsToRentACarCompany(carIds, maxRentACarCompanyId);
                }

                transactionScope.Complete();
                return result;
            }
        }

        public string DeleteRentACarCompany(int id)
        {
            return this.repository.Delete(id);
        }

        public int GetLastestRentACarCompanyId()
        {
            return this.repository.GetAll().Max(x => x.Id);
        }

        public IEnumerable<RentACarCompany> GetRentACarCompanies()
        {
            return this.repository.GetAll();
        }

        public RentACarCompany GetRentACarCompanyById(int id)
        {
            return this.repository.GetById(id);
        }

        public List<Car> GetUnAssignedCars(int rentACarCompanyId)
        {
            List<Car> cars = this.carService.GetCars().Where(x => x.Status == Convert.ToBoolean(Status.Active)).ToList();
            RentACarCompany rentACarCompany = this.GetRentACarCompanyById(rentACarCompanyId);
            List<Car> unAssginedCars = new List<Car>();
            if (rentACarCompany.Cars.Count > 0)
            {
                foreach (var car in cars)
                {
                    if (!rentACarCompany.Cars.Contains(car))
                        unAssginedCars.Add(car);
                }
                return unAssginedCars;
            }
            else
            {
                return cars;
            }
        }

        public bool IfRentACarCompanyCodeExists(string code)
        {
            RentACarCompany rentACarCompany = this.repository.GetDefault(x => x.RentACarCompanyCode == code).FirstOrDefault();
            if (rentACarCompany == null)
                return false;
            else
                return true;
        }

        public string UpdateRentACarCompany(RentACarCompany rentACarCompany)
        {
            return this.repository.Update(rentACarCompany);
        }
    }
}
