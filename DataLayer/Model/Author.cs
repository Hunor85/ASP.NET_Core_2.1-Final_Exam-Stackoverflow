using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Model
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string DisplayName { get; set; }
        public DateTime CreationDate { get; set; }
        public string Location { get; set; }
        public int ?Age { get; set; }

        public List<Post> Posts { get; set; }
        
        public List<Comment> Comments { get; set; }
    }
}
