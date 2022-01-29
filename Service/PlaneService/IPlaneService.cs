using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.PlaneService
{
    public interface IPlaneService
    {
        IEnumerable<Plane> GetPlanes();
        Plane GetPlaneById(int id);
        string CreatePlane(Plane plane);
        string UpdatePlane(Plane plane);
        string DeletePlane(int id);
        IEnumerable<Plane> GetActivePlanes();
    }
}
