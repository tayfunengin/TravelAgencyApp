using Domain.Entities;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;
using Repository.Context;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.IO;
using Domain.Enums;
using Service.AccomodationTypeService;
using Service.RoomTypeService;
using Service.RoomLocationService;
using System.Transactions;

namespace Service.HotelService
{
    public class HotelService : IHotelService
    {
        private readonly IRepository<Hotel> repository;
        private readonly IAccomodationTypeService accomodationTypeService;
        private readonly IRoomTypeService roomTypeService;
        private readonly IRoomLocationService roomLocationService;

        public HotelService(IRepository<Hotel> repository, IAccomodationTypeService accomodationTypeService, IRoomTypeService roomTypeService, IRoomLocationService roomLocationService)
        {
            this.repository = repository;
            this.accomodationTypeService = accomodationTypeService;
            this.roomTypeService = roomTypeService;
            this.roomLocationService = roomLocationService;
        }
        public string CreateHotel(Hotel hotel, int[] accomodationTypeIds, int[] roomLocationIds, int[] roomTypeIds)
        {
            using (var transactionScope = new TransactionScope() )
            {

                string result = this.repository.Insert(hotel);

                string type = result.Split(':')[0];
                var message = result.Split(':')[1];
                if (type == NotificationType.exception.ToString())         
                    return result;
                else
                {
                    int maxHotelId = GetLatestHotelId();
                    this.AddAccomodationTypeToHotel(accomodationTypeIds, maxHotelId);
                    this.AddRoomLocationToHotel(roomLocationIds, maxHotelId);
                    this.AddRoomTypeToHotel(roomTypeIds, maxHotelId);
                }

                transactionScope.Complete();
                return result;
            }
        }
        public string DeleteHotel(int id)
        {
           return this.repository.Delete(id);
        }

        public List<AccomodationType> GetUnAssignedAccomodationTypes(int hotelId)
        {
            List<AccomodationType> accomodationTypes = this.accomodationTypeService.GetActiveAccomodationTypes().ToList();
            Hotel hotel = this.GetHotelById(hotelId);
            List<AccomodationType> unAssginedAccomodationTypes = new List<AccomodationType>();
            if (hotel.AccomodationTypes.Count > 0)
            {
                foreach (var accomodationType in accomodationTypes)
                {
                    if (!hotel.AccomodationTypes.Contains(accomodationType))
                        unAssginedAccomodationTypes.Add(accomodationType);
                }
                return unAssginedAccomodationTypes;
            }
            else
            {
                return accomodationTypes;
            }
        }

        public Hotel GetHotelById(int id)
        {
            return this.repository.GetById(id);
        }
        public IEnumerable<Hotel> GetHotels()
        {
            return this.repository.GetAll();
        }
        public bool IfHotelCodeExist(string hotelCode)
        {
            Hotel hotel = this.repository.GetDefault(x => x.HotelCode == hotelCode).FirstOrDefault();
            if (hotel == null)
                return false;
            else
                return true;            
        }
        public string UpdateHotel(Hotel hotel)
        {
            return  this.repository.Update(hotel);
        }

        public string UploadProfileImage(Hotel hotel, IFormFile formFile, string webRoot)
        {

            var fileName = "";
            string serverPath = "~/images/hotelImage/";


            var uniqueName = Guid.NewGuid();
            serverPath = serverPath.Replace("~", "");
            var fileArray = formFile.FileName.Split('.');
            string extension = '.' + fileArray[fileArray.Length - 1].ToLower();
            fileName = uniqueName + extension;

            if (extension == ".png" || extension == ".jpg" || extension == ".jpeg" || extension == ".gif")
            {
                if (File.Exists(webRoot + serverPath + fileName))
                {
                    return NotificationType.error + ":" + "Photo ia already added.";
                }
                else
                {
                    try
                    {
                        var filePath = Path.GetDirectoryName(webRoot + serverPath + fileName);
                        var stream = new FileStream(Path.Combine(filePath, fileName), FileMode.Create);
                        formFile.CopyTo(stream);
                        hotel.ProfilePhotoPath = fileName;
                   
                        return NotificationType.success + ":" + "Photo has been added successfully.";
                    }
                    catch (Exception ex)
                    {
                        return NotificationType.exception + ":" + ex;
                    }
                }
            }
            else
            {
                return NotificationType.error + ":" + "Photo format is wrong!";
            }
        }

