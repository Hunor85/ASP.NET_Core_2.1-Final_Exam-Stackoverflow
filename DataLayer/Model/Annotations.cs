using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Model
{
    public class Annotations
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime CreationDate { get; set; }

        [ForeignKey("Post")]
        public int PostId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }

        public Post Post { get; set; }

    }
}
