using Common;
using Domain;
using Domain.Entities;
using Domain.Maps;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repository.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Repository.Context
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppUserRole, int>
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<AccomodationType> AccomodationTypes { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<FlightCompany> FlightCompanies { get; set; }
        public DbSet<RentACarCompany> RentACarCompanies { get; set; }
        public DbSet<RoomLocation> RoomLocations { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<TourGuide> TourGuides { get; set; }
        public DbSet<TourOperator> TourOperators { get; set; }
        public DbSet<TransportationCompany> TransportationCompanies { get; set; }
        public DbSet<Plane> Planes { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Photography> Photographies { get; set; }
        public DbSet<UserLog> UserLogs { get; set; }
        public DbSet<ChangeLog> ChangeLogs { get; set; }
        public DbSet<HotelPrice> HotelPrices { get; set; }
        public DbSet<TourOperatorPrice> TourOperatorPrices { get; set; }
        public DbSet<RentACarPrice> RentACarPrices { get; set; }
        public DbSet<FlightPrice> FlightPrices { get; set; }
        public DbSet<TransportationPrice> TrasnportationPrices { get; set; }
        public DbSet<TourGuidePrice> TourGuidePrices { get; set; }
        public DbSet<TourDateInPackage> TourDateInPackges { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Sales> Sales { get; set; }
        public DbSet<SalesCodeCounter> SalesCode { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<AccountBalance> AccountBalances { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            new AccomodationTypeMap(builder.Entity<AccomodationType>());
            new HotelMap(builder.Entity<Hotel>());
            new TourMap(builder.Entity<Tour>());
            new TourOperatorMap(builder.Entity<TourOperator>());
            new RouteMap(builder.Entity<Route>());
            new TransportationCompanyMap(builder.Entity<TransportationCompany>());
            new CarMap(builder.Entity<Car>());
            new RentACarCompanyMap(builder.Entity<RentACarCompany>());     
            new TourGuideMap(builder.Entity<TourGuide>());
            new FlightCompanyMap(builder.Entity<FlightCompany>());
            new RoomLocationMap(builder.Entity<RoomLocation>());
            new RoomTypeMap(builder.Entity<RoomType>());
            new PlaneMap(builder.Entity<Plane>());
            new RegionMap(builder.Entity<Region>());
            new UserLogMap(builder.Entity<UserLog>());
            new ChangeLogMap(builder.Entity<ChangeLog>());
            new HotelPriceMap(builder.Entity<HotelPrice>());
            new TourOperatorPriceMap(builder.Entity<TourOperatorPrice>());
            new RentACarPriceMap(builder.Entity<RentACarPrice>());
            new FlightPriceMap(builder.Entity<FlightPrice>());
            new TransportationPriceMap(builder.Entity<TransportationPrice>());
            new TourGuidePriceMap(builder.Entity<TourGuidePrice>());
            new TourDateInPackageMap(builder.Entity<TourDateInPackage>());
            new PackageMap(builder.Entity<Package>());
            new CustomerMap(builder.Entity<Customer>());
            new SalesMap(builder.Entity<Sales>());
            new AccountBalanceMap(builder.Entity<AccountBalance>());

            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            var modifiedEntries = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified).ToList();

            foreach (var item in modifiedEntries)
            {
                var entityRepository = item.Entity as BaseEntity;

                if (item != null)
                {
                    if (item.State == EntityState.Modified)
                    {
                        string computerName = Environment.MachineName;
                        string userName = null;
                        int id = 0;
                        string ip = IpAddresses.GetHostName();
                        DateTime modifiedDate = DateTime.Now;
                        string entityName = item.Entity.GetType().ToString().Split('.').Last().Replace("Proxy","");
                        foreach (var key in httpContextAccessor.HttpContext.Session.Keys)
                        {
                            if (key == "login")
                            {
                                userName = httpContextAccessor.HttpContext.User.Identity.Name;
                                break;
                            }
                            else
                            {
                                userName = "Bilinmiyor";
                            }
                        }
                        foreach (var property in item.Properties)
                        {
                            if (property.Metadata.Name == "Id")
                                id = Convert.ToInt32(property.CurrentValue);

                            var previousValue = item.GetDatabaseValues().GetValue<object>(property.Metadata.Name);
                            if (previousValue == null)
                                continue;
                            
                            var currentValue = property.CurrentValue;
                            if (!object.Equals(previousValue, currentValue) && previousValue.GetType() != typeof(DateTime))
                            {
                                ChangeLog changeLog = new ChangeLog();
                                changeLog.ComputerName = computerName;
                                changeLog.IpAddress = ip;
                                changeLog.UserName = userName;
                                changeLog.ModifiedDate = modifiedDate;
                                changeLog.EntityName = entityName;
                                changeLog.EntityId = id;

                                changeLog.PropertyName = property.Metadata.Name;
                                changeLog.PreviousValue = previousValue.ToString();
                                changeLog.NextValue = currentValue == null ? string.Empty : currentValue.ToString();

                                this.ChangeLogs.Add(changeLog);
                            }
                        }
                    }
                }
            }

            return base.SaveChanges();
        }
    }
}
