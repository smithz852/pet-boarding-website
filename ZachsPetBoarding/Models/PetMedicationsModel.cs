using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZachsPetBoarding.Models
{
    public class PetMedicationsModel
    {
        [Key]
        public Guid MedicationID { get; set; }

        [Required]
        public string MedicationName { get; set; }
        
        public PetModel Pet { get; set; }

        public PetMedicationsModel()
        {
            MedicationID = Guid.NewGuid();
        }
    }
}