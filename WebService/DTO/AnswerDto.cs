using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.DTO
{
    public class AnswerDto 
    {
        public string Url { get; set; }
        public int Score { get; set; }
        public DateTime CreationDate { get; set; }
        public string Body { get; set; }
        public string UrlComment { get; set; }


    }
}
