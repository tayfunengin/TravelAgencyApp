using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.FlightCompanyService;
using Service.FlightPriceService;
using Service.HotelPriceService;
using Service.HotelService;
using Service.RentACarCompanyService;
using Service.RentACarPriceService;
using Service.TourGuidePriceService;
using Service.TourGuideService;
using Service.TourOperatorPriceService;
using Service.TourOperatorService;
using Service.TransportationCompanyService;
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
  
    public class PriceController : Controller
    {
        private readonly IHotelPriceService hotelPriceService;
        private readonly IHotelService hotelService;
        private readonly ITourOperatorPriceService tourOperatorPriceService;
        private readonly ITourOperatorService tourOperatorService;
        private readonly IRentACarPriceService rentACarPriceService;
        private readonly IRentACarCompanyService rentACarCompanyService;
        private readonly IFlightPriceService flightPriceService;
        private readonly IFlightCompanyService flightCompanyService;
        private readonly ITransportationPriceService transportationPriceService;
        private readonly ITransportationCompanyService transportationCompanyService;
        private readonly ITourGuideService tourGuideService;
        private readonly ITourGuidePriceService tourGuidePriceService;
        GoBackUrl goBackUrl;

        public PriceController(IHotelPriceService hotelPriceService, IHotelService hotelService, ITourOperatorPriceService tourOperatorPriceService, ITourOperatorService tourOperatorService, IRentACarPriceService rentACarPriceService, IRentACarCompanyService rentACarCompanyService, IFlightPriceService flightPriceService, IFlightCompanyService flightCompanyService, ITransportationPriceService transportationPriceService, ITransportationCompanyService transportationCompanyService, ITourGuideService tourGuideService, ITourGuidePriceService tourGuidePriceService, IHttpContextAccessor httpContextAccessor)
        {
            this.hotelPriceService = hotelPriceService;
            this.hotelService = hotelService;
            this.tourOperatorPriceService = tourOperatorPriceService;
            this.tourOperatorService = tourOperatorService;
            this.rentACarPriceService = rentACarPriceService;
            this.rentACarCompanyService = rentACarCompanyService;
            this.flightPriceService = flightPriceService;
            this.flightCompanyService = flightCompanyService;
            this.transportationPriceService = transportationPriceService;
            this.transportationCompanyService = transportationCompanyService;
            this.tourGuideService = tourGuideService;
            this.tourGuidePriceService = tourGuidePriceService;
            goBackUrl = new GoBackUrl(httpContextAccessor);
        }

        #region Hotel Prices

        [Authorize(Roles = "Admin,User,Sales,Accounting,SalesAgent")]   
        public IActionResult HotelPriceList()
        {
            return View(this.hotelPriceService.GetHotelPrices());
        }

        [Authorize(Roles = "Admin,User")]
        public IActionResult CreateHotelPrice(int hotelId)
        {
            try
            {
                Hotel hotel = this.hotelService.GetHotelById(hotelId);
                if (hotel != null)
                {
                    TempData["accomodationTypes"] = new SelectList(hotel.AccomodationTypes, "Id", "Description");
                    TempData["roomLocations"] = new SelectList(hotel.RoomLocations, "Id", "Description");
                    TempData["roomTypes"] = new SelectList(hotel.RoomTypes, "Id", "Description");
                    TempData["referer"] = goBackUrl.GetBackUrl();
                    TempData["hotelId"] = hotel.Id;
                    TempData.Keep("referer");
                    return View();
                }
                else
                {
                    TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                    return RedirectToAction("HotelDetails", "Seller", new { id = hotelId });
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex);
            }

        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateHotelPrice(HotelPrice hotelPrice)
        {
            int hotelId = (int)TempData["hotelId"];
            if (ModelState.IsValid)
            {
                try
                {
                    if (hotelPrice != null)
                    {
                        string[] result = hotelPriceService.CreateHotelPrice(hotelPrice, hotelId).Split(":");
                        string type = result[0];
                        var message = result[1];
                        if (type == NotificationType.exception.ToString())
                            return RedirectToAction("Index", "Error", message);
                        else
                        {
                            TempData["Notification"] = Notification.Message(type, message);
                            return RedirectToAction("HotelDetails", "Seller", new { id = hotelPrice.HotelId });
                        }
                    }
                    else
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                        return RedirectToAction("HotelList", "Seller");
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }

            }
            else
            {
                Hotel hotel = hotelService.GetHotelById(hotelId);
                TempData["accomodationTypes"] = new SelectList(hotel.AccomodationTypes, "Id", "Description");
                TempData["roomLocations"] = new SelectList(hotel.RoomLocations, "Id", "Description");
                TempData["roomTypes"] = new SelectList(hotel.RoomTypes, "Id", "Description");
                TempData.Keep("referer");
                TempData.Keep("hotelId");
                return View();
            }
        }

        [Authorize(Roles = "Admin,User")]
        public IActionResult EditHotelPrice(int id)
        {
            HotelPrice hotelPrice = this.hotelPriceService.GetHotelPriceById(id);
            if (hotelPrice != null)
            {
                TempData["accomodationTypes"] = new SelectList(hotelPrice.Hotel.AccomodationTypes, "Id", "Description");
                TempData["roomLocations"] = new SelectList(hotelPrice.Hotel.RoomLocations, "Id", "Description");
                TempData["roomTypes"] = new SelectList(hotelPrice.Hotel.RoomTypes, "Id", "Description");
                return View(hotelPrice);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return RedirectToAction("HotelList", "Seller");
            }
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public IActionResult EditHotelPrice(HotelPrice hotelPrice)
        {
            HotelPrice hotelPriceUpdated = this.hotelPriceService.GetHotelPriceById(hotelPrice.Id);
            if (ModelState.IsValid)
            {
                try
                {
                    hotelPriceUpdated.PurchasePrice = hotelPrice.PurchasePrice;
                    hotelPriceUpdated.SalePrice = hotelPrice.SalePrice;
                    hotelPriceUpdated.Status = hotelPrice.Status;
                    string[] resultUpdate = hotelPriceService.UpdateHotelPrice(hotelPriceUpdated).Split(":");
                    string typeUpdate = resultUpdate[0];
                    var messageUpdate = resultUpdate[1];
                    if (typeUpdate == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", messageUpdate);
                    else
                        TempData["Notification"] = Notification.Message(typeUpdate, messageUpdate);
                    return RedirectToAction("HotelDetails", "Seller", new { id = hotelPrice.HotelId });

                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }
            }
            else
            {
                TempData["accomodationTypes"] = new SelectList(hotelPriceUpdated.Hotel.AccomodationTypes, "Id", "Description");
                TempData["roomLocations"] = new SelectList(hotelPriceUpdated.Hotel.RoomLocations, "Id", "Description");
                TempData["roomTypes"] = new SelectList(hotelPriceUpdated.Hotel.RoomTypes, "Id", "Description");
                return View(hotelPrice);
            }
        }

        [Authorize(Roles = "Admin,User")]
        public IActionResult DeleteHotelPrice(int id)
        {
            HotelPrice hotelPriceDeleted = hotelPriceService.GetHotelPriceById(id);
            if (hotelPriceDeleted != null)
            {
                try
                {
                    string[] result = hotelPriceService.DeleteHotelPrice(hotelPriceDeleted.Id).Split(":");
                    string type = result[0];
                    var message = result[1];
                    if (type == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", message);
                    else
                        TempData["Notification"] = Notification.Message(type, message);
                    return RedirectToAction("HotelDetails", "Seller", new { id = hotelPriceDeleted.HotelId });
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return RedirectToAction("HotelDetails", "Seller", new { id = hotelPriceDeleted.HotelId });
            }
        }


        #endregion

        #region Tour Operator Price

        [Authorize(Roles = "Admin,User,Sales,Accounting,SalesAgent")]
        public IActionResult TourOperatorPriceList()
        {
            return View(this.tourOperatorPriceService.GetTourOperatorPrices());
        }

        [Authorize(Roles = "Admin,User")]
        public IActionResult CreateTourOperatorPrice(int operatorId)
        {
            try
            {
                TourOperator tourOperator = this.tourOperatorService.GetTourOperatorById(operatorId);
                if (tourOperator != null)
                {
                    TempData["tours"] = new SelectList(tourOperator.Tours, "Id", "TourName");
                    TempData["referer"] = goBackUrl.GetBackUrl();
                    TempData["operatorId"] = operatorId;
                    TempData.Keep("referer");
                    return View();
                }
                else
                {
                    TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                    return RedirectToAction("TourOperatorList", "Seller");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex);
            }

        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTourOperatorPrice(TourOperatorPrice tourOperatorPrice)
        {
            int operatorId = (int)TempData["operatorId"];
            if (ModelState.IsValid)
            {
                try
                {
                    if (tourOperatorPrice != null)
                    {
                        string[] result = tourOperatorPriceService.CreateTourOperatorPrice(tourOperatorPrice, operatorId).Split(":");
                        string type = result[0];
                        var message = result[1];
                        if (type == NotificationType.exception.ToString())
                            return RedirectToAction("Index", "Error", message);
                        else
                        {
                            TempData["Notification"] = Notification.Message(type, message);
                            return RedirectToAction("TourOperatorDetails", "Seller", new { id = tourOperatorPrice.TourOperatorId });
                        }
                    }
                    else
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                        return RedirectToAction("TourOperatorList", "Seller");
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }

            }
            else
            {
                TourOperator tourOperator = tourOperatorService.GetTourOperatorById(operatorId);
                TempData["tours"] = new SelectList(tourOperator.Tours, "Id", "TourName");
                TempData.Keep("referer");
                TempData.Keep("operatorId");
                return View();
            }
        }

        [Authorize(Roles = "Admin,User")]
        public IActionResult EditTourOperatorPrice(int id)
        {
            TourOperatorPrice tourOperatorPrice = this.tourOperatorPriceService.GetTourOperatorPriceById(id);
            if (tourOperatorPrice != null)
            {
                TempData["tours"] = new SelectList(tourOperatorPrice.TourOperator.Tours, "Id", "TourName");              
                return View(tourOperatorPrice);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return RedirectToAction("TourOperatorList", "Seller");
            }
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public IActionResult EditTourOperatorPrice(TourOperatorPrice tourOperatorPrice)
        {
            TourOperatorPrice tourOperatorPriceUpdated = this.tourOperatorPriceService.GetTourOperatorPriceById(tourOperatorPrice.Id);
            if (ModelState.IsValid)
            {
                try
                {
                    tourOperatorPriceUpdated.PurchasePrice = tourOperatorPrice.PurchasePrice;
                    tourOperatorPriceUpdated.SalePrice = tourOperatorPrice.SalePrice;
                    tourOperatorPriceUpdated.Status = tourOperatorPrice.Status;
                    string[] resultUpdate = tourOperatorPriceService.UpdateTourOperatorPrice(tourOperatorPriceUpdated).Split(":");
                    string typeUpdate = resultUpdate[0];
                    var messageUpdate = resultUpdate[1];
                    if (typeUpdate == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", messageUpdate);
                    else
                    {
                        TempData["Notification"] = Notification.Message(typeUpdate, messageUpdate);
                        return RedirectToAction("TourOperatorDetails", "Seller", new { id = tourOperatorPriceUpdated.TourOperatorId });
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }
            }
            else
            {
                TempData["tours"] = new SelectList(tourOperatorPriceUpdated.TourOperator.Tours, "Id", "TourName");            
                return View(tourOperatorPrice);
            }
        }

        [Authorize(Roles = "Admin,User")]
        public IActionResult DeleteTourOperatorPrice(int id)
        {
            TourOperatorPrice tourOperatorPriceDeleted = tourOperatorPriceService.GetTourOperatorPriceById(id);
            if (tourOperatorPriceDeleted != null)
            {
                try
                {
                    string[] result = tourOperatorPriceService.DeleteTourOperatorPrice(tourOperatorPriceDeleted.Id).Split(":");
                    string type = result[0];
                    var message = result[1];
                    if (type == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", message);
                    else
                        TempData["Notification"] = Notification.Message(type, message);
                    return RedirectToAction("TourOperatorDetails", "Seller", new { id = tourOperatorPriceDeleted.TourOperatorId });
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return RedirectToAction("TourOperatorDetails", "Seller", new { id = tourOperatorPriceDeleted.TourOperatorId });
            }
        }


        #endregion

        #region Rent A Car Company Price

        [Authorize(Roles = "Admin,User,Sales,Accounting,SalesAgent")]
        public IActionResult RentACarPriceList()
        {
            return View(this.rentACarPriceService.GetActiveRentACarPrices());
        }

        [Authorize(Roles = "Admin,User")]
        public IActionResult CreateRentACarPrice(int companyId)
        {
            try
            {
                RentACarCompany rentACarCompany = rentACarCompanyService.GetRentACarCompanyById(companyId);
                if (rentACarCompany != null)
                {
                    TempData["cars"] = new SelectList(rentACarCompany.Cars, "Id", "Model");
                    TempData["referer"] = goBackUrl.GetBackUrl();
                    TempData["companyId"] = companyId;
                    TempData.Keep("referer");
                    return View();
                }
                else
                {
                    TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                    return RedirectToAction("RentACarCompanyList", "Seller");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex);
            }

        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public IActionResult CreateRentACarPrice(RentACarPrice rentACarPrice)
        {
            int companyId = (int)TempData["companyId"];
            if (ModelState.IsValid)
            {
                try
                {
                    if (rentACarPrice != null)
                    {
                        string[] result = rentACarPriceService.CreateRentACarPrice(rentACarPrice, companyId).Split(":");
                        string type = result[0];
                        var message = result[1];
                        if (type == NotificationType.exception.ToString())
                            return RedirectToAction("Index", "Error", message);
                        else
                        {
                            TempData["Notification"] = Notification.Message(type, message);
                            return RedirectToAction("RentACarCompanyDetails", "Seller", new { id = rentACarPrice.RentACarCompanyId });
                        }
                    }
                    else
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                        return RedirectToAction("RentACarCompanyList", "Seller");
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }

            }
            else
            {
                RentACarCompany rentACarCompany = rentACarCompanyService.GetRentACarCompanyById(companyId);
                TempData["cars"] = new SelectList(rentACarCompany.Cars, "Id", "Model");
                TempData.Keep("referer");
                TempData.Keep("companyId");
                return View();
            }

        }

        [Authorize(Roles = "Admin,User")]
        public IActionResult EditRentACarPrice(int id)
        {
            RentACarPrice rentACarPrice = rentACarPriceService.GetRentACarPriceById(id);
            if (rentACarPrice != null)
            {
                TempData["cars"] = new SelectList(rentACarPrice.RentACarCompany.Cars, "Id", "Model");           
                return View(rentACarPrice);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return RedirectToAction("RentACarCompanyList", "Seller");
            }
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public IActionResult EditRentACarPrice(RentACarPrice rentACarPrice)
        {
            RentACarPrice rentACarPriceUpdated = rentACarPriceService.GetRentACarPriceById(rentACarPrice.Id);
            if (ModelState.IsValid)
            {
                try
                {
                    rentACarPriceUpdated.PurchasePrice = rentACarPrice.PurchasePrice;
                    rentACarPriceUpdated.SalePrice = rentACarPrice.SalePrice;
                    rentACarPriceUpdated.Status = rentACarPrice.Status;
                    string[] resultUpdate = rentACarPriceService.UpdateRentACarPrice(rentACarPriceUpdated).Split(":");
                    string typeUpdate = resultUpdate[0];
                    var messageUpdate = resultUpdate[1];
                    if (typeUpdate == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", messageUpdate);
                    else
                    {
                        TempData["Notification"] = Notification.Message(typeUpdate, messageUpdate);
                        return RedirectToAction("RentACarCompanyDetails", "Seller", new { id = rentACarPriceUpdated.RentACarCompanyId });
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }
            }
            else
            {
                TempData["cars"] = new SelectList(rentACarPriceUpdated.RentACarCompany.Cars, "Id", "Model");             
                return View(rentACarPrice);
            }
        }

        [Authorize(Roles = "Admin,User")]
        public IActionResult DeleteRentAcarPrice(int id)
        {
            RentACarPrice rentACarPriceDeleted = rentACarPriceService.GetRentACarPriceById(id);
            if (rentACarPriceDeleted != null)
            {
                try
                {
                    string[] result = rentACarPriceService.DeleteRentACarPrice(rentACarPriceDeleted.Id).Split(":");
                    string type = result[0];
                    var message = result[1];
                    if (type == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", message);
                    else
                        TempData["Notification"] = Notification.Message(type, message);
                    return RedirectToAction("RentACarCompanyDetails", "Seller", new { id = rentACarPriceDeleted.RentACarCompanyId });
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return RedirectToAction("RentACarCompanyDetails", "Seller", new { id = rentACarPriceDeleted.RentACarCompanyId });
            }
        }


        #endregion

        #region Flight Prices

        [Authorize(Roles = "Admin,User,Sales,Accounting,SalesAgent")]
        public IActionResult FlightPriceList()
        {
            return View(this.flightPriceService.GetActiveFlightPrices());
        }

        [Authorize(Roles = "Admin,User")]
        public IActionResult CreateFlightPrice(int companyId)
        {
            try
            {
                FlightCompany flightCompany = flightCompanyService.GetFlightCompanyById(companyId);
                if (flightCompany != null)
                {
                    TempData["routes"] = new SelectList(flightCompany.Routes, "Id", "Description");
                    TempData["planes"] = new SelectList(flightCompany.Planes, "Id", "Model");
                    TempData["companyId"] = companyId;
                    TempData.Keep("referer");
                    return View();
                }
                else
                {
                    TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                    return RedirectToAction("FlightCompanyList", "Seller");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex);
            }

        }
        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public IActionResult CreateFlightPrice(FlightPrice flightPrice)
        {
            int companyId = (int)TempData["companyId"];
            if (ModelState.IsValid)
            {
                try
                {
                    if (flightPrice != null)
                    {
                        string[] result = flightPriceService.CreateFlightPrice(flightPrice, companyId).Split(":");
                        string type = result[0];
                        var message = result[1];
                        if (type == NotificationType.exception.ToString())
                            return RedirectToAction("Index", "Error", message);
                        else
                        {
                            TempData["Notification"] = Notification.Message(type, message);
                            return RedirectToAction("FlightCompanyDetails", "Seller", new { id = flightPrice.FlightCompanyId });
                        }
                    }
                    else
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                        return RedirectToAction("FlightCompanyList", "Seller");
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }
            }
            else
            {
                FlightCompany flightCompany = flightCompanyService.GetFlightCompanyById(companyId);
                TempData["routes"] = new SelectList(flightCompany.Routes, "Id", "Description");
                TempData["planes"] = new SelectList(flightCompany.Planes, "Id", "Model");
                TempData.Keep("referer");
                TempData.Keep("companyId");
                return View();
            }
        }
        [Authorize(Roles = "Admin,User")]
        public IActionResult EditFlightPrice(int id)
        {
            FlightPrice flightPrice = flightPriceService.GetFlightPriceById(id);
            if (flightPrice != null)
            {
                TempData["routes"] = new SelectList(flightPrice.FlightCompany.Routes, "Id", "Description");
                TempData["planes"] = new SelectList(flightPrice.FlightCompany.Planes, "Id", "Model");
                return View(flightPrice);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return RedirectToAction("FlightCompanyList", "Seller");
            }
        }
        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public IActionResult EditFlightPrice(FlightPrice flightPrice)
        {
            FlightPrice flightPriceUpdated = flightPriceService.GetFlightPriceById(flightPrice.Id);
            if (ModelState.IsValid)
            {
                try
                {
                    flightPriceUpdated.PurchasePrice = flightPrice.PurchasePrice;
                    flightPriceUpdated.SalePrice = flightPrice.SalePrice;
                    flightPriceUpdated.Status = flightPrice.Status;
                    string[] resultUpdate = flightPriceService.UpdateFlightPrice(flightPriceUpdated).Split(":");
                    string typeUpdate = resultUpdate[0];
                    var messageUpdate = resultUpdate[1];
                    if (typeUpdate == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", messageUpdate);
                    else
                    {
                        TempData["Notification"] = Notification.Message(typeUpdate, messageUpdate);
                        return RedirectToAction("FlightCompanyDetails", "Seller", new { id = flightPriceUpdated.FlightCompanyId });
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }
            }
            else
            {
                TempData["routes"] = new SelectList(flightPriceUpdated.FlightCompany.Routes, "Id", "Description");
                TempData["planes"] = new SelectList(flightPrice.FlightCompany.Planes, "Id", "Model");
                return View(flightPrice);
            }
        }
        [Authorize(Roles = "Admin,User")]
        public IActionResult DeleteFlightPrice(int id)
        {
            FlightPrice flightPriceDeleted = flightPriceService.GetFlightPriceById(id);
            if (flightPriceDeleted != null)
            {
                try
                {
                    string[] result = flightPriceService.DeleteFlightPrice(flightPriceDeleted.Id).Split(":");
                    string type = result[0];
                    var message = result[1];
                    if (type == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", message);
                    else
                        TempData["Notification"] = Notification.Message(type, message);
                    return RedirectToAction("FlightCompanyDetails", "Seller", new { id = flightPriceDeleted.FlightCompanyId });
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return RedirectToAction("FlightCompanyList", "Seller");
            }
        }
        #endregion

        #region Transportation Price

        [Authorize(Roles = "Admin,User,Sales,Accounting,SalesAgent")]
        public IActionResult TransportationPriceList()
        {
            return View(this.transportationPriceService.GetActiveTransportationPrices());
        }
        [Authorize(Roles = "Admin,User")]
        public IActionResult CreateTransportationPrice(int companyId)
        {
            try
            {
                TransportationCompany transportationCompany = transportationCompanyService.GetTransportationCompanyById(companyId);
                if (transportationCompany != null)
                {
                    TempData["routes"] = new SelectList(transportationCompany.Routes, "Id", "Description");
                    TempData["companyId"] = companyId;
                    TempData.Keep("referer");
                    return View();
                }
                else
                {
                    TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                    return RedirectToAction("TransportationCompanyList", "Seller");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex);
            }

        }
        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public IActionResult CreateTransportationPrice(TransportationPrice transportationPrice)
        {
            int companyId = (int)TempData["companyId"];
            if (ModelState.IsValid)
            {
                try
                {
                    if (transportationPrice != null)
                    {
                        string[] result = transportationPriceService.CreateTransportationPrice(transportationPrice, companyId).Split(":");
                        string type = result[0];
                        var message = result[1];
                        if (type == NotificationType.exception.ToString())
                            return RedirectToAction("Index", "Error", message);
                        else
                        {
                            TempData["Notification"] = Notification.Message(type, message);
                            return RedirectToAction("TransportationCompanyDetails", "Seller", new { id = transportationPrice.TransportationCompanyId });
                        }
                    }
                    else
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                        return RedirectToAction("TransportationCompanyList", "Seller");
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }
            }
            else
            {
                TransportationCompany transportationCompany = transportationCompanyService.GetTransportationCompanyById(companyId);
                TempData["routes"] = new SelectList(transportationCompany.Routes, "Id", "Description");
                TempData.Keep("referer");
                TempData.Keep("companyId");
                return View();
            }
        }
        [Authorize(Roles = "Admin,User")]
        public IActionResult EditTransportationPrice(int id)
        {
            TransportationPrice transportationPrice = transportationPriceService.GetTransportationPriceById(id);
            if (transportationPrice != null)
            {
                TempData["routes"] = new SelectList(transportationPrice.TransportationCompany.Routes, "Id", "Description");               
                return View(transportationPrice);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return RedirectToAction("TransportationCompanyList", "Seller");
            }
        }
        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public IActionResult EditTransportationPrice(TransportationPrice transportationPrice)
        {
            TransportationPrice transportationPriceUpdated = transportationPriceService.GetTransportationPriceById(transportationPrice.Id);
            if (ModelState.IsValid)
            {
                try
                {
                    transportationPriceUpdated.PurchasePrice = transportationPrice.PurchasePrice;
                    transportationPriceUpdated.SalePrice = transportationPrice.SalePrice;
                    transportationPriceUpdated.Status = transportationPrice.Status;
                    string[] resultUpdate = transportationPriceService.UpdateTransportationPrice(transportationPriceUpdated).Split(":");
                    string typeUpdate = resultUpdate[0];
                    var messageUpdate = resultUpdate[1];
                    if (typeUpdate == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", messageUpdate);
                    else
                    {
                        TempData["Notification"] = Notification.Message(typeUpdate, messageUpdate);
                        return RedirectToAction("TransportationCompanyDetails", "Seller", new { id = transportationPriceUpdated.TransportationCompanyId });
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }
            }
            else
            {
                TempData["routes"] = new SelectList(transportationPriceUpdated.TransportationCompany.Routes, "Id", "Description");         
                return View(transportationPrice);
            }
        }
        [Authorize(Roles = "Admin,User")]
        public IActionResult DeleteTransportationPrice(int id)
        {
            TransportationPrice transportationPriceDeleted = transportationPriceService.GetTransportationPriceById(id);
            if (transportationPriceDeleted != null)
            {
                try
                {
                    string[] result = transportationPriceService.DeleteTransportationPrice(transportationPriceDeleted.Id).Split(":");
                    string type = result[0];
                    var message = result[1];
                    if (type == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", message);
                    else
                        TempData["Notification"] = Notification.Message(type, message);
                    return RedirectToAction("TransportationCompanyDetails", "Seller", new { id = transportationPriceDeleted.TransportationCompanyId });
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return RedirectToAction("TransportationCompanyList", "Seller");
            }
        }


        #endregion

        #region TourGuidePrice
        [Authorize(Roles = "Admin,User,Sales,Accounting,SalesAgent")]
        public IActionResult TourGuidePriceList()
        {
            return View(this.tourGuidePriceService.GetActiveTourGuidePrices());
        }
        [Authorize(Roles = "Admin,User")]
        public IActionResult CreateTourGuidePrice(int tourGuideId)
        {
            try
            {
                TourGuidePrice tourGuidePrice = new TourGuidePrice();              
                TempData["referer"] = goBackUrl.GetBackUrl();
                TempData["guideId"] = tourGuideId;
                TempData.Keep("referer");
                return View(tourGuidePrice);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex);
            }

        }
        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public IActionResult CreateTourGuidePrice(TourGuidePrice tourGuidePrice)
        {
            int guideId = (int)TempData["guideId"];
            if (ModelState.IsValid)
            {
                try
                {
                    if (tourGuidePrice != null)
                    {
                        tourGuidePrice.TourGuideId = guideId;
                        string[] result = tourGuidePriceService.CreateTourGuidePrice(tourGuidePrice).Split(":");
                        string type = result[0];
                        var message = result[1];
                        if (type == NotificationType.exception.ToString())
                            return RedirectToAction("Index", "Error", message);
                        else
                        {
                            TempData["Notification"] = Notification.Message(type, message);
                            return RedirectToAction("TourGuideDetails", "Seller", new { id = tourGuidePrice.TourGuideId });
                        }
                    }
                    else
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                        return RedirectToAction("TourGuideList", "Seller");
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }
            }
            else
            {              
                TempData.Keep("referer");
                TempData.Keep("guideId");
                return View();
            }
        }
        [Authorize(Roles = "Admin,User")]
        public IActionResult EditTourGuidePrice(int id)
        {
            TourGuidePrice tourGuidePrice = tourGuidePriceService.GetTourGuidePriceById(id);
            if (tourGuidePrice != null)
            {                             
                return View(tourGuidePrice);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return RedirectToAction("TourGuideList", "Seller");
            }
        }
        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public IActionResult EditTourGuidePrice(TourGuidePrice tourGuidePrice)
        {
            TourGuidePrice tourGuidePriceUpdated = tourGuidePriceService.GetTourGuidePriceById(tourGuidePrice.Id);
            if (ModelState.IsValid)
            {
                try
                {
                    tourGuidePriceUpdated.PurchasePrice = tourGuidePrice.PurchasePrice;
                    tourGuidePriceUpdated.SalePrice = tourGuidePrice.SalePrice;
                    tourGuidePriceUpdated.Status = tourGuidePrice.Status;
                    string[] resultUpdate = tourGuidePriceService.UpdateTourGuidePrice(tourGuidePriceUpdated).Split(":");
                    string typeUpdate = resultUpdate[0];
                    var messageUpdate = resultUpdate[1];
                    if (typeUpdate == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", messageUpdate);
                    else
                    {
                        TempData["Notification"] = Notification.Message(typeUpdate, messageUpdate);
                        return RedirectToAction("TourGuideDetails", "Seller", new { id = tourGuidePriceUpdated.TourGuideId });
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }
            }
            else
            {             
                return View(tourGuidePrice);
            }
        }
        [Authorize(Roles = "Admin,User")]
        public IActionResult DeleteTourGuidePrice(int id)
        {
            TourGuidePrice tourGuidePriceDeleted = tourGuidePriceService.GetTourGuidePriceById(id);
            if (tourGuidePriceDeleted != null)
            {
                try
                {
                    string[] result = tourGuidePriceService.DeleteTourGuidePrice(tourGuidePriceDeleted.Id).Split(":");
                    string type = result[0];
                    var message = result[1];
                    if (type == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", message);
                    else
                        TempData["Notification"] = Notification.Message(type, message);
                    return RedirectToAction("TourGuideDetails", "Seller", new { id = tourGuidePriceDeleted.TourGuideId });
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return RedirectToAction("TourGuideList", "Seller");
            }
        }

        #endregion
    }
}
