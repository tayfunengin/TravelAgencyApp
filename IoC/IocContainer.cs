using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Repository.Authentication;
using Repository.Context;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Session;
using Service.AccomodationTypeService;
using Service.CarService;
using Service.HotelService;
using Service.PlaneService;
using Service.RegionService;
using Service.RoomLocationService;
using Service.RoomTypeService;
using Service.TourService;
using Service.UserLogService;
using Service.ChangeLogService;
using Service.TourOperatorService;
using Microsoft.EntityFrameworkCore.Proxies;
using Service.FlightCompanyService;
using Service.RentACarCompanyService;
using Service.PhotographyService;
using Domain.Entities;
using Service.TransportationCompanyService;
using Service.RouteService;
using Service.TourGuideService;
using Service.HotelPriceService;
using Service.TourOperatorPriceService;
using Service.RentACarPriceService;
using Service.FlightPriceService;
using Service.TransportationPriceService;
using Service.TourGuidePriceService;
using Service.PackageService;
using Service.TourDateInPackageService;
using Service.CustomerService;
using Service.SalesService;
using Service.AccountBalanceService;

namespace IoC
{
    public static class IocContainer
    {
        public static void ConfigureIoC(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IAccomodationTypeService, AccomodationTypeService>();
            services.AddScoped<ICarService, CarService>();
            services.AddScoped<IHotelService, HotelService>();
            services.AddScoped<IPlaneService, PlaneService>();
            services.AddScoped<IRegionService, RegionService>();
            services.AddScoped<IRoomLocationService, RoomLocationService>();
            services.AddScoped<IRoomTypeService, RoomTypeService>();
            services.AddScoped<ITourService, TourService>();
            services.AddScoped<IUserLogService, UserLogService>();
            services.AddScoped<IChangeLogService, ChangeLogService>();
            services.AddScoped<ITourOperatorService, TourOperatorService>();
            services.AddScoped<IFlightCompanyService, FlightCompanyService>();
            services.AddScoped<IRentACarCompanyService, RentACarCompanyService>();
            services.AddScoped<IPhotographyService, PhotographyService>();
            services.AddScoped<ITransportationCompanyService, TransportationCompanyService>();
            services.AddScoped<IRouteService, RouteService>();
            services.AddScoped<ITourGuideService, TourGuideService>();
            services.AddScoped<IHotelPriceService, HotelPriceService>();
            services.AddScoped<ITourOperatorPriceService, TourOperatorPriceService>();
            services.AddScoped<IRentACarPriceService, RentACarPriceService>();
            services.AddScoped<IFlightPriceService, FlightPriceService>();
            services.AddScoped<ITransportationPriceService, TransportationPriceService>();
            services.AddScoped<ITourGuidePriceService, TourGuidePriceService>();
            services.AddScoped<IPackageService, PackageService>();
            services.AddScoped<ITourDateInPackageService, TourDateInPackageService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ISalesService, SalesService>();
            services.AddScoped<IAccountBalanceService, AccountBalanceService>();

            services.AddDbContext<ApplicationDbContext>(opitons =>
            {
                opitons.UseLazyLoadingProxies();
                opitons.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));                              
            });

            services.AddIdentity<AppUser, AppUserRole>(x =>
            {
                x.Password.RequireDigit = false;
                x.Password.RequireLowercase = false;
                x.Password.RequireUppercase = false;
                x.Password.RequiredLength = 4;
                x.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>();

            services.ConfigureApplicationCookie(x =>
            {
                x.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                x.Cookie = new Microsoft.AspNetCore.Http.CookieBuilder
                {
                    Name = "TravelAgencyCookie"
                };
                x.SlidingExpiration = true;
                x.ExpireTimeSpan = TimeSpan.FromMinutes(10);
            });
            services.AddDistributedMemoryCache();
            services.AddSession(s => s.IdleTimeout = TimeSpan.FromMinutes(10));
        }
    }
}
