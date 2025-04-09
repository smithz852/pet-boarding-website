using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZachsPetBoarding.Models
{
    public class OwnersToPetsModel
    {
        [Key]
        public Guid OwnerToPetID { get; set; }

        [Required]
        public ApplicationUser Owner { get; set; }

        [Required]
        public PetModel Pet { get; set; }

        public OwnersToPetsModel() 
        {
            OwnerToPetID = Guid.NewGuid();
        }
    }
}