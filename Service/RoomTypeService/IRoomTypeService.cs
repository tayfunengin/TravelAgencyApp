using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.RoomTypeService
{
    public interface IRoomTypeService
    {
        IEnumerable<RoomType> GetRoomTypes();
        IEnumerable<RoomType> GetActiveRoomTypes();
        RoomType GetRoomTypeById(int id);
        string CreateRoomType(RoomType roomType);
        string UpdateRoomType(RoomType roomType);
        string DeleteRoomType(int id);
        bool IfRoomTypeCodeExist(string roomTypeCode);
    }
}
