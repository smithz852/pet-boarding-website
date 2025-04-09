using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZachsPetBoarding.Models
{
    public class BookingsModel
    {
        [Key]
        public Guid BookingID { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal  TotalCost { get; set; }
        public string BillingCode { get; set; }
        public DateTime BookingDateTime { get; set; }

        [Required]
        public KennelsModel Kennel { get; set; }

        [Required]
        public PetModel Pet { get; set; }

        
        public CheckInLogModel CheckInLog { get; set; }

        
        public CheckOutLogModel CheckOutLog { get; set; }

        public BookingsModel() 
        {
            BookingID = Guid.NewGuid();
        }
    }
}