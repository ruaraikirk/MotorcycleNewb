using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MotorcycleNewb.Models
{
    public class Profile
    {
        public int ProfileId { get; set; }
        public Guid AccountId { get; set; } // Corresponding dbo.AspNetUsers Id, links profile model to the correct application user
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Surname")]
        public string LastName { get; set; }
        public string City { get; set; }
        [Display(Name = "Motorcycle Make")]
        public string MotorcycleMake { get; set; }
        [Display(Name = "Motorcycle Model")]
        public string MotorcycleModel { get; set; }
        public virtual Image ProfileImage { get; set; }
        public virtual Image MotorcycleImage { get; set; }
    }
}