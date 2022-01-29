using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.Context;
using Service.AccomodationTypeService;
using Service.CarService;
using Service.PlaneService;
using Service.RegionService;
using Service.RoomLocationService;
using Service.RoomTypeService;
using Service.RouteService;
using Service.TourOperatorService;
using Service.TourService;
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
    [Authorize(Roles = "Admin, User")]
    public class DefinitionController : Controller
    {
        private readonly IAccomodationTypeService accomodationTypeService;
        private readonly ICarService carService;
        private readonly IPlaneService planeService;
        private readonly IRegionService regionService;
        private readonly IRoomLocationService roomLocationService;
        private readonly IRoomTypeService roomTypeService;
        private readonly ITourService tourService;
        private readonly ITourOperatorService tourOperatorService;
        private readonly IRouteService routeService;
        GoBackUrl goBackUrl;

        public DefinitionController(IAccomodationTypeService accomodationTypeService, ICarService carService, IPlaneService planeService, IRegionService regionService, IRoomLocationService roomLocationService, IRoomTypeService roomTypeService, ITourService tourService, ITourOperatorService tourOperatorService, IRouteService routeService, IHttpContextAccessor httpContextAccessor)
        {
            this.accomodationTypeService = accomodationTypeService;
            this.carService = carService;
            this.planeService = planeService;
            this.regionService = regionService;
            this.roomLocationService = roomLocationService;
            this.roomTypeService = roomTypeService;
            this.tourService = tourService;
            this.tourOperatorService = tourOperatorService;
            this.routeService = routeService;
            goBackUrl = new GoBackUrl(httpContextAccessor);
        }
        // GET: DefinitionController
        public ActionResult Index()
        {
            TempData["AccomodationTypeCount"] = accomodationTypeService.GetAccomodationTypes().Where(x => x.Status == Convert.ToBoolean(Status.Active)).Count();
            TempData["RoomLocationCount"] = roomLocationService.GetRoomLocationTypes().Where(x => x.Status == Convert.ToBoolean(Status.Active)).Count();
            TempData["RoomTypeCount"] = roomTypeService.GetRoomTypes().Where(x => x.Status == Convert.ToBoolean(Status.Active)).Count();
            TempData["RegionCount"] = regionService.GetRegions().Where(x => x.Status == Convert.ToBoolean(Status.Active)).Count();
            TempData["CarCount"] = carService.GetCars().Where(x => x.Status == Convert.ToBoolean(Status.Active)).Count();
            TempData["PlaneCount"] = planeService.GetPlanes().Where(x => x.Status == Convert.ToBoolean(Status.Active)).Count();
            TempData["TourCount"] = tourService.GetTours().Where(x => x.Status == Convert.ToBoolean(Status.Active)).Count();
            return View();
        }

        #region AccomodationType
        public ActionResult AccomodationList()
        {
            return View(accomodationTypeService.GetAccomodationTypes());
        }
        
        public ActionResult CreateAccomodationType()
        {
            TempData["referer"] = goBackUrl.GetBackUrl();
            TempData.Keep("referer");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAccomodationType(AccomodationType accomodationType)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (accomodationTypeService.IfAccomodationTypeCodeExist(accomodationType.AccomodationCode))
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), $"AccomodationCode: {accomodationType.AccomodationCode} has been already created.");
                        return View();
                    }
                    else
                    {
                        string[] result = accomodationTypeService.CreateAccomodationType(accomodationType).Split(":");
                        string type = result[0];
                        var message = result[1];
                        if (type == NotificationType.exception.ToString())
                            return RedirectToAction("Index", "Error", message);
                        else
                            TempData["Notification"] = Notification.Message(type, message);
                        return RedirectToAction("AccomodationList");
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

        public ActionResult EditAccomodationType(int id)
        {
            TempData["referer"] = goBackUrl.GetBackUrl();
            TempData.Keep("referer");
            AccomodationType accomodationTypeUpdated = accomodationTypeService.GetAccomodationTypeById(id);
            if (accomodationTypeUpdated != null)
            {
                return View(accomodationTypeUpdated);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return View("AccomodationTypeList");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAccomodationType(AccomodationType accomodationType)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (accomodationTypeService.IfAccomodationTypeCodeExist(accomodationType.AccomodationCode) && accomodationTypeService.GetAccomodationTypeById(accomodationType.Id).AccomodationCode != accomodationType.AccomodationCode)
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), $"AccomodationCode: {accomodationType.AccomodationCode} has been already created.");
                        return View();
                    }
                    else
                    {
                        AccomodationType accomodationTypeUpdated = accomodationTypeService.GetAccomodationTypeById(accomodationType.Id);
                        accomodationTypeUpdated.AccomodationCode = accomodationType.AccomodationCode;
                        accomodationTypeUpdated.Description = accomodationType.Description;
                        accomodationTypeUpdated.Status = accomodationType.Status;             
                        string[] result = accomodationTypeService.UpdateAccomodationType(accomodationTypeUpdated).Split(":");
                        string type = result[0];
                        var message = result[1];
                        if (type == NotificationType.exception.ToString())
                            return RedirectToAction("Index", "Error", message);
                        else
                            TempData["Notification"] = Notification.Message(type, message);
                        return RedirectToAction("AccomodationList");
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
        public ActionResult DeleteAccomodationType(int id)
        {
            AccomodationType accomodationTypeDeleted = accomodationTypeService.GetAccomodationTypeById(id);
            if (accomodationTypeDeleted != null)
            {
                try
                {
                    string[] result = accomodationTypeService.DeleteAccomodationType(accomodationTypeDeleted.Id).Split(":");
                    string type = result[0];
                    var message = result[1];
                    if (type == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", message);
                    else
                        TempData["Notification"] = Notification.Message(type, message);
                    return RedirectToAction("AccomodationList");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return View("AccomodationTypeList");
            }
        }

        #endregion

        #region Car

        public ActionResult CarList()
        {
            return View(carService.GetCars());
        }

        public ActionResult CreateCar()
        {
            TempData["referer"] = goBackUrl.GetBackUrl();
            TempData.Keep("referer");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCar(Car car)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string[] result = carService.CreateCar(car).Split(":");
                    string type = result[0];
                    var message = result[1];
                    if (type == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", message);
                    else
                        TempData["Notification"] = Notification.Message(type, message);
                    return RedirectToAction("CarList");
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
        public ActionResult EditCar(int id)
        {
            TempData["referer"] = goBackUrl.GetBackUrl();
            TempData.Keep("referer");
            Car carUpdated = carService.GetCarById(id);
            if (carUpdated != null)
            {
                return View(carUpdated);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return View("CarList");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCar(Car car)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Car carUpdated = carService.GetCarById(car.Id);
                    carUpdated.Model = car.Model;
                    carUpdated.Brand = car.Brand;
                    carUpdated.Status = car.Status;
                    string[] result = carService.UpdateCar(carUpdated).Split(":");
                    string type = result[0];
                    var message = result[1];
                    if (type == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", message);
                    else
                        TempData["Notification"] = Notification.Message(type, message);
                    return RedirectToAction("CarList");
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

        public ActionResult DeleteCar(int id)
        {
            Car carDeleted = carService.GetCarById(id);
            if (carDeleted != null)
            {
                try
                {
                    string[] result = carService.DeleteCar(carDeleted.Id).Split(":");
                    string type = result[0];
                    var message = result[1];
                    if (type == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", message);
                    else
                        TempData["Notification"] = Notification.Message(type, message);
                    return RedirectToAction("CarList");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return View("CarList");
            }
        }

        #endregion

        #region Plane

        public ActionResult PlaneList()
        {
            return View(planeService.GetPlanes());
        }

        public ActionResult CreatePlane()
        {
            TempData["referer"] = goBackUrl.GetBackUrl();
            TempData.Keep("referer");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePlane(Plane plane)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string[] result = planeService.CreatePlane(plane).Split(":");
                    string type = result[0];
                    var message = result[1];
                    if (type == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", message);
                    else
                        TempData["Notification"] = Notification.Message(type, message);
                    return RedirectToAction("PlaneList");
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
        public ActionResult EditPlane(int id)
        {
            TempData["referer"] = goBackUrl.GetBackUrl();
            TempData.Keep("referer");
            Plane planeUpdated = planeService.GetPlaneById(id);
            if (planeUpdated != null)
            {
                return View(planeUpdated);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return View("PlaneList");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPlane(Plane plane)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Plane planeUpdated = planeService.GetPlaneById(plane.Id);
                    planeUpdated.Brand = plane.Brand;
                    planeUpdated.Model = plane.Model;
                    planeUpdated.Status = plane.Status;
                    string[] result = planeService.UpdatePlane(planeUpdated).Split(":");
                    string type = result[0];
                    var message = result[1];
                    if (type == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", message);
                    else
                        TempData["Notification"] = Notification.Message(type, message);
                    return RedirectToAction("PlaneList");
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

        public ActionResult DeletePlane(int id)
        {
            Plane planeDeleted = planeService.GetPlaneById(id);
            if (planeDeleted != null)
            {
                try
                {
                    string[] result = planeService.DeletePlane(planeDeleted.Id).Split(":");
                    string type = result[0];
                    var message = result[1];
                    if (type == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", message);
                    else
                        TempData["Notification"] = Notification.Message(type, message);
                    return RedirectToAction("PlaneList");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return View("PlaneList");
            }
        }

        #endregion

        #region Region
        public ActionResult RegionList()
        {
            return View(regionService.GetRegions());
        }

        public ActionResult CreateRegion()
        {
            TempData["referer"] = goBackUrl.GetBackUrl();
            TempData.Keep("referer");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRegion(Region region)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string[] result = regionService.CreateRegion(region).Split(":");
                    string type = result[0];
                    var message = result[1];
                    if (type == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", message);
                    else
                        TempData["Notification"] = Notification.Message(type, message);
                    return RedirectToAction("RegionList");
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
        public ActionResult EditRegion(int id)
        {
            TempData["referer"] = goBackUrl.GetBackUrl();
            TempData.Keep("referer");
            Region regionUpdated = regionService.GetRegionById(id);
            if (regionUpdated != null)
            {
                return View(regionUpdated);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return View("RegionList");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRegion(Region region)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Region regionUpdated = regionService.GetRegionById(region.Id);
                    regionUpdated.RegionName = region.RegionName;
                    regionUpdated.Status = region.Status;
                    string[] result = regionService.UpdateRegion(regionUpdated).Split(":");
                    string type = result[0];
                    var message = result[1];
                    if (type == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", message);
                    else
                        TempData["Notification"] = Notification.Message(type, message);
                    return RedirectToAction("RegionList");
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

        public ActionResult DeleteRegion(int id)
        {
            Region regionDeleted = regionService.GetRegionById(id);
            if (regionDeleted != null)
            {
                try
                {
                    string[] result = regionService.DeleteRegion(regionDeleted.Id).Split(":");
                    string type = result[0];
                    var message = result[1];
                    if (type == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", message);
                    else
                        TempData["Notification"] = Notification.Message(type, message);
                    return RedirectToAction("RegionList");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return View("RegionList");
            }
        }

        #endregion

        #region RoomLocation
        public ActionResult RoomLocationList()
        {
            return View(roomLocationService.GetRoomLocationTypes());
        }

        public ActionResult CreateRoomLocation()
        {
            TempData["referer"] = goBackUrl.GetBackUrl();
            TempData.Keep("referer");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRoomLocation(RoomLocation roomLocation)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (roomLocationService.IfRoomLocationCodeExist(roomLocation.RoomLocationCode))
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), $"RoomLocationCode: {roomLocation.RoomLocationCode} has been already created!");
                        return View();
                    }
                    else
                    {
                        string[] result = roomLocationService.CreateRoomLocation(roomLocation).Split(":");
                        string type = result[0];
                        var message = result[1];
                        if (type == NotificationType.exception.ToString())
                            return RedirectToAction("Index", "Error", message);
                        else
                            TempData["Notification"] = Notification.Message(type, message);
                        return RedirectToAction("RoomLocationList");
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
        public ActionResult EditRoomLocation(int id)
        {
            TempData["referer"] = goBackUrl.GetBackUrl();
            TempData.Keep("referer");
            RoomLocation roomLocation = roomLocationService.GetRoomLocationById(id);
            if (roomLocation != null)
            {
                return View(roomLocation);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return View("RoomLocationList");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRoomLocation(RoomLocation roomLocation)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (roomLocationService.IfRoomLocationCodeExist(roomLocation.RoomLocationCode) && roomLocationService.GetRoomLocationById(roomLocation.Id).RoomLocationCode != roomLocation.RoomLocationCode)
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), $"RoomLocationCode: {roomLocation.RoomLocationCode} has been already created..");
                        return View();
                    }
                    else
                    {
                        RoomLocation roomLocationUpdated = roomLocationService.GetRoomLocationById(roomLocation.Id);
                        roomLocationUpdated.RoomLocationCode = roomLocation.RoomLocationCode;
                        roomLocationUpdated.Description = roomLocation.Description;
                        roomLocationUpdated.Status = roomLocation.Status;
                        string[] result = roomLocationService.UpdateRoomLocation(roomLocationUpdated).Split(":");
                        string type = result[0];
                        var message = result[1];
                        if (type == NotificationType.exception.ToString())
                            return RedirectToAction("Index", "Error", message);
                        else
                            TempData["Notification"] = Notification.Message(type, message);
                        return RedirectToAction("RoomLocationList");
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

        public ActionResult DeleteRoomLocation(int id)
        {
            RoomLocation roomLocationDeleted = roomLocationService.GetRoomLocationById(id);
            if (roomLocationDeleted != null)
            {
                try
                {
                    string[] result = roomLocationService.DeleteRoomLocation(roomLocationDeleted.Id).Split(":");
                    string type = result[0];
                    var message = result[1];
                    if (type == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", message);
                    else
                        TempData["Notification"] = Notification.Message(type, message);
                    return RedirectToAction("RoomLocationList");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return View("RoomLocationList");
            }
        }

        #endregion

        #region RoomType
        public ActionResult RoomTypeList()
        {
            return View(roomTypeService.GetRoomTypes());
        }

        public ActionResult CreateRoomType()
        {
            TempData["referer"] = goBackUrl.GetBackUrl();
            TempData.Keep("referer");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRoomType(RoomType roomType)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (roomTypeService.IfRoomTypeCodeExist(roomType.RoomTypeCode))
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), $"RoomTypeCode: {roomType.RoomTypeCode} has been already created..");
                        return View();
                    }
                    else
                    {
                        string[] result = roomTypeService.CreateRoomType(roomType).Split(":");
                        string type = result[0];
                        var message = result[1];
                        if (type == NotificationType.exception.ToString())
                            return RedirectToAction("Index", "Error", message);
                        else
                            TempData["Notification"] = Notification.Message(type, message);
                        return RedirectToAction("RoomTypeList");
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
        public ActionResult EditRoomType(int id)
        {
            TempData["referer"] = goBackUrl.GetBackUrl();
            TempData.Keep("referer");
            RoomType roomType = roomTypeService.GetRoomTypeById(id);
            if (roomType != null)
            {
                return View(roomType);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return View("RoomTypeList");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRoomType(RoomType roomType)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (roomTypeService.IfRoomTypeCodeExist(roomType.RoomTypeCode) && roomTypeService.GetRoomTypeById(roomType.Id).RoomTypeCode != roomType.RoomTypeCode)
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), $"RoomTypeCode: {roomType.RoomTypeCode} has been already created..");
                        return View();
                    }
                    else
                    {
                        RoomType roomTypeUpdated = roomTypeService.GetRoomTypeById(roomType.Id);
                        roomTypeUpdated.RoomTypeCode = roomType.RoomTypeCode;
                        roomTypeUpdated.Description = roomType.Description;
                        roomTypeUpdated.Status = roomType.Status;
                        string[] result = roomTypeService.UpdateRoomType(roomTypeUpdated).Split(":");
                        string type = result[0];
                        var message = result[1];
                        if (type == NotificationType.exception.ToString())
                            return RedirectToAction("Index", "Error", message);
                        else
                            TempData["Notification"] = Notification.Message(type, message);
                        return RedirectToAction("RoomTypeList");
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

        public ActionResult DeleteRoomType(int id)
        {
            RoomType roomTypeDeleted = roomTypeService.GetRoomTypeById(id);
            if (roomTypeDeleted != null)
            {
                try
                {
                    string[] result = roomTypeService.DeleteRoomType(roomTypeDeleted.Id).Split(":");
                    string type = result[0];
                    var message = result[1];
                    if (type == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", message);
                    else
                        TempData["Notification"] = Notification.Message(type, message);
                    return RedirectToAction("RoomTypeList");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return View("RoomTypeList");
            }
        }

        #endregion

        #region Route

        public ActionResult RouteList()
        {
            return View(routeService.GetRoutes());
        }

        [HttpGet]
        public ActionResult CreateRoute()
        {
            TempData["referer"] = goBackUrl.GetBackUrl();
            TempData.Keep("referer");    
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRoute(Route route)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (routeService.IfRouteCodeExist(route.RouteCode))
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), $"RouteCode: {route.RouteCode} has been already created.");
                        return View();
                    }
                    else
                    {
                        string[] result = routeService.CreateRoute(route).Split(":");
                        string type = result[0];
                        var message = result[1];
                        if (type == NotificationType.exception.ToString())
                            return RedirectToAction("Index", "Error", message);
                        else
                            TempData["Notification"] = Notification.Message(type, message);
                        return RedirectToAction("RouteList");
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
                TempData["region"] = new SelectList(regionService.GetActiveRegions(), "Id", "RegionName");
                return View();
            }
        }
        [HttpGet]
        public ActionResult EditRoute(int id)
        {
            TempData["referer"] = goBackUrl.GetBackUrl();
            TempData.Keep("referer");      
            Route routeUpdated = routeService.GetRouteById(id);
            if (routeUpdated != null)
            {
                return View(routeUpdated);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return View("RouteList");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRoute(Route route)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    if (routeService.IfRouteCodeExist(route.RouteCode) && routeService.GetRouteById(route.Id).RouteCode != route.RouteCode)
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), $"RouteCode: {route.RouteCode} has been already created.");
                        return View();
                    }
                    else
                    {
                        Route routeUpdated = routeService.GetRouteById(route.Id);
                        routeUpdated.RouteCode = route.RouteCode;
                        routeUpdated.Description = route.Description;
                        routeUpdated.Status = route.Status;                      
                        string[] result = routeService.UpdateRoute(routeUpdated).Split(":");
                        string type = result[0];
                        var message = result[1];
                        if (type == NotificationType.exception.ToString())
                            return RedirectToAction("Index", "Error", message);
                        else
                            TempData["Notification"] = Notification.Message(type, message);
                        return RedirectToAction("RouteList");
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
                TempData["region"] = new SelectList(regionService.GetActiveRegions(), "Id", "RegionName");
                return View();
            }
        }
        public ActionResult DeleteRoute(int id)
        {
            Route routeDeleted = routeService.GetRouteById(id);
            if (routeDeleted != null)
            {
                try
                {
                    string[] result = routeService.DeleteRoute(routeDeleted.Id).Split(":");
                    string type = result[0];
                    var message = result[1];
                    if (type == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", message);
                    else
                        TempData["Notification"] = Notification.Message(type, message);
                    return RedirectToAction("RouteList");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return View("RouteList");
            }
        }

        #endregion

        #region Tour
        public ActionResult TourList()
        {
            return View(tourService.GetTours());
        }

        public ActionResult CreateTour()
        {
            TempData["referer"] = goBackUrl.GetBackUrl();
            TempData.Keep("referer");
            List<Region> regions = regionService.GetActiveRegions().ToList();
            ViewBag.Regions = new SelectList(regions, "Id", "RegionName");

            List<TourOperator> tourOperators = tourOperatorService.GetTourOperators().ToList();
            ViewBag.TourOperators = new SelectList(tourOperators, "Id", "TourOperatorName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTour(Tour tour)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (tourService.IfTourCodeExist(tour.TourCode))
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), $"TourCode: {tour.TourCode} has been already created.");
                        List<Region> regions = regionService.GetRegions().ToList();
                        ViewBag.Regions = new SelectList(regions, "Id", "RegionName");

                        List<TourOperator> tourOperators = tourOperatorService.GetTourOperators().ToList();
                        ViewBag.TourOperators = new SelectList(tourOperators, "Id", "TourOperatorName");
                        return View();
                    }
                    else
                    {
                        string[] result = tourService.CreateTour(tour).Split(":");
                        string type = result[0];
                        var message = result[1];
                        if (type == NotificationType.exception.ToString())
                            return RedirectToAction("Index", "Error", message);
                        else
                            TempData["Notification"] = Notification.Message(type, message);
                        return RedirectToAction("TourList");
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

                List<TourOperator> tourOperators = tourOperatorService.GetTourOperators().ToList();
                ViewBag.TourOperators = new SelectList(tourOperators, "Id", "TourOperatorName");
                TempData.Keep("referer");
                return View();
            }
        }
        public ActionResult EditTour(int id)
        {
            TempData["referer"] = goBackUrl.GetBackUrl();
            TempData.Keep("referer");
            List<Region> regions = regionService.GetRegions().ToList();
            ViewBag.Regions = new SelectList(regions, "Id", "RegionName");
            List<TourOperator> tourOperators = tourOperatorService.GetTourOperators().ToList();
            ViewBag.TourOperators = new SelectList(tourOperators, "Id", "TourOperatorName");

            Tour tourUpdated = tourService.GetTourById(id);
            if (tourUpdated != null)
            {
                return View(tourUpdated);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return View("TourList");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTour(Tour tour)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (tourService.IfTourCodeExist(tour.TourCode) && tourService.GetTourById(tour.Id).TourCode != tour.TourCode)
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), $"TourCode: {tour.TourCode} has been already created.");
                        List<Region> regions = regionService.GetRegions().ToList();
                        ViewBag.Regions = new SelectList(regions, "Id", "RegionName");

                        List<TourOperator> tourOperators = tourOperatorService.GetTourOperators().ToList();
                        ViewBag.TourOperators = new SelectList(tourOperators, "Id", "TourOperatorName");
                        return View();
                    }
                    else
                    {
                        Tour updatedTour = tourService.GetTourById(tour.Id);
                        updatedTour.TourCode = tour.TourCode;
                        updatedTour.TourName = tour.TourName;
                        updatedTour.RegionId = tour.RegionId;
                        updatedTour.TourOperatorId = tour.TourOperatorId;
                        updatedTour.Status = tour.Status;

                        string[] result = tourService.UpdateTour(updatedTour).Split(":");
                        string type = result[0];
                        var message = result[1];
                        if (type == NotificationType.exception.ToString())
                            return RedirectToAction("Index", "Error", message);
                        else
                            TempData["Notification"] = Notification.Message(type, message);
                        return RedirectToAction("TourList");
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }
            }
            else
            {
                List<Region> regions = regionService.GetRegions().ToList();
                ViewBag.Regions = new SelectList(regions, "Id", "RegionName");
                List<TourOperator> tourOperators = tourOperatorService.GetTourOperators().ToList();
                ViewBag.TourOperators = new SelectList(tourOperators, "Id", "TourOperatorName");
                TempData.Keep("referer");
                return View();
            }
        }

        public ActionResult DeleteTour(int id)
        {
            Tour tourDeleted = tourService.GetTourById(id);
            if (tourDeleted != null)
            {
                try
                {
                    string[] result = tourService.DeleteTour(tourDeleted.Id).Split(":");
                    string type = result[0];
                    var message = result[1];
                    if (type == NotificationType.exception.ToString())
                        return RedirectToAction("Index", "Error", message);
                    else
                        TempData["Notification"] = Notification.Message(type, message);
                    return RedirectToAction("TourList");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity could not be found.");
                return View("TourList");
            }
        }

        #endregion


    }
}
