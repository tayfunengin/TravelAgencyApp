using Domain.Entities;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.TourOperatorService
{
    public interface ITourOperatorService
    {
        IEnumerable<TourOperator> GetTourOperators();
        TourOperator GetTourOperatorById(int id);
        string CreateTourOperator(TourOperator tourOperator);
        string UpdateTourOperator(TourOperator tourOperator);
        string DeleteTourOperator(int id);
        bool IfTourOperatorCodeExist(string tourOperatorCode);
        string RemoveTour(TourOperator tourOperator, Tour tour); 
    }
}
