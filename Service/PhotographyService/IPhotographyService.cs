using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.PhotographyService
{
    public interface IPhotographyService
    {
        Photography GetPhotography(int id);
    }
}
