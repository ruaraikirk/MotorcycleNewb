using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MotorcycleNewb.Models
{
    public class Profile
    {
        public int ProfileId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string City { get; set; }

        public Guid AccountId { get; set; } // Corresponding dbo.AspNetUsers Id
    }
}