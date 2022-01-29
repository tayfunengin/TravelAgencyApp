using Domain.Entities;
using Domain.Enums;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.PlaneService
{
    public class PlaneService : IPlaneService
    {
        private readonly IRepository<Plane> repository;

        public PlaneService(IRepository<Plane> repository )
        {
            this.repository = repository;
        }
        public string CreatePlane(Plane plane)
        {
            return this.repository.Insert(plane);
        }
        public string DeletePlane(int id)
        {
            return this.repository.Delete(id);
        }
        public IEnumerable<Plane> GetActivePlanes()
        {
            return this.repository.GetDefault(x => x.Status == Convert.ToBoolean(Status.Active));
        }

        public Plane GetPlaneById(int id)
        {
            return this.repository.GetById(id);
        }
        public IEnumerable<Plane> GetPlanes()
        {
            return this.repository.GetAll();
        }
        public string UpdatePlane(Plane plane)
        {
            return this.repository.Update(plane);
        }
    }
}
