using System;
using System.Collections.Generic;
using System.Text;
//using Microsoft.AspNetCore.Identity;

namespace DataLayer.Model
{
    public class User
    {
        public  int Id { get; set; }
        public  string UserName{ get; set; }
        public DateTime CreationDate { get; set; }
        public string Password { get; set; }
        public List<SearchHistories> SearchHistory{ get; set; }
        public List<Mark> Marks { get; set; }
        public List<Annotations> Annotations { get; set; }

    }
}
