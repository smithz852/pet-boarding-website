using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ZachsPetBoarding.Models;

namespace ZachsPetBoarding.ViewModels
{
    public class PetsViewModel
    {
        [Required(ErrorMessage = "Pet name is required")]
        public string PetName { get; set; }
        [Required(ErrorMessage = "Pet type is required")]
        public string PetType { get; set; }
        public string PetBreed { get; set; }
        [Required(ErrorMessage = "Pet age is required")]
        public string PetAge { get; set; }
        [Required(ErrorMessage = "Feed times per day is required")]
        public string FeedingSchedule { get; set; }
        [Required(ErrorMessage = "Food amount per day is required")]
        public string FeedAmountInCups { get; set; }
        public List<string> Medications { get; set; } = new List<string>();

    }
}