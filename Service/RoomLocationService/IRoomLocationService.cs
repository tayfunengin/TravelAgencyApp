using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.RoomLocationService
{
    public interface IRoomLocationService
    {
        IEnumerable<RoomLocation> GetRoomLocationTypes();
        IEnumerable<RoomLocation> GetActiveRoomLocationTypes();
        RoomLocation GetRoomLocationById(int id);
        string CreateRoomLocation(RoomLocation roomLocation);
        string UpdateRoomLocation(RoomLocation roomLocation);
        string DeleteRoomLocation(int id);
        bool IfRoomLocationCodeExist(string roomLocationCode);
    }
}
