using Domain.Entities;
using Domain.Enums;
using Repository;
using Service.RouteService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Service.TransportationCompanyService
{
    public class TransportationCompanyService : ITransportationCompanyService
    {
        private readonly IRepository<TransportationCompany> repository;
        private readonly IRouteService routeService;

        public TransportationCompanyService(IRepository<TransportationCompany> repository, IRouteService routeService)
        {
            this.repository = repository;
            this.routeService = routeService;
        }

        public void AddRoutesToTransportationCompany(int[] selectedIds, int transportationCompanyId)
        {
            TransportationCompany transportationCompanyUpdated = this.GetTransportationCompanyById(transportationCompanyId);
            if (transportationCompanyUpdated.Routes == null)
            {
                transportationCompanyUpdated.Routes = new List<Route>();
            }
            foreach (var routeId in selectedIds)
            {
                Route route = this.routeService.GetRouteById(routeId);
                transportationCompanyUpdated.Routes.Add(route);
            }
        }

        public string CreateTransportationCompany(TransportationCompany transportationCompany, int[] routeIds)
        {
            using (var transactionScope = new TransactionScope())
            {
                string result = this.repository.Insert(transportationCompany);

                string type = result.Split(':')[0];
                var message = result.Split(':')[1];
                if (type == NotificationType.exception.ToString())
                    return result;
                else
                {
                    int maxId = this.GetLatestTransportationCompanyId();
                    this.AddRoutesToTransportationCompany(routeIds, maxId);               
                }
                transactionScope.Complete();
                return result;
            }
        }

        public string DeleteTransportationCompany(int transportationCompanyId)
        {
            return this.repository.Delete(transportationCompanyId);
        }

        public int GetLatestTransportationCompanyId()
        {
            return this.repository.GetAll().Max(x => x.Id);
        }

        public IEnumerable<TransportationCompany> GetTransportationCompanies()
        {
          return this.repository.GetAll();
        }

        public TransportationCompany GetTransportationCompanyById(int transportationCompanyId)
        {
            return this.repository.GetById(transportationCompanyId);
        }

        public List<Route> GetUnAssignedRoutes(int transportationCompanyId)
        {

            List<Route> routes = this.routeService.GetActiveRoutes().ToList();
            TransportationCompany transportationCompany = this.GetTransportationCompanyById(transportationCompanyId);
            List<Route> unAssginedroutes = new List<Route>();
            if (transportationCompany.Routes.Count > 0)
            {
                foreach (var route in routes)
                {
                    if (!transportationCompany.Routes.Contains(route))
                        unAssginedroutes.Add(route);
                }
                return unAssginedroutes;
            }
            else
            {
                return routes;
            }
        }

        public bool IfTransportationCodeExist(string code)
        {
            TransportationCompany transportationCompany = this.repository.GetDefault(x => x.TransportaionCompanyCode == code).FirstOrDefault();
            if (transportationCompany == null)
                return false;
            else
                return true;
        }

        public string UpdateTransporationCompany(TransportationCompany transportationCompany)
        {
            return this.repository.Update(transportationCompany);
        }
    }
}
