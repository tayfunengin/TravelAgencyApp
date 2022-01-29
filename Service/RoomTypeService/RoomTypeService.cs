using Domain.Entities;
using Domain.Enums;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.RoomTypeService
{
    public class RoomTypeService : IRoomTypeService
    {
        private readonly IRepository<RoomType> repository;

        public RoomTypeService(IRepository<RoomType> repository)
        {
            this.repository = repository;
        }
        public string CreateRoomType(RoomType roomType)
        {
            return this.repository.Insert(roomType);
        }
        public string DeleteRoomType(int id)
        {
            return this.repository.Delete(id);
        }
        public IEnumerable<RoomType> GetRoomTypes()
        {
            return this.repository.GetAll();
        }
        public RoomType GetRoomTypeById(int id)
        {
            return this.repository.GetById(id);
        }
        public string UpdateRoomType(RoomType roomType)
        {
           return this.repository.Update(roomType);
        }
        public bool IfRoomTypeCodeExist(string roomTypeCode)
        {
            RoomType roomType = this.repository.GetDefault(x => x.RoomTypeCode == roomTypeCode).FirstOrDefault();
            if (roomType == null)
                return false;
            else
                return true;            
        }

        public IEnumerable<RoomType> GetActiveRoomTypes()
        {
           return this.repository.GetDefault(x => x.Status == Convert.ToBoolean( Status.Active));
        }
    }
}
