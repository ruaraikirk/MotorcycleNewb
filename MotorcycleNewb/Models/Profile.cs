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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string MotorcycleMake { get; set; }
        public string MotorcycleModel { get; set; }
        public virtual Image ProfileImage { get; set; }
        public virtual Image MotorcycleImage { get; set; }

        // TODO - add display names and required fields
    }
}