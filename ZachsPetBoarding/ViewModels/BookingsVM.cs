using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ZachsPetBoarding.Models;

namespace ZachsPetBoarding.ViewModels
{
    public class BookingsVM
    {
        [Required(ErrorMessage = "Please enter a first name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter a last name")]
        public string LastName { get; set; }
        public string PetName { get; set; }

        [Required(ErrorMessage = "Please choose a check-in date")]
        public DateTime CheckInDate { get; set; }
        [Required(ErrorMessage = "Please choose a check-out date")]
        public DateTime CheckOutDate { get; set; }

    }
}