using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MotorcycleNewb.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public string PostContent { get; set; }
        public DateTime Timestamp { get; set; }
        
        // Integer property will act as foreign keys
        public int? ProfileId { get; set; }

        public virtual Profile Profile { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}