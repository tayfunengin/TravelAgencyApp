using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.TourService
{
    public class TourService : ITourService
    {
        private readonly IRepository<Tour> repository;

        public TourService(IRepository<Tour> repository)
        {
            this.repository = repository;
        }
        public string CreateTour(Tour tour)
        {
            return this.repository.Insert(tour);
        }

        public string DeleteTour(int id)
        {
            return this.repository.Delete(id);
        }
        public Tour GetTourById(int id)
        {
            return this.repository.GetById(id);
        }
        public bool IfTourCodeExist(string tourCode)
        {
            Tour tour = repository.GetDefault(x => x.TourCode == tourCode).FirstOrDefault();
            if (tour == null)
                return false;
            else
                return true;
        }
        public IEnumerable<Tour> GetTours()
        {
            return this.repository.GetAll();
        }

        public string UpdateTour(Tour tour)
        {
            return this.repository.Update(tour);
        }
    }
}
