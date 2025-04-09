using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZachsPetBoarding.Models
{
    public class FormSubmissionModel
    {
        [Key]
        public Guid FormID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string Phone { get; set; }

        [Required]
        public string Question { get; set; }

        public bool IsClosed { get; set; }

        public FormSubmissionModel() 
        {
            FormID = Guid.NewGuid();
            IsClosed = false;
        }
    }
}