using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.RouteService
{
    public interface IRouteService
    {
        IEnumerable<Route> GetRoutes();
        Route GetRouteById(int id);

        string CreateRoute(Route route);

        string UpdateRoute(Route route);

        string DeleteRoute(int id);
        IEnumerable<Route> GetActiveRoutes();

        bool IfRouteCodeExist(string routeCode);
    }
}
