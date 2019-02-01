using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Model;
using Microsoft.AspNetCore.Mvc;

namespace WebService.Controllers
{
    [Route("api/wordCloud")]
    [ApiController]
    public class WordCloudController : Controller
    {
        private readonly IDataService _dataService;

        public WordCloudController(DataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet("{word}")]
        public IActionResult GetWordCloud(string word)
        {
            var clouds = _dataService.GetWordCloud(word);
            return Ok(clouds);
        }

       
    }
}