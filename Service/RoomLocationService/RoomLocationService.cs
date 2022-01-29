using Domain.Entities;
using Domain.Enums;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.RoomLocationService
{
    public class RoomLocationService : IRoomLocationService
    {
        private readonly Repository.IRepository<RoomLocation> repository;

        public RoomLocationService(IRepository<RoomLocation> repository)
        {
            this.repository = repository;
        }
        public string CreateRoomLocation(RoomLocation roomLocation)
        {
           return this.repository.Insert(roomLocation);
        }
        public string DeleteRoomLocation(int id)
        {
            return this.repository.Delete(id);
        }
        public IEnumerable<RoomLocation> GetRoomLocationTypes()
        {
            return this.repository.GetAll();
        }
        public RoomLocation GetRoomLocationById(int id)
        {
            return this.repository.GetById(id);
        }
        public string UpdateRoomLocation(RoomLocation roomLocation)
        {
            return this.repository.Update(roomLocation);
        }
        public bool IfRoomLocationCodeExist(string roomLocationCode)
        {
            RoomLocation roomLocation = this.repository.GetDefault(x => x.RoomLocationCode == roomLocationCode).FirstOrDefault();
            if (roomLocation == null)
                return false;
            else
                return true;         
        }

        public IEnumerable<RoomLocation> GetActiveRoomLocationTypes()
        {
            return this.repository.GetDefault(x => x.Status == Convert.ToBoolean(Status.Active));
        }
    }
}
