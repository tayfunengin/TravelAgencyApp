using Domain.Entities;
using Domain.Enums;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.PackageService
{
    public interface IPackageService
    {
        IEnumerable<Package> GetPackages();

        IEnumerable<Package> GetActivePackages();

        IEnumerable<Package> GetPackagesByWhereClause(Period period, string year);
        bool IfPackageNameExist(string packageName);
        Package GetPackageById(int id);

        string CreatePackage(Package package);

        string UpdatePackage(Package package);

        string DeletePackage(int id);

        string AddHotelPriceToPackage(int hotelPriceId, int packageId);

        string AddFlightPriceToPackage(int flightPriceId, int packageId);
        string AddTransportationPriceToPackage(int transportationPriceId, int packageId);
        string AddTourOperatorPriceToPackage(int tourOperatorPriceId, int packageId, DateTime tourDate);
        string AddRentACarPriceToPackage(int rentACarPriceId, DateTime beginDate, DateTime endDate, int packageId);

        string AddTourGuidePriceToPackage(int tourGuidePriceId, DateTime date, int packageId);
        IEnumerable<FlightPrice> GetUnAssignedFlightPricesForPackage(int packageId);
        IEnumerable<TransportationPrice> GetUnAssignedTransportationPricesForPackage(int packageId);
        IEnumerable<TourOperatorPrice> GetUnAssignedTourOperatorPricesForPackage(int packageId);

        string RemoveTourOperatorPriceFromPackage(int tourOperatorPriceId, int packageId);

        IEnumerable<Package> GetByPeriodAndRegion(Period period, int regionId);

    }
}
