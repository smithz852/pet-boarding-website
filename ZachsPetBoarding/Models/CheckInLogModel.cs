using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZachsPetBoarding.Models
{
    public class CheckInLogModel
    {
        [Key]
        public Guid CheckInTimeID { get; set; }

        [Required]
        public EmployeesModel Employee { get; set; }
        public List<BookingsModel> Bookings { get; set; }

        public DateTime CheckInDateTime { get; set; }

        public CheckInLogModel() 
        {
            CheckInTimeID = Guid.NewGuid();
        }
    }
}