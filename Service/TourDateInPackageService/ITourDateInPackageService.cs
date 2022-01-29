using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.TourDateInPackageService
{
    public interface ITourDateInPackageService
    {
        string Create(TourDateInPackage tourDate);
        string Delete(int id);
        TourDateInPackage GetByPackageIdAndTourPriceId(int packageId, int tourOperatorPriceId);

        TourDateInPackage IsTourDateExist(int packageId, DateTime tourDate);
    }
}
