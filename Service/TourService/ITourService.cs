using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.TourService
{
    public interface ITourService
    {
        IEnumerable<Tour> GetTours();
        Tour GetTourById(int id);
        string CreateTour(Tour tour);
        string UpdateTour(Tour tour);
        string DeleteTour(int id);
        bool IfTourCodeExist(string tourCode);             
    }
}
