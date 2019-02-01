using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Model
{
    public class SearchHistories
    {
        public string Search { get; set; }

        public int UserId { get; set; }
        public DateTime Date { get; set; }

        public User User { get; set; }
    }
}
