using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.TransportationCompanyService
{
    public interface ITransportationCompanyService
    {
        IEnumerable<TransportationCompany> GetTransportationCompanies();

        bool IfTransportationCodeExist(string code);

        string CreateTransportationCompany(TransportationCompany transportationCompany, int[] routeIds);
        int GetLatestTransportationCompanyId();
        void AddRoutesToTransportationCompany(int[] selectedIds, int transportationCompanyId);
        TransportationCompany GetTransportationCompanyById(int transportationCompanyId);
        string UpdateTransporationCompany(TransportationCompany transportationCompany);

        string DeleteTransportationCompany(int transportationCompanyId);

        List<Route> GetUnAssignedRoutes(int transportationCompanyId);
    }
}
