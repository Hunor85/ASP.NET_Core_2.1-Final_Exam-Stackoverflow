using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Model
{
   public class Answer : Post
    {
        public int ParentId { get; set; }

        public List<Comment> Comments { get; set; }
        public List<PostLink> PostLinks { get; set; }

      
    }
}
