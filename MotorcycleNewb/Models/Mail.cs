using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MotorcycleNewb.Models
{
    public class Mail
    {
        [Key]
        public string Email { get; set; }
        public string FirstName { get; set; }
    }
}