        public List<RoomLocation> GetUnAssignedRoomLocations(int hotelId)
        {
            List<RoomLocation> roomLocations = this.roomLocationService.GetActiveRoomLocationTypes().ToList();
            Hotel hotel = this.GetHotelById(hotelId);
            List<RoomLocation> unAssginedRoomLocations = new List<RoomLocation>();
            if (hotel.RoomLocations.Count > 0)
            {
                foreach (var roomLocation in roomLocations)
                {
                    if (!hotel.RoomLocations.Contains(roomLocation))
                        unAssginedRoomLocations.Add(roomLocation);
                }
                return unAssginedRoomLocations;
            }
            else
            {
                return roomLocations;
            }
        }

        public List<RoomType> GetUnAssignedRoomTypes(int hotelId)
        {
            List<RoomType> roomTypes = this.roomTypeService.GetActiveRoomTypes().ToList();
            Hotel hotel = this.GetHotelById(hotelId);
            List<RoomType> unAssginedRoomTypes = new List<RoomType>();
            if (hotel.RoomTypes.Count > 0)
            {
                foreach (var roomType in roomTypes)
                {
                    if (!hotel.RoomTypes.Contains(roomType))
                        unAssginedRoomTypes.Add(roomType);
                }
                return unAssginedRoomTypes;
            }
            else
            {
                return roomTypes;
            }
        }

        public void AddAccomodationTypeToHotel(int[] selectedIds, int hotelId)
        {
            Hotel hotelUpdated = this.GetHotelById(hotelId);
            if (hotelUpdated.AccomodationTypes == null)
            {
                hotelUpdated.AccomodationTypes = new List<AccomodationType>();
            }
            foreach (var accomodationTypeId in selectedIds)
            {
                AccomodationType accomodationType = accomodationTypeService.GetAccomodationTypeById(accomodationTypeId);
                hotelUpdated.AccomodationTypes.Add(accomodationType);
            }
        }

        public void AddRoomLocationToHotel(int[] selectedIds, int hotelId)
        {
            Hotel hotelUpdated = this.GetHotelById(hotelId);
            if (hotelUpdated.RoomLocations == null)
            {
                hotelUpdated.RoomLocations = new List<RoomLocation>();
            }
            foreach (var roomLocationId in selectedIds)
            {
                RoomLocation roomLocation = roomLocationService.GetRoomLocationById(roomLocationId);
                hotelUpdated.RoomLocations.Add(roomLocation);
            }
        }

        public void AddRoomTypeToHotel(int[] selectedIds, int hotelId)
        {
            Hotel hotelUpdated = this.GetHotelById(hotelId);
            if (hotelUpdated.RoomTypes == null)
            {
                hotelUpdated.RoomTypes = new List<RoomType>();
            }
            foreach (var roomTypeId in selectedIds)
            {
                RoomType roomType = roomTypeService.GetRoomTypeById(roomTypeId);
                hotelUpdated.RoomTypes.Add(roomType);
            }
        }

        public int GetLatestHotelId()
        {
           return this.repository.GetAll().Max(x=>x.Id);
        }

        public string UploadImageForGallery(Photography photo, IFormFile formFile, int id, string webRoot)
        {
            Hotel hotel = this.GetHotelById(id);
            var fileName = "";
            string serverPath = "~/images/hotelImage/";


            var uniqueName = Guid.NewGuid();
            serverPath = serverPath.Replace("~", "");
            var fileArray = formFile.FileName.Split('.');
            string extension = '.' + fileArray[fileArray.Length - 1].ToLower();
            fileName = uniqueName + extension;

            if (extension == ".png" || extension == ".jpg" || extension == ".jpeg" || extension == ".gif")
            {
                if (File.Exists(webRoot + serverPath + fileName))
                {
                    return NotificationType.error + ":" + "Photo ia already added.";
                }
                else
                {
                    try
                    {
                        var filePath = Path.GetDirectoryName(webRoot + serverPath + fileName);
                        var stream = new FileStream(Path.Combine(filePath, fileName), FileMode.Create);
                        formFile.CopyTo(stream);

                        Photography photography = new Photography();
                        photography.Title = photo.Title;
                        photography.Path = fileName;
                        hotel.Photographies.Add(photography);                    
                        
                        return NotificationType.success + ":" + "Photo has been added successfully.";
                    }
                    catch (Exception ex)
                    {
                        return NotificationType.exception + ":" + ex;
                    }
                }
            }
            else
            {
                return NotificationType.error + ":" + "Photo format is wrong!";
            }
        }
    }
}
