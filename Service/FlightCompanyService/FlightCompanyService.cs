using Domain.Entities;
using Domain.Enums;
using Repository;
using Service.PlaneService;
using Service.RegionService;
using Service.RouteService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Service.FlightCompanyService
{
    public class FlightCompanyService : IFlightCompanyService
    {
        private readonly IRepository<FlightCompany> repository;
        private readonly IPlaneService planeService;
        private readonly IRegionService regionService;
        private readonly IRouteService routeService;

        public FlightCompanyService(IRepository<FlightCompany> repository, IPlaneService planeService, IRegionService regionService, IRouteService routeService)
        {
            this.repository = repository;
            this.planeService = planeService;
            this.regionService = regionService;
            this.routeService = routeService;
        }

        public void AddPlaneToFlightCompany(int[] selectedIds, int flightCompanyId)
        {
            FlightCompany flightCompanyUpdated = this.GetFlightCompanyById(flightCompanyId);
            if (flightCompanyUpdated.Planes == null)
            {
                flightCompanyUpdated.Planes = new List<Plane>();
            }
            foreach (var planeId in selectedIds)
            {
                Plane plane = planeService.GetPlaneById(planeId);
                flightCompanyUpdated.Planes.Add(plane);
            }
        }

        public void AddRegionToFlightCompany(int[] selectedIds, int flightCompanyId)
        {
            FlightCompany flightCompanyUpdated = this.GetFlightCompanyById(flightCompanyId);
            if (flightCompanyUpdated.Regions == null)
            {
                flightCompanyUpdated.Regions = new List<Region>();
            }
            foreach (var redionId in selectedIds)
            {
                Region region = regionService.GetRegionById(redionId);
                flightCompanyUpdated.Regions.Add(region);
            }
        }

        public void AddRouteToFlightCompany(int[] selectedIds, int flightCompanyId)
        {
            FlightCompany flightCompanyUpdated = this.GetFlightCompanyById(flightCompanyId);
            if (flightCompanyUpdated.Routes == null)
            {
                flightCompanyUpdated.Routes = new List<Route>();
            }
            foreach (var routeId in selectedIds)
            {
                Route route = routeService.GetRouteById(routeId);
                flightCompanyUpdated.Routes.Add(route);
            }
        }

        public string CreateFlightCompany(FlightCompany flightCompany, int[] regionIds, int[] planeIds, int[] routeIds)
        {
            using(var transactionScope = new TransactionScope())
            {
               string result = this.repository.Insert(flightCompany);
                string type = result.Split(':')[0];
                var message = result.Split(':')[1];
                if (type == NotificationType.exception.ToString())
                    return result;
                else
                {
                    int maxFlightCompanyId = this.GetLatestFlightCompanyId();
                    this.AddRegionToFlightCompany(regionIds, maxFlightCompanyId);
                    this.AddPlaneToFlightCompany(planeIds, maxFlightCompanyId);
                    this.AddRouteToFlightCompany(routeIds, maxFlightCompanyId);
                 
                }
                transactionScope.Complete();
                return result;               
            }           
        }

        public string DeleteFlightCompany(int id)
        {
            return this.repository.Delete(id);
        }

        public IEnumerable<FlightCompany> GetFlightCompanies()
        {
            return this.repository.GetAll();
        }

        public FlightCompany GetFlightCompanyById(int id)
        {
            return this.repository.GetById(id);
        }

        public int GetLatestFlightCompanyId()
        {
            return this.repository.GetAll().Max(x => x.Id);
        }

        public List<Plane> GetUnAssignedPlanes(int flightCompanyId)
        {
            List<Plane> planes = this.planeService.GetActivePlanes().ToList();
            FlightCompany flightCompany = this.GetFlightCompanyById(flightCompanyId);
            List<Plane> unAssginedPlanes = new List<Plane>();
            if (flightCompany.Planes.Count > 0)
            {
                foreach (var plane in planes)
                {
                    if (!flightCompany.Planes.Contains(plane))
                        unAssginedPlanes.Add(plane);
                }
                return unAssginedPlanes;
            }
            else
            {
                return planes;
            }
        }

        public List<Region> GetUnAssignedRegions(int flightCompanyId)
        {
            List<Region> regions = this.regionService.GetActiveRegions().ToList();
            FlightCompany flightCompany = this.GetFlightCompanyById(flightCompanyId);
            List<Region> unAssginedRegions = new List<Region>();
            if (flightCompany.Regions.Count > 0)
            {
                foreach (var region in regions)
                {
                    if (!flightCompany.Regions.Contains(region))
                        unAssginedRegions.Add(region);
                }
                return unAssginedRegions;
            }
            else
            {
                return regions;
            }
        }

        public List<Route> GetUnAssignedRoutes(int flightCompanyId)
        {
            List<Route> routes = this.routeService.GetActiveRoutes().ToList();
            FlightCompany flightCompany = this.GetFlightCompanyById(flightCompanyId);
            List<Route> unAssginedRoutes = new List<Route>();
            if (flightCompany.Routes.Count > 0)
            {
                foreach (var route in routes)
                {
                    if (!flightCompany.Routes.Contains(route))
                        unAssginedRoutes.Add(route);
                }
                return unAssginedRoutes;
            }
            else
            {
                return routes;
            }
        }

        public bool IfFlightCompanyCodeExist(string flightCompanyCode)
        {
            FlightCompany flightCompany = this.repository.GetDefault(x => x.FlightCompanyCode == flightCompanyCode).FirstOrDefault();
            if (flightCompany == null)
                return false;
            else
                return true;
        }

        public string UpdateFlightCompany(FlightCompany flightCompany)
        {
            return this.repository.Update(flightCompany);
        }
    }
}
