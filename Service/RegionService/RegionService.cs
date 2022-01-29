using Domain.Entities;
using Domain.Enums;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.RegionService
{
    public class RegionService : IRegionService
    {
        private readonly IRepository<Region> repository;

        public RegionService(IRepository<Region> repository)
        {
            this.repository = repository;
        }

        public string CreateRegion(Region region)
        {
            return this.repository.Insert(region);
        }

        public string DeleteRegion(int id)
        {
            return this.repository.Delete(id);
        }

        public IEnumerable<Region> GetActiveRegions()
        {
            return this.repository.GetDefault(x => x.Status == Convert.ToBoolean( Status.Active ));
        }

        public Region GetRegionById(int id)
        {
            return this.repository.GetById(id);
        }

        public IEnumerable<Region> GetRegions()
        {
            return repository.GetAll();
        }
        public string UpdateRegion(Region region)
        {
            return this.repository.Update(region);
        }
    }
}
