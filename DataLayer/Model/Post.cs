using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Model
{
    
   public abstract class Post
    {
       
        public int Id { get; set; }

        public int PostType { get; set; }
        
       //public int ?ParentId { get; set; }
        
        public int ?AcceptedAnswerId { get; set; }

        public int Score { get; set; }
       
        public DateTime CreationDate { get; set; }
       
        public string Body { get; set; }

        public DateTime ?ClosedDate { get; set; }

        //[ForeignKey("Author")]
        public int AuthorId { get; set; }

        //public List<Comment> Comments { get; set; }
        public List<PostTag> PostTags { get; set; }
        

        public List<Annotations> Annotations { get; set; }

        public List<Mark> Marked { get; set; }




        
    }
}
