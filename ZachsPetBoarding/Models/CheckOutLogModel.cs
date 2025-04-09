using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZachsPetBoarding.Models
{
    public class CheckOutLogModel
    {
        [Key]
        public Guid CheckOutTimeID {  get; set; }

        [Required]
        public EmployeesModel Employee { get; set; }
        public List<BookingsModel> Bookings { get; set; }

        public DateTime CheckOutDateTime { get; set; }

        public CheckOutLogModel() 
        {
            CheckOutTimeID = Guid.NewGuid();
        }
    }
}