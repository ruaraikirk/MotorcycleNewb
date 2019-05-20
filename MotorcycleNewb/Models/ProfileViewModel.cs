using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MotorcycleNewb.Models
{
    public class ProfileViewModel
    {
        [Key]
        public Guid CurrAccId { get; set; } // Needed to avoid errors relating to entity not having a key attribute

        public Profile CurrentProfile { get; set; }

        public bool IsThisUser { get; set; }

        public string CurrentView { get; set; }

        public Profile User { get; set; }
    }
}