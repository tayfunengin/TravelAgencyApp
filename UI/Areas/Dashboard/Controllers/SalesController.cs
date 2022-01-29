using Common;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.PackageService;
using Service.RegionService;
using Service.RentACarPriceService;
using Service.SalesService;
using Service.TourGuidePriceService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using UI.Filters;
using UI.Tools;

namespace UI.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [TypeFilter(typeof(AcFilter))]
    [TypeFilter(typeof(AuthFilter))]
  
    public class SalesController : Controller
    {
        private readonly IPackageService packageService;
        private readonly IRegionService regionService;
        private readonly IRentACarPriceService rentACarPriceService;
        private readonly ITourGuidePriceService tourGuidePriceService;
        private readonly ISalesService salesService;
        GoBackUrl goBackUrl;

        public SalesController(IPackageService packageService, IRegionService regionService, IRentACarPriceService rentACarPriceService, ITourGuidePriceService tourGuidePriceService, IHttpContextAccessor httpContextAccessor, ISalesService salesService)
        {
            this.packageService = packageService;
            this.regionService = regionService;
            this.rentACarPriceService = rentACarPriceService;
            this.tourGuidePriceService = tourGuidePriceService;
            this.salesService = salesService;
            goBackUrl = new GoBackUrl(httpContextAccessor);
        }

        [Authorize(Roles = "Admin,SalesAgent")]
        public IActionResult PackageList()
        {    
            List<Region> regions = regionService.GetActiveRegions().ToList();
            TempData["regions"] = new SelectList(regions, "Id", "RegionName");
            return View(packageService.GetActivePackages().Where(x=>x.Year== DateTime.Now.Year.ToString()));
        }
        [Authorize(Roles = "Admin,SalesAgent")]
        [HttpPost]
        public IActionResult PackageList(Period period, int regionId)
        {
            List<Region> regions = regionService.GetActiveRegions().ToList();
            TempData["regions"] = new SelectList(regions, "Id", "RegionName");
            return View(packageService.GetByPeriodAndRegion(period, regionId));
        }

        [Authorize(Roles = "Admin,SalesAgent")]
        public IActionResult Details(int id)
        {
            Package package = packageService.GetPackageById(id);
            if (package != null)
            {
                decimal euroRate = GetCurrencyRate.GetByCurrency("EUR");
                double totalLira =Convert.ToDouble(  package.Total * euroRate );
                TempData["totalTl"] = totalLira;
                return View(package);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return RedirectToAction("PackageList");
            }
        }

        [Authorize(Roles = "Admin,SalesAgent")]
        public IActionResult AddRentACar(int id)
        {

            Package package = packageService.GetPackageById(id);
            TempData["referer"] = goBackUrl.GetBackUrl();
            TempData["packageId"] = id;
            TempData["startDate"] = package.CheckInDate;
            TempData["endDate"] = package.CheckOutDate;
            TempData.Keep("referer");
            List<RentACarPrice> rentACarPrices = rentACarPriceService.GetActiveRentACarPrices().Where(x => x.Year == package.Year && x.Period == package.Period).ToList();

            return View(rentACarPrices);
        }
        [Authorize(Roles = "Admin,SalesAgent")]
        [HttpPost]
        public IActionResult AddRentACar(DateTime begindate, DateTime endDate, int priceId)
        {
            try
            {
                RentACarPrice rentACarPrice = rentACarPriceService.GetRentACarPriceById(priceId);
                if (rentACarPrice != null)
                {
                    int packageId = (int)TempData["packageId"];
                    string[] result = packageService.AddRentACarPriceToPackage(priceId, begindate, endDate, packageId).Split(":");
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
        [Authorize(Roles = "Admin,SalesAgent")]
        public IActionResult RemoveRentalCar(int rentACarPriceId, int packageId)
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
        [Authorize(Roles = "Admin,SalesAgent")]
        public IActionResult AddTourGuide(int id)
        {

            Package package = packageService.GetPackageById(id);
            TempData["referer"] = goBackUrl.GetBackUrl();
            TempData["packageId"] = id;
            TempData["startDate"] = package.CheckInDate;
            TempData["endDate"] = package.CheckOutDate;
            if (package != null)
            {
                List<TourGuidePrice> prices = tourGuidePriceService.GetActiveTourGuidePrices().Where(x => x.Year == package.Year && x.Period == package.Period && x.TourGuide.Region == package.Region).ToList();
                return View(prices);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return RedirectToAction("PackageList");
            }
        }
        [Authorize(Roles = "Admin,SalesAgent")]
        [HttpPost]
        public IActionResult AddTourGuide(DateTime date, int priceId)
        {
            try
            {
                TourGuidePrice tourGuidePrice = tourGuidePriceService.GetTourGuidePriceById(priceId);
                if (tourGuidePrice != null)
                {
                    int packageId = (int)TempData["packageId"];
                    string[] result = packageService.AddTourGuidePriceToPackage(priceId, date, packageId).Split(":");
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
        [Authorize(Roles = "Admin,SalesAgent")]
        public IActionResult RemoveTourGuide(int tourGuidePriceId, int packageId)
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
        [Authorize(Roles = "Admin,SalesAgent")]
        public IActionResult Purchase(int id)
        {
            Package package = packageService.GetPackageById(id);
            if (package !=null)
            {
                TempData["referer"] = goBackUrl.GetBackUrl();
                TempData.Keep("referer");
                TempData["package"] = package;

                decimal euroRate = GetCurrencyRate.GetByCurrency("EUR");
                double totalLira = Convert.ToDouble(package.Total * euroRate);
                TempData["totalTl"] = totalLira;
       
                return View();
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return RedirectToAction("PackageList");
            }
            
        }
        [Authorize(Roles = "Admin,SalesAgent")]
        [HttpPost]
        public IActionResult Purchase(Customer customer, int packageId , string name, string surname, string email)
        {
            Package package = packageService.GetPackageById(packageId);
            if (ModelState.IsValid)
            {
                Customer newCustomer = new Customer()
                {
                    Name = name,
                    Surname = surname,
                    Email = email
                };
                string[] result = this.salesService.CreateSales(package, newCustomer).Split(":");
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
            else
            {
                decimal euroRate = GetCurrencyRate.GetByCurrency("EUR");
                double totalLira = Convert.ToDouble(package.Total * euroRate);
                TempData["totalTl"] = totalLira;
                TempData.Keep("referer");
                TempData["package"] = package;
                return View();
            }        
        }

        [Authorize(Roles = "Admin,SalesAgent,Sales")]
        public IActionResult SalesList()
        {
            List<Region> regions = regionService.GetRegions().ToList();
            TempData["regions"] = new SelectList(regions, "Id", "RegionName");
            return View(salesService.GetAllSales());
        }
        [Authorize(Roles = "Admin,Sales")]
        [HttpPost]
        public IActionResult SalesList(DateTime salesDate, int regionId)
        {
            List<Region> regions = regionService.GetRegions().ToList();
            TempData["regions"] = new SelectList(regions, "Id", "RegionName");
            return View(salesService.GetByRegionAndSalesDate(salesDate, regionId));
        }
        [Authorize(Roles = "Admin,Sales")]
        public IActionResult SalesDetails(int id)
        {
            Sales sales = salesService.GetById(id);
            if (sales != null)
            {
                TempData["referer"] = goBackUrl.GetBackUrl();
                TempData.Keep("referer");
                return View(sales);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return RedirectToAction("PackageList");
            }
        }


    }
}
