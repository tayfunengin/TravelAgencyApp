using Domain.Entities;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.TourGuideService
{
    public class TourGuideService : ITourGuideService
    {
        private readonly IRepository<TourGuide> repository;

        public TourGuideService(IRepository<TourGuide> repository)
        {
            this.repository = repository;
        }
        public string CreateTourGuide(TourGuide tourGuide)
        {
            return this.repository.Insert(tourGuide);
        }

        public string DeleteTourGuide(int tourGuideId)
        {
            return this.repository.Delete(tourGuideId);
        }

        public TourGuide GetTourGuideById(int tourGuideId)
        {
            return this.repository.GetById(tourGuideId);
        }

        public IEnumerable<TourGuide> GetTourGuides()
        {
            return this.repository.GetAll();
        }

        public bool IfTourGuideCodeExist(string code)
        {
            TourGuide tourGuide = this.repository.GetDefault(x => x.TourGuideCode == code).FirstOrDefault();
            if (tourGuide == null)
                return false;
            else
                return true;
        }

        public string UpdateTourGuide(TourGuide tourGuide)
        {
            return this.repository.Update(tourGuide);
        }
    }
}
