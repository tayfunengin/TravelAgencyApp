using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.FlightPriceService;
using Service.HotelPriceService;
using Service.PackageService;
using Service.RegionService;
using Service.RentACarPriceService;
using Service.TourGuidePriceService;
using Service.TourOperatorPriceService;
using Service.TransportationPriceService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UI.Filters;
using UI.Tools;

namespace UI.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [TypeFilter(typeof(AcFilter))]
    [TypeFilter(typeof(AuthFilter))]
    [Authorize(Roles = "Admin,User")]
    public class PackageController : Controller
    {
        private readonly IPackageService packageService;
        private readonly IRegionService regionService;
        private readonly IHotelPriceService hotelPriceService;
        private readonly IFlightPriceService flightPriceService;
        private readonly ITransportationPriceService transportationPriceService;
        private readonly ITourOperatorPriceService tourOperatorPriceService;
        private readonly IRentACarPriceService rentACarPriceService;
        private readonly ITourGuidePriceService tourGuidePriceService;
        GoBackUrl goBackUrl;
        public PackageController(IPackageService packageService, IRegionService regionService, IHttpContextAccessor httpContextAccessor, IHotelPriceService hotelPriceService, IFlightPriceService flightPriceService, ITransportationPriceService transportationPriceService, ITourOperatorPriceService tourOperatorPriceService, IRentACarPriceService rentACarPriceService, ITourGuidePriceService tourGuidePriceService)
        {
            this.packageService = packageService;
            this.regionService = regionService;
            this.hotelPriceService = hotelPriceService;
            this.flightPriceService = flightPriceService;
            this.transportationPriceService = transportationPriceService;
            this.tourOperatorPriceService = tourOperatorPriceService;
            this.rentACarPriceService = rentACarPriceService;
            this.tourGuidePriceService = tourGuidePriceService;
            goBackUrl = new GoBackUrl(httpContextAccessor);
        }


        public IActionResult PackageList()
        {               
            return View(packageService.GetActivePackages());
        }

        [HttpPost]
        public IActionResult PackageList(Period period, string year)
        {            
            return View(packageService.GetPackagesByWhereClause(period,year));
        }

        public IActionResult Details(int id)
        {
            Package package = packageService.GetPackageById(id);
            if (package != null)
            {
                return View(package);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return RedirectToAction("PackageList");
            }
        }

        public IActionResult Create()
        {
            List<Region> regions = regionService.GetActiveRegions().ToList();
            ViewBag.Regions = new SelectList(regions, "Id", "RegionName");
            TempData["referer"] = goBackUrl.GetBackUrl();
            TempData.Keep("referer");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Package package)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    if (packageService.IfPackageNameExist(package.PackageName))
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), $"PackageName: {package.PackageName} has been already created.");
                        return View();
                    }
                    else
                    {
                        string[] result = packageService.CreatePackage(package).Split(":");
                        string type = result[0];
                        var message = result[1];
                        if (type == NotificationType.exception.ToString())
                            return RedirectToAction("Index", "Error", message);
                        else
                        {
                            TempData["Notification"] = Notification.Message(type, message);
                            return RedirectToAction("PackageList");
                        }
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }
            }
            else
            {
                List<Region> regions = regionService.GetActiveRegions().ToList();
                ViewBag.Regions = new SelectList(regions, "Id", "RegionName");
                TempData.Keep("referer");
                return View(package);
            }

        }

        public IActionResult Delete(int id)
        {
            Package packageDeleted = packageService.GetPackageById(id);
            if (packageDeleted != null)
            {
                try
                {
                    string[] result = packageService.DeletePackage(packageDeleted.Id).Split(":");
                    string type = result[0];
                    var message = result[1];
                    if (type == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", message);
                    else
                        TempData["Notification"] = Notification.Message(type, message);
                    return RedirectToAction("PackageList");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found!");
                return View("PackageList");
            }       
        }
      
        #region Hotel 

        public IActionResult ChooseHotel(int id)
        {
            Package package = packageService.GetPackageById(id);
            TempData["referer"] = goBackUrl.GetBackUrl();
            TempData["packageId"] = id;
            if (package != null)
            {
                IEnumerable<HotelPrice> hotelPrices = hotelPriceService.GetHotelPrices().Where(x => x.Hotel.Region == package.Region && x.Period == package.Period && x.Status == Convert.ToBoolean(Status.Active));
                return View(hotelPrices);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return RedirectToAction("PackageList");
            }
        }

        public IActionResult AddHotelPriceToPackage(int id)
        {

            try
            {
                HotelPrice hotelPrice = hotelPriceService.GetHotelPriceById(id);
                if (hotelPrice != null)
                {
                    int packageId = (int)TempData["packageId"];
                    string[] result = packageService.AddHotelPriceToPackage(id, packageId).Split(":");
                    string type = result[0];
                    var message = result[1];
                    if (type == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", message);
                    else
                    {
                        TempData["Notification"] = Notification.Message(type, message);
                        return RedirectToAction("Details", new { id = packageId });
                    }
                }
                else
                {
                    TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                    return RedirectToAction("PackageList");
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex);
            }
        }

        public IActionResult RemoveHotelPriceFromPackage(int hotelPriceId, int packageId)
        {
            try
            {
                HotelPrice hotelPrice = hotelPriceService.GetHotelPriceById(hotelPriceId);
                if (hotelPrice != null)
                {
                    Package package = packageService.GetPackageById(packageId);
                    package.HotelPriceId = null;
                    string[] result = packageService.UpdatePackage(package).Split(":");
                    string type = result[0];
                    var message = result[1];
                    if (type == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", message);
                    else
                    {
                        TempData["Notification"] = Notification.Message(type, message);
                        return RedirectToAction("Details", new { id = packageId });
                    }
                }
                else
                {
                    TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                    return RedirectToAction("PackageList");
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex);
            }
        }

        #endregion

        #region Flight
        public IActionResult ChooseFlight(int id)
        {
            Package package = packageService.GetPackageById(id);
            TempData["referer"] = goBackUrl.GetBackUrl();
            TempData["packageId"] = id;
            if (package != null)
            {
                IEnumerable<FlightPrice> flightPrices = packageService.GetUnAssignedFlightPricesForPackage(id);
                return View(flightPrices);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return RedirectToAction("PackageList");
            }
        }

        public IActionResult AddFlightPriceToPackage(int id)
        {

            try
            {
                FlightPrice flightPrice = flightPriceService.GetFlightPriceById(id);
                if (flightPrice != null)
                {
                    int packageId = (int)TempData["packageId"];
                    string[] result = packageService.AddFlightPriceToPackage(id, packageId).Split(":");
                    string type = result[0];
                    var message = result[1];
                    if (type == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", message);
                    else
                    {
                        TempData["Notification"] = Notification.Message(type, message);
                        return RedirectToAction("Details", new { id = packageId });
                    }
                }
                else
                {
                    TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                    return RedirectToAction("PackageList");
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex);
            }
        }

        public IActionResult RemoveFlightPriceFromPackage(int flightPriceId, int packageId)
        {
            try
            {
                FlightPrice flightPrice = flightPriceService.GetFlightPriceById(flightPriceId);
                if (flightPrice != null)
                {
                    Package package = packageService.GetPackageById(packageId);
                    package.FlightPrices.Remove(flightPrice);
                    TempData["Notification"] = Notification.Message(NotificationType.success.ToString(), "Transaction has been complated successfully.");
                    return RedirectToAction("Details", new { id = packageId });
                }
                else
                {
                    TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                    return RedirectToAction("PackageList");
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex);
            }
        }

        #endregion

        #region Transportation

        public IActionResult ChooseTransportation(int id)
        {
            Package package = packageService.GetPackageById(id);
            TempData["referer"] = goBackUrl.GetBackUrl();
            TempData["packageId"] = id;
            if (package != null)
            {
                IEnumerable<TransportationPrice> transportationPrices = packageService.GetUnAssignedTransportationPricesForPackage(id);
                return View(transportationPrices);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return RedirectToAction("PackageList");
            }
        }

        public IActionResult AddTransportationPriceToPackage(int id)
        {

            try
            {
                TransportationPrice transportationPrice = transportationPriceService.GetTransportationPriceById(id);
                if (transportationPrice != null)
                {
                    int packageId = (int)TempData["packageId"];
                    string[] result = packageService.AddTransportationPriceToPackage(id, packageId).Split(":");
                    string type = result[0];
                    var message = result[1];
                    if (type == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", message);
                    else
                    {
                        TempData["Notification"] = Notification.Message(type, message);
                        return RedirectToAction("Details", new { id = packageId });
                    }
                }
                else
                {
                    TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                    return RedirectToAction("PackageList");
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex);
            }
        }

        public IActionResult RemoveTransportationPriceFromPackage(int transportationPriceId, int packageId)
        {
            try
            {
                TransportationPrice transportationPrice = transportationPriceService.GetTransportationPriceById(transportationPriceId);
                if (transportationPrice != null)
                {
                    Package package = packageService.GetPackageById(packageId);
                    package.TransportationPrices.Remove(transportationPrice);
                    TempData["Notification"] = Notification.Message(NotificationType.success.ToString(), "Transaction has been complated successfully.");
                    return RedirectToAction("Details", new { id = packageId });
                }
                else
                {
                    TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                    return RedirectToAction("PackageList");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex);
            }
        }

        #endregion

        #region Tour

        public IActionResult ChooseTour(int id)
        {
            Package package = packageService.GetPackageById(id);
            TempData["referer"] = goBackUrl.GetBackUrl();
            TempData["packageId"] = id;
            TempData["startDate"] = package.CheckInDate;
            TempData["endDate"] = package.CheckOutDate;
            TempData.Keep("referer");
            if (package != null)
            {
                IEnumerable<TourOperatorPrice> transportationPrices = packageService.GetUnAssignedTourOperatorPricesForPackage(id);
                return View(transportationPrices);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return RedirectToAction("PackageList");
            }
        }

        [HttpPost]
        public IActionResult ChooseTour(DateTime date, int priceId)
        {

            try
            {
                TourOperatorPrice tourOperatorPrice = tourOperatorPriceService.GetTourOperatorPriceById(priceId);
                if (tourOperatorPrice != null)
                {
                    int packageId = (int)TempData["packageId"];
                    string[] result = packageService.AddTourOperatorPriceToPackage(priceId, packageId, date).Split(":");
                    string type = result[0];
                    var message = result[1];
                    if (type == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", message);
                    else
                    {
                        TempData["Notification"] = Notification.Message(type, message);
                        return RedirectToAction("Details", new { id = packageId });
                    }
                }
                else
                {
                    TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                    return RedirectToAction("PackageList");
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex);
            }
        }

        public IActionResult RemoveTourOperatorPriceFromPackage(int tourOperatorPriceId, int packageId)
        {
            try
            {
                TourOperatorPrice tourOperatorPrice = tourOperatorPriceService.GetTourOperatorPriceById(tourOperatorPriceId);
                if (tourOperatorPrice != null)
                {
                    string[] result = packageService.RemoveTourOperatorPriceFromPackage(tourOperatorPriceId, packageId).Split(":");
                    string type = result[0];
                    var message = result[1];
                    if (type == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", message);
                    else
                    {
                        TempData["Notification"] = Notification.Message(type, message);
                        return RedirectToAction("Details", new { id = packageId });
                    }
                }
                else
                {
                    TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                    return RedirectToAction("PackageList");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex);
            }
        }


        #endregion

        #region RentACar
             

        public IActionResult RemoveRentACarPriceFromPackage(int rentACarPriceId, int packageId)
        {
            try
            {
                RentACarPrice rentACarPrice = rentACarPriceService.GetRentACarPriceById(rentACarPriceId);
                if (rentACarPrice != null)
                {
                    Package package = packageService.GetPackageById(packageId);
                    package.RentACarPriceId = null;
                    package.RentACarStartDate = null;
                    package.RentACarEndDate = null;
                    TempData["Notification"] = Notification.Message(NotificationType.success.ToString(), "Transaction has been complated successfully.");
                    return RedirectToAction("Details", new { id = packageId });
                }
                else
                {
                    TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                    return RedirectToAction("PackageList");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex);
            }
        }

        #endregion

        #region TourGuide
     
        public IActionResult RemoveTourGuidePriceFromPackage(int tourGuidePriceId, int packageId)
        {
            try
            {
                TourGuidePrice tourGuidePrice = tourGuidePriceService.GetTourGuidePriceById(tourGuidePriceId);
                if (tourGuidePrice != null)
                {
                    Package package = packageService.GetPackageById(packageId);
                    package.TourGuidePriceId = null;
                    package.TourGuideRentDay = null;
                    TempData["Notification"] = Notification.Message(NotificationType.success.ToString(), "Transaction has been complated successfully.");
                    return RedirectToAction("Details", new { id = packageId });
                }
                else
                {
                    TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                    return RedirectToAction("PackageList");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex);
            }
        }
        #endregion
    }
}
