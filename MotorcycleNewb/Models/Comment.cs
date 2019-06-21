using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MotorcycleNewb.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string CommentContent { get; set; }
        public DateTime Timestamp { get; set; }

        // Integer properties will act as foreign keys
        public int? ProfileId { get; set; }
        public int? PostId { get; set; }

        public virtual Profile Profile { get; set; }
        public virtual Post Post { get; set; }
    }
}