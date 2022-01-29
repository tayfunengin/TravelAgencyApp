using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.RegionService
{
    public interface IRegionService
    {
        IEnumerable<Region> GetRegions();
        Region GetRegionById(int id);

        string CreateRegion(Region region);

        string UpdateRegion(Region region);

        string DeleteRegion(int id);
        IEnumerable<Region> GetActiveRegions();      
    }
}
