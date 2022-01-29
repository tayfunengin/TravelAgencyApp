using Domain.Entities;
using Domain.Enums;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.RouteService
{
    public class RouteService : IRouteService
    {
        private readonly IRepository<Route> repository;

        public RouteService(IRepository<Route> repository)
        {
            this.repository = repository;
        }

        public string CreateRoute(Route route)
        {
            return this.repository.Insert(route);
        }

        public string DeleteRoute(int id)
        {
            return this.repository.Delete(id);
        }

        public IEnumerable<Route> GetActiveRoutes()
        {
            return this.repository.GetDefault(x => x.Status == Convert.ToBoolean(Status.Active));
        }

        public Route GetRouteById(int id)
        {
            return this.repository.GetById(id);
        }

        public IEnumerable<Route> GetRoutes()
        {
            return this.repository.GetAll();
        }

        public bool IfRouteCodeExist(string routeCode)
        {
            Route route = this.repository.GetDefault(x => x.RouteCode == routeCode).FirstOrDefault();
            if (route == null)
                return false;
            else
                return true;
        }

        public string UpdateRoute(Route route)
        {
            return this.repository.Update(route);
        }
    }
}
