using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Model
{
    public class PostLink
    {
        public int Id { get; set; }
        public int PostLinkId { get; set; }
        
        public Question Question { get; set; }
        public Answer Answer { get; set; }
    }
}

