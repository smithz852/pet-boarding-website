using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ZachsPetBoarding.Models
{
    public class KennelsModel
    {
        [Key]
        public Guid KennelID { get; set; }

        public int KennelNumber { get; set; }
        public bool IsReserved { get; set; }
        public decimal Rate { get; set; }  

        public List<BookingsModel> Bookings { get; set; }

        public KennelsModel() 
        {
            KennelID = Guid.NewGuid();
            KennelNumber = 1;
        }
    }
}