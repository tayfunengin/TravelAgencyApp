using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repository.Authentication;
using Service.FlightCompanyService;
using Service.HotelService;
using Service.RentACarCompanyService;
using Service.SalesService;
using Service.TourGuideService;
using Service.TourOperatorService;
using Service.TransportationCompanyService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UI.Filters;

namespace UI.Areas.Dashboard.Controllers
{    
    [Area("Dashboard")]
    [TypeFilter(typeof(AcFilter))]
    [TypeFilter(typeof(AuthFilter))]
    [Authorize(Roles = "Admin, Accounting,Admin,Sales,SalesAgent,User")]
    public class HomeController : Controller
    {
        private readonly RoleManager<AppUserRole> roleManager;
        private readonly UserManager<AppUser> userManager;
        private readonly IHotelService hotelService;
        private readonly IFlightCompanyService flightCompanyService;
        private readonly ITransportationCompanyService transportationCompanyService;
        private readonly ITourOperatorService tourOperatorService;
        private readonly ITourGuideService tourGuideService;
        private readonly IRentACarCompanyService rentACarCompanyService;
        private readonly ISalesService salesService;
        public HomeController(RoleManager<AppUserRole> roleManager, UserManager<AppUser> userManager, IHotelService hotelService, IFlightCompanyService flightCompanyService, ITransportationCompanyService transportationCompanyService, ITourOperatorService tourOperatorService, ITourGuideService tourGuideService, IRentACarCompanyService rentACarCompanyService, ISalesService salesService)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.hotelService = hotelService;
            this.flightCompanyService = flightCompanyService;
            this.transportationCompanyService = transportationCompanyService;
            this.tourOperatorService = tourOperatorService;
            this.tourGuideService = tourGuideService;
            this.rentACarCompanyService = rentACarCompanyService;
            this.salesService = salesService;
        }
        public IActionResult Index()
        {
            TempData["users"] = userManager.Users.ToList();
            TempData["roles"] = roleManager.Roles.ToList();
            TempData["hotels"] = hotelService.GetHotels().Where(x => x.Status == Convert.ToBoolean(Status.Active)).ToList();
            TempData["flightCompanies"] = flightCompanyService.GetFlightCompanies().Where(x => x.Status == Convert.ToBoolean(Status.Active)).ToList();
            TempData["transportationCompanies"] = transportationCompanyService.GetTransportationCompanies().Where(x => x.Status == Convert.ToBoolean(Status.Active)).ToList();
            TempData["tourOperators"] = tourOperatorService.GetTourOperators().Where(x => x.Status == Convert.ToBoolean(Status.Active)).ToList();
            TempData["tourGuides"] = tourGuideService.GetTourGuides().Where(x => x.Status == Convert.ToBoolean(Status.Active)).ToList();
            TempData["rentACarCompanies"] = rentACarCompanyService.GetRentACarCompanies().Where(x => x.Status == Convert.ToBoolean(Status.Active)).ToList();
            TempData["sales"] = salesService.GetAllSales().Where(x => x.Status == Convert.ToBoolean(Status.Active)).ToList();
            return View();
        }
    }
}
