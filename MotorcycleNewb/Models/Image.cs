using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MotorcycleNewb.Models
{
    public class Image
    {
        public int ImageId { get; set; }
        public byte[] Content { get; set; }
        private string filename;
        public string FileName
        {
            get
            {
                return filename;
            }
            set
            {
                if (Content != null && Content.Length > 0)
                {
                    filename = String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(Content));
                }
                else
                {
                    filename = value;
                }
            }
        }
    }
}