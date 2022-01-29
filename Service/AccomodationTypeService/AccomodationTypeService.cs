using Domain.Entities;
using Domain.Enums;
using Repository;
using Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Service.AccomodationTypeService
{
    public class AccomodationTypeService : IAccomodationTypeService
    {
        private readonly IRepository<AccomodationType> repository;        

        public AccomodationTypeService(IRepository<AccomodationType> repository )
        {
            this.repository = repository;        
        }

        public string CreateAccomodationType(AccomodationType accomodationType)
        {            
              return this.repository.Insert(accomodationType);           
        }

        public string DeleteAccomodationType(int id)
        {
           return this.repository.Delete(id);
        }
  
        public AccomodationType GetAccomodationTypeById(int id)
        {
            return this.repository.GetById(id);
        }

        public IEnumerable<AccomodationType> GetAccomodationTypes()
        {
           return this.repository.GetAll();
        }

        public IEnumerable<AccomodationType> GetActiveAccomodationTypes()
        {
            return this.repository.GetDefault(x => x.Status == Convert.ToBoolean(Status.Active));
        }

        public bool IfAccomodationTypeCodeExist(string accomodationTypeCode)
        {
            AccomodationType accomodationType = this.repository.GetDefault(x => x.AccomodationCode == accomodationTypeCode).FirstOrDefault();
            if (accomodationType == null)
                return false;
            else
                return true;            
        }

        public string UpdateAccomodationType(AccomodationType accomodationType)
        {
           return this.repository.Update(accomodationType);
        }
    }
}
