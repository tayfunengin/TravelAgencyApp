using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.HotelService
{
    public interface IHotelService
    {
        string CreateHotel(Hotel hotel, int[] accomodationTypeIds, int[] roomLocationIds, int[] roomTypeIds);

        string DeleteHotel(int id);

        Hotel GetHotelById(int id);
        IEnumerable<Hotel> GetHotels();

        bool IfHotelCodeExist(string hotelCode);

        string UpdateHotel(Hotel hotel);

        string UploadProfileImage(Hotel hotel, IFormFile formFile, string webRoot);

        List<AccomodationType> GetUnAssignedAccomodationTypes(int hotelId);
        List<RoomLocation> GetUnAssignedRoomLocations(int hotelId);
        List<RoomType> GetUnAssignedRoomTypes(int hotelId);

        void AddAccomodationTypeToHotel(int[] selectedIds, int hotelId);
        void AddRoomLocationToHotel(int[] selectedIds, int hotelId);
        void AddRoomTypeToHotel(int[] selectedIds, int hotelId);
        int GetLatestHotelId();

        string UploadImageForGallery(Photography photo, IFormFile formFile, int id, string webRoot);

    }
}
