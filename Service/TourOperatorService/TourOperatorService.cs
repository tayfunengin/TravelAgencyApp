using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.TourOperatorService
{
    public class TourOperatorService : ITourOperatorService
    {
        private readonly IRepository<TourOperator> repository;       
        public TourOperatorService(IRepository<TourOperator> repository)
        {
            this.repository = repository;       
        }
        public string CreateTourOperator(TourOperator tourOperator)
        {
            return this.repository.Insert(tourOperator);
        }
        public string DeleteTourOperator(int id)
        {
            return this.repository.Delete(id);
        }
        public TourOperator GetTourOperatorById(int id)
        {
            return this.repository.GetById(id);
        }
        public IEnumerable<TourOperator> GetTourOperators()
        {
            return this.repository.GetAll();
        }
        public bool IfTourOperatorCodeExist(string tourOperatorCode)
        {           
            TourOperator tourOperator = this.repository.GetDefault(x => x.TourOperatorCode == tourOperatorCode).FirstOrDefault();
            if (tourOperator == null)
                return false;
            else
                return true;
        }    
        public string RemoveTour(TourOperator tourOperator, Tour tour)
        {
            string result = "";
            try
            {               
                tourOperator.Tours.Remove(tour);
                result = NotificationType.success + ":" + "Tour has been removed successfully.";
            }
            catch (Exception ex)
            {
                result = NotificationType.exception + ":" + ex.Message;
            }

            return result;
        }
        public string UpdateTourOperator(TourOperator tourOperator)
        {            
            return this.repository.Update(tourOperator);
        }
    }
}
