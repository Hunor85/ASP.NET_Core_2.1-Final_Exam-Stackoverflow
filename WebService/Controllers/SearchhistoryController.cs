using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Model;
using Microsoft.AspNetCore.Mvc;

namespace WebService.Controllers
{
    [Route("api/searchhistory")]
    [ApiController]
    public class SearchhistoryController : Controller
    {
        private readonly IDataService _dataService;
        public SearchhistoryController(DataService dataService)
        {
            _dataService = dataService;
        }


        [HttpGet("{userid}")]
        public IActionResult GetSearchhistory(int userid, int postid, int page = 0, int pageSize = 5)
        {
            var searchh = _dataService.SearchHistories(userid);
            if (searchh == null) return NotFound();
            return Ok(searchh);
        }

        [HttpDelete("{userid}")]
        public IActionResult DeleteSearchhistory(int userid)
        {
            var Searchh = _dataService.SearchHistories(userid);
            if (Searchh == null) return NotFound();
            return Ok(Searchh);
        }

        [HttpPost("add")]
        public IActionResult CreateSearch( [FromBody] SearchHistories searchHistories)
        {
            var createSearch = _dataService.SaveSearch(searchHistories.Search, searchHistories.UserId);
            return Created($"api/searchhistory/{createSearch}", createSearch);

        }
    }
}