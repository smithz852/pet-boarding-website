using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZachsPetBoarding.Models
{
    public class EmployeesModel
    {
        [Key]
        public Guid EmployeeID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }    
        public string PhoneNum { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public List<CheckInLogModel> CheckInLog { get; set; }

        public List<CheckOutLogModel> CheckOutLog { get; set; }

        public EmployeesModel()
        {
            EmployeeID = Guid.NewGuid();
        }

    }
}