using Domain.Entities;
using Domain.Enums;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.TourDateInPackageService
{
    public class TourDateInPackageService : ITourDateInPackageService
    {
        private readonly IRepository<TourDateInPackage> repository;

        public TourDateInPackageService(IRepository<TourDateInPackage> repository)
        {
            this.repository = repository;
        }
        public string Create(TourDateInPackage tourDate)
        {
            return this.repository.Insert(tourDate);
        }

        public string Delete(int id)
        {
            return this.repository.Delete(id);
        }

        public TourDateInPackage GetByPackageIdAndTourPriceId(int packageId, int tourOperatorPriceId)
        {
            return this.repository.GetDefault(x => x.PackageId == packageId && x.TourPriceId == tourOperatorPriceId && x.Status == Convert.ToBoolean( Status.Active )).FirstOrDefault();
        }

        public TourDateInPackage IsTourDateExist(int packageId, DateTime tourDate)
        {
            return this.repository.GetDefault(x=>x.PackageId == packageId && x.TourDate == tourDate && x.Status == Convert.ToBoolean(Status.Active)).FirstOrDefault();
        }
    }
}
