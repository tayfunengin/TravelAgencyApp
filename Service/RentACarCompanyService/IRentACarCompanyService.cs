using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.RentACarCompanyService
{
    public interface IRentACarCompanyService
    {
        IEnumerable<RentACarCompany> GetRentACarCompanies();

        string CreateRentACarCompany(RentACarCompany rentACarCompany, int[] carIds);

        int GetLastestRentACarCompanyId();

        void AddCarsToRentACarCompany(int[] selectedIds, int rentACarCompanyId);

        RentACarCompany GetRentACarCompanyById(int id);
        bool IfRentACarCompanyCodeExists(string code);

        string UpdateRentACarCompany(RentACarCompany rentACarCompany);

        string DeleteRentACarCompany(int id);

        List<Car> GetUnAssignedCars(int rentACarCompanyId);
    }
}
