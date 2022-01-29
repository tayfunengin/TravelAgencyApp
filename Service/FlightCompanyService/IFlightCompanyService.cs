using Domain.Entities;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.FlightCompanyService
{
    public interface IFlightCompanyService
    {
        IEnumerable<FlightCompany> GetFlightCompanies();
        FlightCompany GetFlightCompanyById(int id);
        int GetLatestFlightCompanyId();
        string CreateFlightCompany(FlightCompany flightCompany, int[] regionIds, int[] planeIds, int[] routeIds);
        string UpdateFlightCompany(FlightCompany flightCompany);
        string DeleteFlightCompany(int id);
        bool IfFlightCompanyCodeExist(string flightCompanyCode);
        List<Plane> GetUnAssignedPlanes(int flightCompanyId);
        List<Region> GetUnAssignedRegions(int flightCompanyId);
        List<Route> GetUnAssignedRoutes(int flightCompanyId);
        void AddRegionToFlightCompany(int[] selectedIds, int flightCompanyId);
        void AddPlaneToFlightCompany(int[] selectedIds, int flightCompanyId);
        void AddRouteToFlightCompany(int[] selectedIds, int flightCompanyId);
    }
}
