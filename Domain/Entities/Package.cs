using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class Package : BaseEntity
    {        

        [Required]
        public string PackageName { get; set; }
        [Required]
        public int RegionId { get; set; }

        public virtual Region Region { get; set; }
        [Required]
        public Period Period { get; set; }

        [Required]
        public string Year { get; set; }
        [Required]
        public DateTime CheckInDate { get; set; }
        [Required]
        public DateTime CheckOutDate { get; set; }

        public byte NightCount
        {
            get
            {
                int days = (CheckOutDate.Date - CheckInDate.Date).Days;
                return Convert.ToByte(days);
            }
        }

        [Required]
        public byte GuestCount { get; set; }

        [Required]
        public byte Quota { get; set; }

        public DateTime? RentACarStartDate { get; set; }
        public DateTime? RentACarEndDate { get; set; }

        public byte? RentACarDayCount
        {
            get
            {
                if (RentACarStartDate != null && RentACarEndDate != null)
                {
                    int days = (RentACarEndDate.Value.Date - RentACarStartDate.Value.Date).Days;
                    if (days == 0)
                    {
                        days = 1;
                    }
                    return Convert.ToByte(days);                
                }

                else
                {
                    return null;
                }

            }
        }

        public DateTime? TourGuideRentDay { get; set; }
        public int? HotelPriceId { get; set; }
        public virtual HotelPrice HotelPrice { get; set; }

        public virtual List<TourOperatorPrice> TourPrices { get; set; }

        public virtual List<TourDateInPackage> TourDates { get; set; }
        public int? RentACarPriceId { get; set; }
        public virtual RentACarPrice RentACarPrice { get; set; }

        public virtual List<FlightPrice> FlightPrices { get; set; }

        public virtual List<TransportationPrice> TransportationPrices { get; set; }

        public int? TourGuidePriceId { get; set; }
        public virtual TourGuidePrice TourGuidePrice { get; set; }

        public decimal Total
        {
            get
            {
                decimal totalPrice = 0;
                if (HotelPrice != null)
                    totalPrice = HotelPrice.SalePrice * NightCount;

                if (TourPrices != null)
                {
                    foreach (var tourPrice in TourPrices)
                    {
                        totalPrice += tourPrice.SalePrice * GuestCount;
                    }
                }

                if (RentACarPrice != null)
                {
                    if (RentACarDayCount != null)
                    {
                        totalPrice += RentACarPrice.SalePrice * RentACarDayCount.Value;
                    }
                }                   
                    

                if (FlightPrices != null)
                {
                    foreach (var flightPrice in FlightPrices)
                    {
                        totalPrice += flightPrice.SalePrice * GuestCount;
                    }
                }

                if (TransportationPrices != null)
                {
                    foreach (var transportationPrice in TransportationPrices)
                    {
                        totalPrice += transportationPrice.SalePrice * GuestCount;
                    }
                }

                if (TourGuidePriceId != null)
                    totalPrice += TourGuidePrice.SalePrice;

                return totalPrice;
            }
        }
        public virtual List<Sales> Sales { get; set; }
    }
}
