using Domain.Entities;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.PhotographyService
{
    public class PhotographyService : IPhotographyService
    {
        private readonly IRepository<Photography> repository;

        public PhotographyService(IRepository<Photography> repository)
        {
            this.repository = repository;
        }
        public Photography GetPhotography(int id)
        {
            return this.repository.GetById(id);
        }
    }
}
