using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZachsPetBoarding.Models
{
    public class PetModel
    {
        [Key]
        public Guid PetID { get; set; }

        [Required]
        public string PetName { get; set; }
        public string PetType { get; set; }
        public string PetBreed { get; set; }
        public string PetAge { get; set; }
        public string FeedingSchedule {  get; set; }
        public string FeedAmountInCups { get; set; }

        public List<PetMedicationsModel> Medications { get; set; }
        public List<BookingsModel> Bookings { get; set; }
        public List<OwnersToPetsModel> Owners { get; set; }

        public PetModel()
        {
            PetID = Guid.NewGuid();
        }
    }
}