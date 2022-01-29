using Domain.Entities;
using Domain.Enums;
using Repository;
using Service.FlightPriceService;
using Service.HotelPriceService;
using Service.RentACarPriceService;
using Service.TourDateInPackageService;
using Service.TourGuidePriceService;
using Service.TourOperatorPriceService;
using Service.TransportationPriceService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Service.PackageService
{
    public class PackageService : IPackageService
    {
        private readonly IRepository<Package> repository;
        private readonly IHotelPriceService hotelPriceService;
        private readonly IFlightPriceService flightPriceService;
        private readonly ITransportationPriceService transportationPriceService;
        private readonly ITourOperatorPriceService tourOperatorPriceService;
        private readonly IRentACarPriceService rentACarPriceService;
        private readonly ITourGuidePriceService tourGuidePriceService;
        private readonly ITourDateInPackageService tourDateInPackageService;

        public PackageService(IRepository<Package> repository, IHotelPriceService hotelPriceService, IFlightPriceService flightPriceService, ITransportationPriceService transportationPriceService, ITourOperatorPriceService tourOperatorPriceService, IRentACarPriceService rentACarPriceService, ITourGuidePriceService tourGuidePriceService, ITourDateInPackageService tourDateInPackageService)
        {
            this.repository = repository;
            this.hotelPriceService = hotelPriceService;
            this.flightPriceService = flightPriceService;
            this.transportationPriceService = transportationPriceService;
            this.tourOperatorPriceService = tourOperatorPriceService;
            this.rentACarPriceService = rentACarPriceService;
            this.tourGuidePriceService = tourGuidePriceService;
            this.tourDateInPackageService = tourDateInPackageService;
        }

        public string AddFlightPriceToPackage(int flightPriceId, int packageId)
        {
            try
            {
                Package packageUpdated = this.repository.GetById(packageId);
                FlightPrice flightPrice = flightPriceService.GetFlightPriceById(flightPriceId);

                if (packageUpdated.FlightPrices == null)
                {
                    packageUpdated.FlightPrices = new List<FlightPrice>();
                }
                packageUpdated.FlightPrices.Add(flightPrice);
                return NotificationType.success + ":" + "Transaction has been completed successfully.";
            }
            catch (Exception ex)
            {
                return NotificationType.exception + ":" + ex;
            }
        }

        public string AddHotelPriceToPackage(int hotelPriceId, int packageId)
        {
            try
            {
                Package packageUpdated = this.repository.GetById(packageId);
                HotelPrice hotelPrice = hotelPriceService.GetHotelPriceById(hotelPriceId);
                if (packageUpdated.GuestCount == 2 && hotelPrice.RoomType.RoomTypeCode.ToUpper() == "SGL")
                {
                    return NotificationType.error + ":" + "Single room can not be defined for packages with 2 guests!";
                }
                else if (packageUpdated.GuestCount == 3 && (hotelPrice.RoomType.RoomTypeCode.ToUpper() == "SGL" || hotelPrice.RoomType.RoomTypeCode.ToUpper() == "DBL"))
                {
                    return NotificationType.error + ":" + "Only triple room can be defined for the packages with 3 guests!";
                }
                if (packageUpdated.HotelPrice == null)
                {
                    packageUpdated.HotelPrice = new HotelPrice();
                }
                packageUpdated.HotelPrice = hotelPrice;
                return NotificationType.success + ":" + "Transaction has been completed successfully.";
            }
            catch (Exception ex)
            {
                return NotificationType.exception + ":" + ex;
            }

        }

        public string AddRentACarPriceToPackage(int rentACarPriceId, DateTime beginDate, DateTime endDate, int packageId)
        {
            try
            {
                Package packageUpdated = this.repository.GetById(packageId);
                RentACarPrice rentACarPrice = rentACarPriceService.GetRentACarPriceById(rentACarPriceId);

                if (packageUpdated.RentACarPrice == null)
                {
                    packageUpdated.RentACarPrice = new RentACarPrice();
                }
                packageUpdated.RentACarPrice = rentACarPrice;
                packageUpdated.RentACarStartDate = beginDate;
                packageUpdated.RentACarEndDate = endDate;
                return NotificationType.success + ":" + "Transaction has been completed successfully.";
            }
            catch (Exception ex)
            {
                return NotificationType.exception + ":" + ex;
            }
        }

        public string AddTourGuidePriceToPackage(int tourGuidePriceId, DateTime date, int packageId)
        {
            try
            {
                Package packageUpdated = this.repository.GetById(packageId);
                TourGuidePrice tourGuidePrice = tourGuidePriceService.GetTourGuidePriceById(tourGuidePriceId);

                if (packageUpdated.TourGuidePrice == null)
                {
                    packageUpdated.TourGuidePrice = new TourGuidePrice();
                }
                packageUpdated.TourGuidePrice = tourGuidePrice;
                packageUpdated.TourGuideRentDay = date;
                return NotificationType.success + ":" + "Transaction has been completed successfully.";
            }
            catch (Exception ex)
            {
                return NotificationType.exception + ":" + ex;
            }
        }

        public string AddTourOperatorPriceToPackage(int tourOperatorPriceId, int packageId, DateTime tourDate)
        {
            try
            {
                using (var transactionScope = new TransactionScope())
                {

                    Package packageUpdated = this.repository.GetById(packageId);
                    TourOperatorPrice tourOperatorPrice = tourOperatorPriceService.GetTourOperatorPriceById(tourOperatorPriceId);

                    TourDateInPackage isTourDateExist = tourDateInPackageService.IsTourDateExist(packageId, tourDate);
                    if (isTourDateExist != null)
                    {
                        return NotificationType.error + ":" + $"There is already another tour that has been assigned to date {tourDate.ToShortDateString()}!";
                    }

                    if (packageUpdated.TourPrices == null)
                    {
                        packageUpdated.TourPrices = new List<TourOperatorPrice>();
                    }
                    packageUpdated.TourPrices.Add(tourOperatorPrice);
                    TourDateInPackage tourDateInPackage = new TourDateInPackage()
                    {
                        PackageId = packageId,
                        TourPriceId = tourOperatorPriceId,
                        TourDate = tourDate
                    };
                    this.tourDateInPackageService.Create(tourDateInPackage);
                    transactionScope.Complete();
                    return NotificationType.success + ":" + "Transaction has been completed successfully.";
                }
            }
            catch (Exception ex)
            {
                return NotificationType.exception + ":" + ex;
            }
        }

        public string AddTransportationPriceToPackage(int transportationPriceId, int packageId)
        {
            try
            {
                Package packageUpdated = this.repository.GetById(packageId);
                TransportationPrice transportationPrice = transportationPriceService.GetTransportationPriceById(transportationPriceId);

                if (packageUpdated.TransportationPrices == null)
                {
                    packageUpdated.TransportationPrices = new List<TransportationPrice>();
                }
                packageUpdated.TransportationPrices.Add(transportationPrice);
                return NotificationType.success + ":" + "Transaction has been completed successfully.";
            }
            catch (Exception ex)
            {
                return NotificationType.exception + ":" + ex;
            }
        }

        public string CreatePackage(Package package)
        {
            return this.repository.Insert(package);
        }

        public string DeletePackage(int id)
        {
            return this.repository.Delete(id);
        }

        public IEnumerable<Package> GetActivePackages()
        {
            return this.repository.GetDefault(x => x.Status == Convert.ToBoolean(Status.Active));
        }

        public IEnumerable<Package> GetByPeriodAndRegion(Period period, int regionId)
        {
            List<Package> packages = new List<Package>();

            if (period != 0 && regionId != 0)
            {
                packages = this.repository.GetDefault(x => x.Status == Convert.ToBoolean(Status.Active) && x.Period == period && x.RegionId == regionId && x.Year == DateTime.Now.Year.ToString()).ToList();
            }
            else if (period == 0 && regionId != 0)
            {
                packages = this.repository.GetDefault(x => x.Status == Convert.ToBoolean(Status.Active) && x.RegionId == regionId && x.Year == DateTime.Now.Year.ToString()).ToList();
            }
            else if (period != 0 && regionId == 0)
            {
                packages = this.repository.GetDefault(x => x.Status == Convert.ToBoolean(Status.Active) && x.Period == period && x.Year == DateTime.Now.Year.ToString()).ToList();
            }
            else
            {
                packages = this.GetActivePackages().ToList();
            }
            return packages;
        }

        public Package GetPackageById(int id)
        {
            return this.repository.GetById(id);
        }

        public IEnumerable<Package> GetPackages()
        {
            return this.repository.GetAll();
        }

        public IEnumerable<Package> GetPackagesByWhereClause(Period period, string year)
        {
            List<Package> packages = new List<Package>();

            if (!string.IsNullOrEmpty(year) && period != 0)
            {
                packages = this.repository.GetDefault(x => x.Status == Convert.ToBoolean(Status.Active) && x.Period == period && x.Year == year).ToList();
            }
            else if (!string.IsNullOrEmpty(year) && period == 0)
            {
                packages = this.repository.GetDefault(x => x.Status == Convert.ToBoolean(Status.Active) && x.Year == year).ToList();
            }
            else if (string.IsNullOrEmpty(year) && period != 0)
            {
                packages = this.repository.GetDefault(x => x.Status == Convert.ToBoolean(Status.Active) && x.Period == period).ToList();
            }
            else
            {
                packages = this.GetActivePackages().ToList();
            }
            return packages;
        }

        public IEnumerable<FlightPrice> GetUnAssignedFlightPricesForPackage(int packageId)
        {
            Package package = this.repository.GetById(packageId);
            IEnumerable<FlightPrice> flightPrices = this.flightPriceService.GetFlightPrices().Where(x => x.Period == package.Period && x.Status == Convert.ToBoolean(Status.Active) && x.Year == package.Year);
            List<FlightPrice> unAssignedFlightPrices = new List<FlightPrice>();

            if (package.FlightPrices.Count > 0)
            {
                foreach (var flightPrice in flightPrices)
                {
                    if (!package.FlightPrices.Contains(flightPrice))
                        unAssignedFlightPrices.Add(flightPrice);
                }
                return unAssignedFlightPrices;
            }
            else
            {
                return flightPrices;
            }
        }

        public IEnumerable<TourOperatorPrice> GetUnAssignedTourOperatorPricesForPackage(int packageId)
        {
            Package package = this.repository.GetById(packageId);
            IEnumerable<TourOperatorPrice> tourOperatorPrices = this.tourOperatorPriceService.GetTourOperatorPrices().Where(x => x.Period == package.Period && x.Status == Convert.ToBoolean(Status.Active) && x.Tour.Region == package.Region && x.Year == package.Year);
            List<TourOperatorPrice> unAssignedtourOperatorPrices = new List<TourOperatorPrice>();

            if (package.TransportationPrices.Count > 0)
            {
                foreach (var tourOperatorPrice in tourOperatorPrices)
                {
                    if (!package.TourPrices.Contains(tourOperatorPrice))
                        unAssignedtourOperatorPrices.Add(tourOperatorPrice);
                }
                return unAssignedtourOperatorPrices;
            }
            else
            {
                return tourOperatorPrices;
            }
        }

        public IEnumerable<TransportationPrice> GetUnAssignedTransportationPricesForPackage(int packageId)
        {
            Package package = this.repository.GetById(packageId);
            IEnumerable<TransportationPrice> transportationPrices = this.transportationPriceService.GetTransportationPrices().Where(x => x.Period == package.Period && x.Status == Convert.ToBoolean(Status.Active) && x.TransportationCompany.Region == package.Region && x.Year == package.Year);
            List<TransportationPrice> unAssignedtransportationPrices = new List<TransportationPrice>();

            if (package.TransportationPrices.Count > 0)
            {
                foreach (var transportationPrice in transportationPrices)
                {
                    if (!package.TransportationPrices.Contains(transportationPrice))
                        unAssignedtransportationPrices.Add(transportationPrice);
                }
                return unAssignedtransportationPrices;
            }
            else
            {
                return transportationPrices;
            }
        }

        public bool IfPackageNameExist(string packageName)
        {
            Package package = this.repository.GetDefault(x => x.PackageName == packageName).FirstOrDefault();
            if (package == null)
                return false;
            else
                return true;
        }

        public string RemoveTourOperatorPriceFromPackage(int tourOperatorPriceId, int packageId)
        {
            try
            {
                using (var transactionScope = new TransactionScope())
                {
                    Package packageUpdated = this.repository.GetById(packageId);
                    TourOperatorPrice tourOperatorPrice = tourOperatorPriceService.GetTourOperatorPriceById(tourOperatorPriceId);
                    packageUpdated.TourPrices.Remove(tourOperatorPrice);
                    TourDateInPackage tourDateInPackage = tourDateInPackageService.GetByPackageIdAndTourPriceId(packageId, tourOperatorPriceId);
                    tourDateInPackageService.Delete(tourDateInPackage.Id);
                    transactionScope.Complete();

                    return NotificationType.success + ":" + "Transaction has been completed successfully.";
                }
            }
            catch (Exception ex)
            {
                return NotificationType.exception + ":" + ex;
            }
        }

        public string UpdatePackage(Package package)
        {
            return this.repository.Update(package);
        }
    }
}
