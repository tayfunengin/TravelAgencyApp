using Common;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.AccomodationTypeService;
using Service.CarService;
using Service.FlightCompanyService;
using Service.HotelService;
using Service.PhotographyService;
using Service.PlaneService;
using Service.RegionService;
using Service.RentACarCompanyService;
using Service.RoomLocationService;
using Service.RoomTypeService;
using Service.RouteService;
using Service.TourGuideService;
using Service.TourOperatorService;
using Service.TourService;
using Service.TransportationCompanyService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using UI.Filters;
using UI.Tools;

namespace UI.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [TypeFilter(typeof(AcFilter))]
    [TypeFilter(typeof(AuthFilter))]
    [Authorize(Roles = "Admin,User")]
    public class SellerController : Controller
    {
        private readonly ITourOperatorService tourOperatorService;
        private readonly ITourService tourService;
        private readonly IFlightCompanyService flightCompanyService;
        private readonly IRegionService regionService;
        private readonly IPlaneService planeService;
        private readonly IHotelService hotelService;
        private readonly IAccomodationTypeService accomodationTypeService;
        private readonly IRoomTypeService roomTypeService;
        private readonly IRoomLocationService roomLocationService;
        private readonly IRentACarCompanyService rentACarCompanyService;
        private readonly IPhotographyService photographyService;
        private readonly ICarService carService;
        private readonly ITransportationCompanyService transportationCompanyService;
        private readonly IRouteService routeService;
        private readonly ITourGuideService tourGuideService;
        private readonly IWebHostEnvironment webHostEnvironment;
        GoBackUrl goBackUrl;

        public SellerController(ITourOperatorService tourOperatorService, ITourService tourService, IFlightCompanyService flightCompanyService, IRegionService regionService, IPlaneService planeService, IWebHostEnvironment webHostEnvironment, IHotelService hotelService, IAccomodationTypeService accomodationTypeService, IRoomTypeService roomTypeService, IRoomLocationService roomLocationService, IRentACarCompanyService rentACarCompanyService, IPhotographyService photographyService, ICarService carService, ITransportationCompanyService transportationCompanyService, IRouteService routeService, ITourGuideService tourGuideService, IHttpContextAccessor httpContextAccessor)
        {
            this.tourOperatorService = tourOperatorService;
            this.tourService = tourService;
            this.flightCompanyService = flightCompanyService;
            this.regionService = regionService;
            this.planeService = planeService;
            this.hotelService = hotelService;
            this.accomodationTypeService = accomodationTypeService;
            this.roomTypeService = roomTypeService;
            this.roomLocationService = roomLocationService;
            this.rentACarCompanyService = rentACarCompanyService;
            this.photographyService = photographyService;
            this.carService = carService;
            this.transportationCompanyService = transportationCompanyService;
            this.routeService = routeService;
            this.tourGuideService = tourGuideService;
            this.webHostEnvironment = webHostEnvironment;
            goBackUrl = new GoBackUrl(httpContextAccessor);
        }

        #region Flight Company
        public IActionResult FlightCompanyList()
        {
            return View(flightCompanyService.GetFlightCompanies());
        }
        public IActionResult FlightCompanyDetails(int id)
        {
            TempData["referer"] = goBackUrl.GetBackUrl();
            FlightCompany flightCompany = flightCompanyService.GetFlightCompanyById(id);
            if (flightCompany != null)
            {
                return View(flightCompany);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return RedirectToAction("FlightCompanyList");
            }
        }

        public IActionResult CreateFlightCompany()
        {
            TempData["referer"] = goBackUrl.GetBackUrl();
            TempData.Keep("referer");

            List<Region> regions = regionService.GetActiveRegions().ToList();
            List<Plane> planes = planeService.GetActivePlanes().ToList();
            List<Route> routes = routeService.GetActiveRoutes().ToList();
            TempData["regions"] = new SelectList(regions, "Id", "RegionName");
            TempData["planes"] = new SelectList(planes, "Id", "Model");
            TempData["routes"] = new SelectList(routes, "Id", "Description");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateFlightCompany(FlightCompany flightCompany, int[] regionIds, int[] planeIds, int[] routeIds)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (flightCompanyService.IfFlightCompanyCodeExist(flightCompany.FlightCompanyCode))
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), $"FlightCompanyCode: {flightCompany.FlightCompanyCode} has been already created.");
                        return View();
                    }
                    else
                    {
                        string[] result = flightCompanyService.CreateFlightCompany(flightCompany, regionIds, planeIds,routeIds).Split(":");
                        string type = result[0];
                        var message = result[1];
                        if (type == NotificationType.exception.ToString())
                            return RedirectToAction("Index", "Error", message);
                        else
                            TempData["Notification"] = Notification.Message(type, message);
                        return RedirectToAction("FlightCompanyList");
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
                List<Plane> planes = planeService.GetActivePlanes().ToList();
                List<Route> routes = routeService.GetActiveRoutes().ToList();
                TempData["regions"] = new SelectList(regions, "Id", "RegionName");
                TempData["planes"] = new SelectList(planes, "Id", "Model");
                TempData["routes"] = new SelectList(routes, "Id", "Description");
                TempData.Keep("referer");
                return View();
            }
        }
        public IActionResult EditFlightCompany(int id)
        {
            TempData["referer"] = goBackUrl.GetBackUrl();
            TempData.Keep("referer");
            FlightCompany flightCompanyUpdated = flightCompanyService.GetFlightCompanyById(id);
            if (flightCompanyUpdated != null)
            {
                return View(flightCompanyUpdated);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return View("FlightCompanyList");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditFlightCompany(FlightCompany flightCompany)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (flightCompanyService.IfFlightCompanyCodeExist(flightCompany.FlightCompanyCode) && flightCompanyService.GetFlightCompanyById(flightCompany.Id).FlightCompanyCode != flightCompany.FlightCompanyCode)
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), $"FlightCompanyCode: {flightCompany.FlightCompanyCode} has been already created.");
                        return View();
                    }
                    else
                    {
                        FlightCompany flightCompanyUpdated = flightCompanyService.GetFlightCompanyById(flightCompany.Id);
                        flightCompanyUpdated.FlightCompanyCode = flightCompany.FlightCompanyCode;
                        flightCompanyUpdated.FlightCompanyName = flightCompany.FlightCompanyName;
                        flightCompanyUpdated.Address = flightCompany.Address;
                        flightCompanyUpdated.Status = flightCompany.Status;

                        string[] result = flightCompanyService.UpdateFlightCompany(flightCompanyUpdated).Split(":");
                        string type = result[0];
                        var message = result[1];
                        if (type == NotificationType.exception.ToString())
                            return RedirectToAction("Index", "Error", message);
                        else
                            TempData["Notification"] = Notification.Message(type, message);
                        return RedirectToAction("FlightCompanyList");
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
                return View();
            }
        }

        public IActionResult DeleteFlightCompany(int id)
        {
            FlightCompany flightCompanyDeleted = flightCompanyService.GetFlightCompanyById(id);
            if (flightCompanyDeleted != null)
            {
                try
                {
                    string[] result = flightCompanyService.DeleteFlightCompany(flightCompanyDeleted.Id).Split(":");
                    string type = result[0];
                    var message = result[1];
                    if (type == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", message);
                    else
                        TempData["Notification"] = Notification.Message(type, message);
                    return RedirectToAction("FlightCompanyList");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return View("FlightCompanyList");
            }
        }
        #region Plane İşlemleri
        public IActionResult AddPlaneToFlightCompany(int id)
        {
            List<Plane> unAssignedPlanes = flightCompanyService.GetUnAssignedPlanes(id);
            if (unAssignedPlanes.Count == 0)
            {
                TempData["Notification"] = Notification.Message(NotificationType.warning.ToString(), "All planes are already assigned");
                return RedirectToAction("FlightCompanyDetails", new { id = id });
            }
            else
            {
                TempData["referer"] = goBackUrl.GetBackUrl();
                TempData["planes"] = new SelectList(unAssignedPlanes, "Id", "Model");
            }
            return View(flightCompanyService.GetFlightCompanyById(id));
        }

        [HttpPost]
        public IActionResult AddPlaneToFlightCompany(int[] selectedIds, FlightCompany company)
        {
            FlightCompany flightCompany = flightCompanyService.GetFlightCompanyById(company.Id);
            foreach (var planeId in selectedIds)
            {
                Plane plane = planeService.GetPlaneById(planeId);
                flightCompany.Planes.Add(plane);
            }
            return RedirectToAction("FlightCompanyDetails", new { id = flightCompany.Id });
        }
        public IActionResult RemovePlaneFromFlightCompany(int planeId, int flightCompanyId)
        {
            try
            {
                FlightCompany flightCompany = flightCompanyService.GetFlightCompanyById(flightCompanyId);
                Plane plane = planeService.GetPlaneById(planeId);
                flightCompany.Planes.Remove(plane);
                TempData["Notification"] = Notification.Message(NotificationType.success.ToString(), "Transaction has been completed successfully.");
                return RedirectToAction("FlightCompanyDetails", new { id = flightCompanyId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex);
            }

        }

        #endregion

        #region Region İşlemleri
        public IActionResult AddRegionToFlightCompany(int id)
        {
            List<Region> unAssignedRegions = flightCompanyService.GetUnAssignedRegions(id);
            if (unAssignedRegions.Count == 0)
            {
                TempData["Notification"] = Notification.Message(NotificationType.warning.ToString(), "Tüm bölgeler zaten atanmış.");
                return RedirectToAction("FlightCompanyDetails", new { id = id });
            }
            else
            {
                TempData["referer"] = goBackUrl.GetBackUrl();
                TempData["regions"] = new SelectList(unAssignedRegions, "Id", "RegionName");
            }
            return View(flightCompanyService.GetFlightCompanyById(id));
        }

        [HttpPost]
        public IActionResult AddRegionToFlightCompany(int[] selectedIds, FlightCompany company)
        {
            try
            {
                FlightCompany flightCompany = flightCompanyService.GetFlightCompanyById(company.Id);
                foreach (var regionId in selectedIds)
                {
                    Region region = regionService.GetRegionById(regionId);
                    flightCompany.Regions.Add(region);
                }
                TempData["Notification"] = Notification.Message(NotificationType.success.ToString(), "Transaction has been completed successfully.");
                return RedirectToAction("FlightCompanyDetails", new { id = flightCompany.Id });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex);
            }

        }
        public IActionResult RemoveRegionFromFlightCompany(int regionId, int flightCompanyId)
        {
            try
            {
                FlightCompany flightCompany = flightCompanyService.GetFlightCompanyById(flightCompanyId);
                Region region = regionService.GetRegionById(regionId);
                flightCompany.Regions.Remove(region);
                TempData["Notification"] = Notification.Message(NotificationType.success.ToString(), "Transaction has been completed successfully.");
                return RedirectToAction("FlightCompanyDetails", new { id = flightCompanyId });
            }
            catch (Exception ex)
            {

                return RedirectToAction("Index", "Error", ex);
            }

        }

        #endregion

        #region Route İşlemleri
        public IActionResult AddRouteToFlightCompany(int id)
        {
            List<Route> unAssignedRoutes = flightCompanyService.GetUnAssignedRoutes(id);
            if (unAssignedRoutes.Count == 0)
            {
                TempData["Notification"] = Notification.Message(NotificationType.warning.ToString(), "All routes have been already assigned.");
                return RedirectToAction("FlightCompanyDetails", new { id = id });
            }
            else
            {
                TempData["referer"] = goBackUrl.GetBackUrl();
                TempData["routes"] = new SelectList(unAssignedRoutes, "Id", "Description");
            }
            return View(flightCompanyService.GetFlightCompanyById(id));
        }

        [HttpPost]
        public IActionResult AddRouteToFlightCompany(int[] selectedIds, FlightCompany company)
        {
            try
            {
                FlightCompany flightCompany = flightCompanyService.GetFlightCompanyById(company.Id);
                foreach (var routeId in selectedIds)
                {
                    Route route = routeService.GetRouteById(routeId);
                    flightCompany.Routes.Add(route);
                }
                TempData["Notification"] = Notification.Message(NotificationType.success.ToString(), "Transaction has been completed successfully.");
                return RedirectToAction("FlightCompanyDetails", new { id = flightCompany.Id });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex);
            }

        }
        public IActionResult RemoveRouteFromFlightCompany(int routeId, int flightCompanyId)
        {
            try
            {
                FlightCompany flightCompany = flightCompanyService.GetFlightCompanyById(flightCompanyId);
                Route route = routeService.GetRouteById(routeId);
                flightCompany.Routes.Remove(route);
                TempData["Notification"] = Notification.Message(NotificationType.success.ToString(), "Transaction has been completed successfully..");
                return RedirectToAction("FlightCompanyDetails", new { id = flightCompanyId });
            }
            catch (Exception ex)
            {

                return RedirectToAction("Index", "Error", ex);
            }

        }

        #endregion

        #endregion

        #region Hotel

        public IActionResult HotelList()
        {
            IEnumerable<Hotel> hotels = hotelService.GetHotels();
            return View(hotels);
        }

        public IActionResult HotelDetails(int id)
        {
            Hotel hotel = hotelService.GetHotelById(id);
            if (hotel != null)
            {
                return View(hotel);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return RedirectToAction("HotelList");
            }
        }

        public IActionResult CreateHotel()
        {
            TempData["referer"] = goBackUrl.GetBackUrl();
            TempData.Keep("referer");
            List<AccomodationType> accomodationTypes = accomodationTypeService.GetActiveAccomodationTypes().ToList();
            List<RoomType> roomTypes = roomTypeService.GetActiveRoomTypes().ToList();
            List<RoomLocation> roomLocations = roomLocationService.GetActiveRoomLocationTypes().ToList();
            List<Region> regions = regionService.GetActiveRegions().ToList();
            TempData["roomLocations"] = new SelectList(roomLocations, "Id", "Description");
            TempData["roomTypes"] = new SelectList(roomTypes, "Id", "Description");
            TempData["accomodationTypes"] = new SelectList(accomodationTypes, "Id", "Description");
            TempData["regions"] = new SelectList(regions, "Id", "RegionName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateHotel(Hotel hotel, IFormFile formFile, int[] accomodationTypeIds, int[] roomTypeIds, int[] roomLocationIds)
        {
            string webroot = webHostEnvironment.WebRootPath;
            if (formFile == null)
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Photo cannot be empty!.");
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        if (hotelService.IfHotelCodeExist(hotel.HotelCode))
                        {
                            TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), $"HotelCode: {hotel.HotelCode} has been already created.");
                            return View();
                        }
                        else
                        {
                            string[] photoResult = hotelService.UploadProfileImage(hotel, formFile, webroot).Split(':');
                            string photoType = photoResult[0];
                            var photoMessage = photoResult[1];
                            if (photoType == NotificationType.exception.ToString())
                            {
                                return RedirectToAction("Index", "Error", "Fotoğraf yüklenirken bir sorun oluştu!");
                            }

                            else if (photoType == NotificationType.error.ToString())
                            {
                                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), photoMessage);
                                return RedirectToAction("HotelList");
                            }
                            else
                            {
                                string[] result = hotelService.CreateHotel(hotel, accomodationTypeIds, roomLocationIds, roomTypeIds).Split(":");
                                string type = result[0];
                                var message = result[1];
                                if (type == NotificationType.exception.ToString())

                                    return RedirectToAction("Index", "Error", message);

                                else
                                {
                                    TempData["Notification"] = Notification.Message(type, message);
                                    return RedirectToAction("HotelList");
                                }
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
                    List<AccomodationType> accomodationTypes = accomodationTypeService.GetActiveAccomodationTypes().ToList();
                    List<RoomType> roomTypes = roomTypeService.GetActiveRoomTypes().ToList();
                    List<RoomLocation> roomLocations = roomLocationService.GetActiveRoomLocationTypes().ToList();
                    List<Region> regions = regionService.GetActiveRegions().ToList();
                    TempData["roomLocations"] = new SelectList(roomLocations, "Id", "Description");
                    TempData["roomTypes"] = new SelectList(roomTypes, "Id", "Description");
                    TempData["accomodationTypes"] = new SelectList(accomodationTypes, "Id", "Description");
                    TempData["regions"] = new SelectList(regions, "Id", "RegionName");
                    TempData.Keep("referer");
                    return View();
                }

            }

        }
        public IActionResult EditHotel(int id)
        {
            TempData["referer"] = goBackUrl.GetBackUrl();
            TempData.Keep("referer");
            List<Region> regions = regionService.GetActiveRegions().ToList();
            TempData["regions"] = new SelectList(regions, "Id", "RegionName");
            Hotel hotelUpdated = hotelService.GetHotelById(id);
            if (hotelUpdated != null)
            {
                return View(hotelUpdated);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return View("HotelList");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditHotel(Hotel hotel, IFormFile formFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (hotelService.IfHotelCodeExist(hotel.HotelCode) && hotelService.GetHotelById(hotel.Id).HotelCode != hotel.HotelCode)
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), $"HotelCode: {hotel.HotelCode} has been already created.");
                        return View();
                    }
                    else
                    {
                        Hotel hotelUpdated = hotelService.GetHotelById(hotel.Id);
                        hotelUpdated.HotelCode = hotel.HotelCode;
                        hotelUpdated.HotelName = hotel.HotelName;
                        hotelUpdated.Address = hotel.Address;
                        hotelUpdated.Status = hotel.Status;
                        hotelUpdated.RegionId = hotel.RegionId;
                        string webroot = webHostEnvironment.WebRootPath;
                        if (formFile != null)
                        {
                            string[] photoResult = hotelService.UploadProfileImage(hotelUpdated, formFile, webroot).Split(':');
                            string photoType = photoResult[0];
                            var photoMessage = photoResult[1];
                            if (photoType == NotificationType.exception.ToString())
                                return RedirectToAction("Index", "Error", "Fotoğraf yüklenirken bir sorun oluştu!");
                            else if (photoType == NotificationType.error.ToString())
                            {
                                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), photoMessage);
                                return RedirectToAction("HotelList");
                            }
                            else
                            {
                                string[] resultUpdate = hotelService.UpdateHotel(hotelUpdated).Split(":");
                                string typeUpdate = resultUpdate[0];
                                var messageUpdate = resultUpdate[1];
                                if (typeUpdate == NotificationType.exception.ToString())
                                    return RedirectToAction("Index", "Error", messageUpdate);
                                else
                                    TempData["Notification"] = Notification.Message(typeUpdate, messageUpdate);
                                return RedirectToAction("HotelList");

                            }
                        }
                        hotelUpdated.ProfilePhotoPath = hotelService.GetHotelById(hotel.Id).ProfilePhotoPath;
                        string[] result = hotelService.UpdateHotel(hotelUpdated).Split(":");
                        string type = result[0];
                        var message = result[1];
                        if (type == NotificationType.exception.ToString())
                            return RedirectToAction("Index", "Error", message);
                        else
                            TempData["Notification"] = Notification.Message(type, message);
                        return RedirectToAction("HotelList");
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
                List<Region> regions = regionService.GetActiveRegions().ToList();
                TempData["regions"] = new SelectList(regions, "Id", "RegionName");
                return View();
            }
        }

        public IActionResult DeleteHotel(int id)
        {
            Hotel hotelDeleted = hotelService.GetHotelById(id);
            if (hotelDeleted != null)
            {
                try
                {
                    string[] result = hotelService.DeleteHotel(hotelDeleted.Id).Split(":");
                    string type = result[0];
                    var message = result[1];
                    if (type == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", message);
                    else
                        TempData["Notification"] = Notification.Message(type, message);
                    return RedirectToAction("HotelList");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return View("HotelList");
            }
        }


        public IActionResult AddAccomodationTypeToHotel(int id)
        {
            List<AccomodationType> unAssignedAccomodationTypes = hotelService.GetUnAssignedAccomodationTypes(id);
            if (unAssignedAccomodationTypes.Count == 0)
            {
                TempData["Notification"] = Notification.Message(NotificationType.warning.ToString(), "All accommodation types are already assigned.");
                return RedirectToAction("HotelDetails", new { id = id });
            }
            else
            {
                TempData["referer"] = goBackUrl.GetBackUrl();
                TempData["accomodationTypes"] = new SelectList(unAssignedAccomodationTypes, "Id", "Description");
            }
            return View(hotelService.GetHotelById(id));
        }

        [HttpPost]
        public IActionResult AddAccomodationTypeToHotel(int[] selectedIds, Hotel hotel)
        {
            try
            {
                hotelService.AddAccomodationTypeToHotel(selectedIds, hotel.Id);
                TempData["Notification"] = Notification.Message(NotificationType.success.ToString(), "Transaction has been completed successfully.");
                return RedirectToAction("HotelDetails", new { id = hotel.Id });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex);
            }

        }

        public IActionResult AddRoomTypeToHotel(int id)
        {
            List<RoomType> unAssignedRoomTypes = hotelService.GetUnAssignedRoomTypes(id);
            if (unAssignedRoomTypes.Count == 0)
            {
                TempData["Notification"] = Notification.Message(NotificationType.warning.ToString(), "All room types are already assigned.");
                return RedirectToAction("HotelDetails", new { id = id });
            }
            else
            {
                TempData["referer"] = goBackUrl.GetBackUrl();
                TempData["roomTypes"] = new SelectList(unAssignedRoomTypes, "Id", "Description");
            }
            return View(hotelService.GetHotelById(id));
        }

        [HttpPost]
        public IActionResult AddRoomTypeToHotel(int[] selectedIds, Hotel hotel)
        {
            try
            {
                hotelService.AddRoomTypeToHotel(selectedIds, hotel.Id);
                TempData["Notification"] = Notification.Message(NotificationType.success.ToString(), "Transaction has been completed successfully.");
                return RedirectToAction("HotelDetails", new { id = hotel.Id });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex);
            }

        }

        public IActionResult AddRoomLocationToHotel(int id)
        {
            List<RoomLocation> unAssignedRoomLocations = hotelService.GetUnAssignedRoomLocations(id);
            if (unAssignedRoomLocations.Count == 0)
            {
                TempData["Notification"] = Notification.Message(NotificationType.warning.ToString(), "All room locations are already assigned.");
                return RedirectToAction("HotelDetails", new { id = id });
            }
            else
            {
                TempData["referer"] = goBackUrl.GetBackUrl();
                TempData["roomLocations"] = new SelectList(unAssignedRoomLocations, "Id", "Description");
            }
            return View(hotelService.GetHotelById(id));
        }

        [HttpPost]
        public IActionResult AddRoomLocationToHotel(int[] selectedIds, Hotel hotel)
        {
            try
            {
                hotelService.AddRoomLocationToHotel(selectedIds, hotel.Id);
                TempData["Notification"] = Notification.Message(NotificationType.success.ToString(), "Transaction has been completed successfully.");
                return RedirectToAction("HotelDetails", new { id = hotel.Id });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex);
            }

        }

        public IActionResult RemoveAccomodationTypeFromHotel(int accTypeId, int hotelId)
        {
            try
            {
                Hotel hotel = hotelService.GetHotelById(hotelId);
                AccomodationType accomodationType = accomodationTypeService.GetAccomodationTypeById(accTypeId);
                hotel.AccomodationTypes.Remove(accomodationType);
                TempData["Notification"] = Notification.Message(NotificationType.success.ToString(), "Transaction has been completed successfully.");
                return RedirectToAction("HotelDetails", new { id = hotelId });
            }
            catch (Exception ex)
            {

                return RedirectToAction("Index", "Error", ex);
            }

        }
        public IActionResult RemoveRoomTypeFromHotel(int roomTypeId, int hotelId)
        {
            try
            {
                Hotel hotel = hotelService.GetHotelById(hotelId);
                RoomType roomType = roomTypeService.GetRoomTypeById(roomTypeId);
                hotel.RoomTypes.Remove(roomType);
                TempData["Notification"] = Notification.Message(NotificationType.success.ToString(), "Transaction has been completed successfully.");
                return RedirectToAction("HotelDetails", new { id = hotelId });
            }
            catch (Exception ex)
            {

                return RedirectToAction("Index", "Error", ex);
            }

        }
        public IActionResult RemoveRoomLocationFromHotel(int roomLocationId, int hotelId)
        {
            try
            {
                Hotel hotel = hotelService.GetHotelById(hotelId);
                RoomLocation roomLocation = roomLocationService.GetRoomLocationById(roomLocationId);
                hotel.RoomLocations.Remove(roomLocation);
                TempData["Notification"] = Notification.Message(NotificationType.success.ToString(), "Transaction has been completed successfully.");
                return RedirectToAction("HotelDetails", new { id = hotelId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex);
            }

        }

        public IActionResult HotelGallery(int id)
        {            
            Hotel hotel = hotelService.GetHotelById(id);
            if (hotel == null)
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Hotel could not be found.");
                return RedirectToAction("HotelDetails", new { id = id });
            }
            else
            {
                return View(hotel);
            }
        }
        public IActionResult AddPhotoToHotel(int id)
        {
            TempData["referer"] = goBackUrl.GetBackUrl();
            TempData.Keep("referer");
            Hotel hotel = hotelService.GetHotelById(id);
            if (hotel == null)
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return RedirectToAction("HotelDetails", new { id = id });
            }
            else
            {
                TempData["hotelForPhoto"] = hotel;
                return View();
            }
        }

        [HttpPost]
        public IActionResult AddPhotoToHotel(Photography photo, IFormFile formFile, int id)
        {
            TempData["referer"] = goBackUrl.GetBackUrl();
            TempData.Keep("referer");
            Hotel hotel = hotelService.GetHotelById(id);
            if (formFile != null)
            {
                if (hotel == null)
                {
                    TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                    return RedirectToAction("HotelDetails", new { id = id });
                }
                else
                {
                    string webRoot = webHostEnvironment.WebRootPath;
                    string[] photoResult = hotelService.UploadImageForGallery(photo, formFile, id, webRoot).Split(':');
                    string type = photoResult[0];
                    var message = photoResult[1];
                    if (type == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", "There was a problem while uploading the photo!");
                    else if (type == NotificationType.error.ToString())
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), message);
                        return RedirectToAction("HotelDetails", new { id = hotel.Id });
                    }
                    else
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.success.ToString(), message);
                        TempData.Keep("referer");
                        return RedirectToAction("HotelGallery", new { id = hotel.Id });
                    }
                }
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Please select a photo.");
                TempData.Keep("referer");
                return RedirectToAction("HotelGallery", new { id = hotel.Id });
            }

        }

        public IActionResult DeletePhotoFromHotel(int id, int hotelId)
        {
            try
            {
                Hotel hotel = hotelService.GetHotelById(hotelId);
                Photography removePhoto = photographyService.GetPhotography(id);
                if (hotel != null && removePhoto != null)
                {
                    hotel.Photographies.Remove(removePhoto);
                    TempData["Notification"] = Notification.Message(NotificationType.success.ToString(), "The photo has been removed successfully.");
                    return RedirectToAction("HotelGallery", new { id = hotel.Id });
                }
                else
                {
                    TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Hotel/photo could not be found.");
                    return RedirectToAction("HotelGallery", new { id = hotel.Id });
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex); ;
            }
        }

        public IActionResult ArrangeAsProfilePhoto(int id, int hotelId)
        {
            try
            {
                Hotel hotel = hotelService.GetHotelById(hotelId);
                Photography profilePhoto = photographyService.GetPhotography(id);
                if (hotel != null && profilePhoto != null)
                {
                    hotel.ProfilePhotoPath = profilePhoto.Path;
                    string[] result = hotelService.UpdateHotel(hotel).Split(':');
                    string type = result[0];
                    var message = result[1];
                    if (type == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", message);
                    else
                        TempData["Notification"] = Notification.Message(type, "Profile photo has been updated successfully.");
                    return RedirectToAction("HotelList");
                }
                else
                {
                    TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Hotel/photo could not be found.");
                    return RedirectToAction("HotelGallery", new { id = hotel.Id });
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex); ;
            }
        }

        #endregion

        #region RentACarCompany

        public IActionResult RentACarCompanyList()
        {
            return View(rentACarCompanyService.GetRentACarCompanies().ToList());
        }

        public IActionResult CreateRentACarCompany()
        {
            TempData["referer"] = goBackUrl.GetBackUrl();
            TempData.Keep("referer");
            List<Car> cars = carService.GetCars().ToList();
            TempData["cars"] = new SelectList(cars, "Id", "Model");
            return View();
        }

        [HttpPost]
        public IActionResult CreateRentACarCompany(RentACarCompany rentACarCompany, int[] carIds)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (rentACarCompanyService.IfRentACarCompanyCodeExists(rentACarCompany.RentACarCompanyCode))
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), $"RentACarCompanyCode: {rentACarCompany.RentACarCompanyCode} has been already created.");
                        return View();
                    }
                    else
                    {
                        string[] result = rentACarCompanyService.CreateRentACarCompany(rentACarCompany, carIds).Split(":");
                        string type = result[0];
                        var message = result[1];
                        if (type == NotificationType.exception.ToString())
                            return RedirectToAction("Index", "Error", message);
                        else
                        {
                            TempData["Notification"] = Notification.Message(type, message);
                            return RedirectToAction("RentACarCompanyList");
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
                List<Car> cars = carService.GetCars().ToList();
                TempData["cars"] = new SelectList(cars, "Id", "Model");
                TempData.Keep("referer");
                return View();
            }
        }
        public IActionResult EditRentACarCompany(int id)
        {
            TempData["referer"] = goBackUrl.GetBackUrl();
            TempData.Keep("referer");
            RentACarCompany rentACarCompanyUpdated = rentACarCompanyService.GetRentACarCompanyById(id);
            if (rentACarCompanyUpdated != null)
            {
                return View(rentACarCompanyUpdated);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return View("RentACarCompanyList");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditRentACarCompany(RentACarCompany rentACarCompany)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (rentACarCompanyService.IfRentACarCompanyCodeExists(rentACarCompany.RentACarCompanyCode) && rentACarCompanyService.GetRentACarCompanyById(rentACarCompany.Id).RentACarCompanyCode != rentACarCompany.RentACarCompanyCode)
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), $"RentACarCompanyCode: {rentACarCompany.RentACarCompanyCode} has been already created.");
                        return View();
                    }
                    else
                    {
                        RentACarCompany updatedRentACarCompany = rentACarCompanyService.GetRentACarCompanyById(rentACarCompany.Id);
                        updatedRentACarCompany.RentACarCompanyCode = rentACarCompany.RentACarCompanyCode;
                        updatedRentACarCompany.RentACarCompanyName = rentACarCompany.RentACarCompanyName;
                        updatedRentACarCompany.Address = rentACarCompany.Address;
                        updatedRentACarCompany.Status = rentACarCompany.Status;

                        string[] result = rentACarCompanyService.UpdateRentACarCompany(updatedRentACarCompany).Split(":");
                        string type = result[0];
                        var message = result[1];
                        if (type == NotificationType.exception.ToString())
                            return RedirectToAction("Index", "Error", message);
                        else
                            TempData["Notification"] = Notification.Message(type, message);
                        return RedirectToAction("RentACarCompanyList");
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
                return View();
            }
        }
        public IActionResult DeleteRentACarCompany(int id)
        {
            RentACarCompany rentACarCompanyDeleted = rentACarCompanyService.GetRentACarCompanyById(id);
            if (rentACarCompanyDeleted != null)
            {
                try
                {
                    string[] result = rentACarCompanyService.DeleteRentACarCompany(rentACarCompanyDeleted.Id).Split(":");
                    string type = result[0];
                    var message = result[1];
                    if (type == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", message);
                    else
                        TempData["Notification"] = Notification.Message(type, message);
                    return RedirectToAction("RentACarCompanyList");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return View("RentACarCompanyList");
            }
        }

        public IActionResult RentACarCompanyDetails(int id)
        {           
            RentACarCompany rentACarCompany = rentACarCompanyService.GetRentACarCompanyById(id);
            if (rentACarCompany != null)
            {
                return View(rentACarCompany);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return RedirectToAction("RentACarCompanyList");
            }

        }       
        public IActionResult AddCarToRentACarCompany(int id)
        {
            List<Car> unAssignedCars = rentACarCompanyService.GetUnAssignedCars(id);
            if (unAssignedCars.Count == 0)
            {
                TempData["Notification"] = Notification.Message(NotificationType.warning.ToString(), "All cars are already assigned.");                
                return RedirectToAction("RentACarCompanyDetails", new { id = id });
            }
            else
            {
                TempData["referer"] = goBackUrl.GetBackUrl();
                TempData["cars"] = new SelectList(unAssignedCars, "Id", "Model");
                return View(rentACarCompanyService.GetRentACarCompanyById(id));
            }          
        }

        [HttpPost]
        public IActionResult AddCarToRentACarCompany(int[] selectedIds, RentACarCompany rentACarCompany)
        {
            try
            {
                rentACarCompanyService.AddCarsToRentACarCompany(selectedIds, rentACarCompany.Id);
                TempData["Notification"] = Notification.Message(NotificationType.success.ToString(), "Transaction has been completed successfully.");            
                return RedirectToAction("RentACarCompanyDetails", new { id = rentACarCompany.Id });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex);
            }
        }


        public IActionResult RemoveCarFromRentACarCompany(int carId, int rentACarCompanyId)
        {
            try
            {
                RentACarCompany rentACarCompany = rentACarCompanyService.GetRentACarCompanyById(rentACarCompanyId);
                Car car = carService.GetCarById(carId);
                rentACarCompany.Cars.Remove(car);
                TempData["Notification"] = Notification.Message(NotificationType.success.ToString(), "Transaction has been completed successfully.");
                TempData.Keep("referer");
                return RedirectToAction("RentACarCompanyDetails", new { id = rentACarCompanyId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex);
            }

        }


        #endregion

        #region TransportationCompany
        public IActionResult TransportationCompanyList()
        {
            return View(transportationCompanyService.GetTransportationCompanies());
        }

        public IActionResult CreateTransportationCompany()
        {
            TempData["referer"] = goBackUrl.GetBackUrl();
            TempData.Keep("referer");
            List<Route> routes = routeService.GetActiveRoutes().ToList();
            TempData["routes"] = new SelectList(routes, "Id", "Description");
            TempData["regions"] = new SelectList(regionService.GetActiveRegions(), "Id", "RegionName");
            return View();
        }

        [HttpPost]
        public IActionResult CreateTransportationCompany(TransportationCompany transportationCompany, int[] routeIds)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (transportationCompanyService.IfTransportationCodeExist(transportationCompany.TransportaionCompanyCode))
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), $"TransportaionCompanyCode: {transportationCompany.TransportaionCompanyCode} has been already created.");
                        return View();
                    }
                    else
                    {
                        string[] result = transportationCompanyService.CreateTransportationCompany(transportationCompany, routeIds).Split(":");
                        string type = result[0];
                        var message = result[1];
                        if (type == NotificationType.exception.ToString())
                            return RedirectToAction("Index", "Error", message);
                        else
                        {
                            TempData["Notification"] = Notification.Message(type, message);
                            return RedirectToAction("TransportationCompanyList");
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
                List<Route> routes = routeService.GetActiveRoutes().ToList();
                TempData["routess"] = new SelectList(routes, "Id", "Description");
                TempData.Keep("referer");
                return View();
            }
        }


        public IActionResult EditTransportationCompany(int id)
        {
            TempData["referer"] = goBackUrl.GetBackUrl();
            TempData.Keep("referer");
            TempData["regions"] = new SelectList(regionService.GetActiveRegions(), "Id", "RegionName");
            TransportationCompany transportationCompany = transportationCompanyService.GetTransportationCompanyById(id);
            if (transportationCompany != null)
            {
                return View(transportationCompany);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return View("TransportationCompanyList");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTransportationCompany(TransportationCompany transportationCompany)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (transportationCompanyService.IfTransportationCodeExist(transportationCompany.TransportaionCompanyCode) && transportationCompanyService.GetTransportationCompanyById(transportationCompany.Id).TransportaionCompanyCode != transportationCompany.TransportaionCompanyCode)
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), $"TransportaionCompanyCode: {transportationCompany.TransportaionCompanyCode} has been already created.");
                        return View();
                    }
                    else
                    {
                        TransportationCompany transportationCompanyUpdated = transportationCompanyService.GetTransportationCompanyById(transportationCompany.Id);
                        transportationCompanyUpdated.TransportaionCompanyCode = transportationCompany.TransportaionCompanyCode;
                        transportationCompanyUpdated.TransportationCompanyName = transportationCompany.TransportationCompanyName;
                        transportationCompanyUpdated.Address = transportationCompany.Address;
                        transportationCompanyUpdated.Status = transportationCompany.Status;

                        string[] result = transportationCompanyService.UpdateTransporationCompany(transportationCompanyUpdated).Split(":");
                        string type = result[0];
                        var message = result[1];
                        if (type == NotificationType.exception.ToString())
                            return RedirectToAction("Index", "Error", message);
                        else
                            TempData["Notification"] = Notification.Message(type, message);
                        return RedirectToAction("TransportationCompanyList");
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }
            }
            else
            {
                List<Route> routes = routeService.GetActiveRoutes().ToList();
                TempData["routes"] = new SelectList(routes, "Id", "Description");
                TempData.Keep("referer");
                return View();
            }
        }

        public IActionResult DeleteTransportationCompany(int id)
        {
            TransportationCompany transportationCompanyDeleted = transportationCompanyService.GetTransportationCompanyById(id);
            if (transportationCompanyDeleted != null)
            {
                try
                {
                    string[] result = transportationCompanyService.DeleteTransportationCompany(transportationCompanyDeleted.Id).Split(":");
                    string type = result[0];
                    var message = result[1];
                    if (type == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", message);
                    else
                        TempData["Notification"] = Notification.Message(type, message);
                    return RedirectToAction("TransportationCompanyList");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return View("TransportationCompanyList");
            }
        }

        public IActionResult TransportationCompanyDetails(int id)
        {
            TransportationCompany transportationCompany = transportationCompanyService.GetTransportationCompanyById(id);
            if (transportationCompany != null)
            {
                return View(transportationCompany);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return RedirectToAction("TransportationCompanyList");
            }

        }

        public IActionResult AddRouteToTransportationCompany(int id)
        {
            List<Route> unAssignedRoutes = transportationCompanyService.GetUnAssignedRoutes(id);
            if (unAssignedRoutes.Count == 0)
            {
                TempData["Notification"] = Notification.Message(NotificationType.warning.ToString(), "All routes are already assigned.");
                return RedirectToAction("TransportationCompanyDetails", new { id = id });
            }
            else
            {
                TempData["referer"] = goBackUrl.GetBackUrl();
                TempData.Keep("referer");
                TempData["routes"] = new SelectList(unAssignedRoutes, "Id", "Description");
                return View(transportationCompanyService.GetTransportationCompanyById(id));
            }
        }

        [HttpPost]
        public IActionResult AddRouteToTransportationCompany(int[] selectedIds, TransportationCompany transportationCompany)
        {
            try
            {
                transportationCompanyService.AddRoutesToTransportationCompany(selectedIds, transportationCompany.Id);
                TempData["Notification"] = Notification.Message(NotificationType.success.ToString(), "Transaction has been completed successfully.");
                return RedirectToAction("TransportationCompanyDetails", new { id = transportationCompany.Id });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex);
            }
        }

        public IActionResult RemoveRouteFromTransportationCompany(int routeId, int transportationCompanyId)
        {
            try
            {
                TransportationCompany transportationCompany = transportationCompanyService.GetTransportationCompanyById(transportationCompanyId);
                Route route = routeService.GetRouteById(routeId);
                transportationCompany.Routes.Remove(route);
                TempData["Notification"] = Notification.Message(NotificationType.success.ToString(), "Transaction has been completed successfully.");
                TempData.Keep("referer");
                return RedirectToAction("TransportationCompanyDetails", new { id = transportationCompanyId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex);
            }

        }

        #endregion

        #region TourGuide
        public IActionResult TourGuideList()
        {
            return View(tourGuideService.GetTourGuides());
        }

        public IActionResult TourGuideDetails(int id)
        {
            TempData["referer"] = goBackUrl.GetBackUrl();
            TourGuide tourGuide = tourGuideService.GetTourGuideById(id);
            if (tourGuide != null)
            {

                return View(tourGuide);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return RedirectToAction("TourGuideList");
            }

        }
        public IActionResult CreateTourGuide()
        {
            List<Region> regions = regionService.GetActiveRegions().ToList();
            TempData["regions"] = new SelectList(regions, "Id", "RegionName");
            TempData["referer"] = goBackUrl.GetBackUrl();
            TempData.Keep("referer");       
            return View();
        }

        [HttpPost]
        public IActionResult CreateTourGuide(TourGuide tourGuide)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (tourGuideService.IfTourGuideCodeExist(tourGuide.TourGuideCode))
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), $"TourGuideCode: {tourGuide.TourGuideCode} has been already created.");
                        return View();
                    }
                    else
                    {
                        string[] result = tourGuideService.CreateTourGuide(tourGuide).Split(":");
                        string type = result[0];
                        var message = result[1];
                        if (type == NotificationType.exception.ToString())
                            return RedirectToAction("Index", "Error", message);
                        else
                            TempData["Notification"] = Notification.Message(type, message);
                        return RedirectToAction("TourGuideList");
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
                TempData["regions"] = new SelectList(regions, "Id", "RegionName");
                TempData.Keep("referer");
                return View();
            }
        }

        public IActionResult EditTourGuide(int id)
        {
            List<Region> regions = regionService.GetActiveRegions().ToList();
            TempData["regions"] = new SelectList(regions, "Id", "RegionName");
            TempData["referer"] = goBackUrl.GetBackUrl();
            TempData.Keep("referer");
            TourGuide tourGuide = tourGuideService.GetTourGuideById(id);
            if (tourGuide != null)
            {
                return View(tourGuide);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return View("TourGuideList");
            }
        }

        [HttpPost]
        public IActionResult EditTourGuide(TourGuide tourGuide)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (tourGuideService.IfTourGuideCodeExist(tourGuide.TourGuideCode) && tourGuideService.GetTourGuideById(tourGuide.Id).TourGuideCode != tourGuide.TourGuideCode)
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), $"TourGuideCode: {tourGuide.TourGuideCode} has been already created.");
                        return View();
                    }
                    else
                    {
                        TourGuide tourGuideUpdated = tourGuideService.GetTourGuideById(tourGuide.Id);
                        tourGuideUpdated.TourGuideCode = tourGuide.TourGuideCode;
                        tourGuideUpdated.TourGuideName = tourGuide.TourGuideName;
                        tourGuideUpdated.TourGuideSurname = tourGuide.TourGuideSurname;
                        tourGuideUpdated.RegionId = tourGuide.RegionId;
                        tourGuideUpdated.Language = tourGuide.Language;
                        tourGuideUpdated.Address = tourGuide.Address;
                        tourGuideUpdated.Status = tourGuide.Status;
                        string[] result = tourGuideService.UpdateTourGuide(tourGuideUpdated).Split(":");
                        string type = result[0];
                        var message = result[1];
                        if (type == NotificationType.exception.ToString())
                            return RedirectToAction("Index", "Error", message);
                        else
                            TempData["Notification"] = Notification.Message(type, message);
                        return RedirectToAction("TourGuideList");
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
                TempData["regions"] = new SelectList(regions, "Id", "RegionName");
                TempData.Keep("referer");
                return View();
            }
        }

        public IActionResult DeleteTourGuide(int id)
        {
            TourGuide tourGuideDeleted = tourGuideService.GetTourGuideById(id);
            if (tourGuideDeleted != null)
            {
                try
                {
                    string[] result = tourGuideService.DeleteTourGuide(tourGuideDeleted.Id).Split(":");
                    string type = result[0];
                    var message = result[1];
                    if (type == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", message);
                    else
                        TempData["Notification"] = Notification.Message(type, message);
                    return RedirectToAction("TourGuideList");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return View("TourGuideList");
            }
        }

        #endregion

        #region TourOperator
        public IActionResult TourOperatorList()
        {
            return View(tourOperatorService.GetTourOperators());
        }
        public IActionResult TourOperatorDetails(int id)
        {
            TempData["referer"] = goBackUrl.GetBackUrl();
            TourOperator tourOperator = tourOperatorService.GetTourOperatorById(id);
            if (tourOperator != null)
            {

                return View(tourOperator);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return RedirectToAction("TourOperatorList");
            }

        }
        public IActionResult CreateTourOperator()
        {
            TempData["referer"] = goBackUrl.GetBackUrl();
            TempData.Keep("referer");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTourOperator(TourOperator tourOperator)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (tourOperatorService.IfTourOperatorCodeExist(tourOperator.TourOperatorCode))
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), $"TourOperatorCode: {tourOperator.TourOperatorCode} has been already created.");
                        return View();
                    }
                    else
                    {
                        string[] result = tourOperatorService.CreateTourOperator(tourOperator).Split(":");
                        string type = result[0];
                        var message = result[1];
                        if (type == NotificationType.exception.ToString())
                            return RedirectToAction("Index", "Error", message);
                        else
                            TempData["Notification"] = Notification.Message(type, message);
                        return RedirectToAction("TourOperatorList");
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
                return View();
            }
        }
        public IActionResult EditTourOperator(int id)
        {
            TempData["referer"] = goBackUrl.GetBackUrl();
            TempData.Keep("referer");
            TourOperator tourOperatorUpdated = tourOperatorService.GetTourOperatorById(id);
            if (tourOperatorUpdated != null)
            {
                return View(tourOperatorUpdated);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return View("TourOperatorList");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTourOperator(TourOperator tourOperator)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (tourOperatorService.IfTourOperatorCodeExist(tourOperator.TourOperatorCode) && tourOperatorService.GetTourOperatorById(tourOperator.Id).TourOperatorCode != tourOperator.TourOperatorCode)
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), $"TourOperatorCode: {tourOperator.TourOperatorCode} has been already created.");
                        return View();
                    }
                    else
                    {
                        TourOperator updatedTourOperator = tourOperatorService.GetTourOperatorById(tourOperator.Id);
                        updatedTourOperator.TourOperatorCode = tourOperator.TourOperatorCode;
                        updatedTourOperator.TourOperatorName = tourOperator.TourOperatorName;
                        updatedTourOperator.Address = tourOperator.Address;
                        updatedTourOperator.Status = tourOperator.Status;

                        string[] result = tourOperatorService.UpdateTourOperator(updatedTourOperator).Split(":");
                        string type = result[0];
                        var message = result[1];
                        if (type == NotificationType.exception.ToString())
                            return RedirectToAction("Index", "Error", message);
                        else
                            TempData["Notification"] = Notification.Message(type, message);
                        return RedirectToAction("TourOperatorList");
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
                return View();
            }
        }

        public IActionResult DeleteTourOperator(int id)
        {
            TourOperator tourOperatorDeleted = tourOperatorService.GetTourOperatorById(id);
            if (tourOperatorDeleted != null)
            {
                try
                {
                    string[] result = tourOperatorService.DeleteTourOperator(tourOperatorDeleted.Id).Split(":");
                    string type = result[0];
                    var message = result[1];
                    if (type == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", message);
                    else
                        TempData["Notification"] = Notification.Message(type, message);
                    return RedirectToAction("TourOperatorList");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return View("TourOperatorList");
            }
        }

        public IActionResult RemoveTourFromTourOperator(int tourId, int operatorId)
        {
            TourOperator tourOperator = tourOperatorService.GetTourOperatorById(operatorId);
            Tour tour = tourService.GetTourById(tourId);
            if (tourOperator != null && tour != null)
            {
                string[] result = tourOperatorService.RemoveTour(tourOperator, tour).Split(":");
                string type = result[0];
                var message = result[1];
                if (type == NotificationType.exception.ToString())
                    return RedirectToAction("Index", "Error", message);
                else
                    TempData["Notification"] = Notification.Message(type, message);
                return RedirectToAction("TourOperatorDetails", new { id = operatorId });
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return View("TourOperatorDetails", new { id = operatorId });
            }
        }

        #endregion

    }
}
