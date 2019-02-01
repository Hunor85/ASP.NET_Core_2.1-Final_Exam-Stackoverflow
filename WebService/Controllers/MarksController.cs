using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Model;
using Microsoft.AspNetCore.Mvc;

namespace WebService.Controllers
{
    [Route("api/mark")]
    [ApiController]
    public class MarksController : Controller
    {
        private readonly IDataService _dataService;
        public MarksController(DataService dataService)
        {
            _dataService = dataService;
        }


        [HttpPost]
        public IActionResult PostMarks([FromBody]Mark mark)
        {

            _dataService.CreateMarking(mark.PostId, mark.UserId);

            return Created($"api/mark/{mark}", mark);

        }



     
        [HttpGet("{userid}", Name = nameof(GetMarks))]
        public IActionResult GetMarks(int userid, int postid, int page = 0, int pageSize = 10)
        {
            var questions = _dataService.GetMarks(userid, page, pageSize)
                .Select(x => new
                {
                    Link = Url.Link(nameof(QuestionsController.GetQuestion), new { id = x.PostId }),
                        x.UserId,
                        x.CreationDate



                });
            var total = _dataService.GetNumberOfMarks();
            var pages = Math.Ceiling(total / (double)pageSize);
            var prev = page > 0 ? Url.Link(nameof(GetMarks), new { page = page - 1, pageSize }) : null;
            var next = page < pages - 1 ? Url.Link(nameof(GetMarks), new { page = page + 1, pageSize }) : null;

            var result = new
            {
                total,
                pages,
                prev,
                next,
                items = questions
            };
            return Ok(result);
        }


        [HttpDelete]
        public IActionResult DeleteMarks([FromBody]Mark mark)
        {
            var del = _dataService.DeleteMarking(mark.UserId, mark.PostId);

            if (del==false) return NotFound();
            return Ok(del);
        }
            
           
          
    }

    


}