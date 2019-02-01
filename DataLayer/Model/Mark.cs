using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLayer.Model
{
     public class Mark
    {
        
        public DateTime CreationDate { get; set; }

        public int PostId { get; set; }

        public int UserId { get; set; }

       public Post Posts { get; set; }
        public User User { get; set; }
    }
}
