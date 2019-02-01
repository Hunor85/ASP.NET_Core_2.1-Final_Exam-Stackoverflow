using System;
using System.Linq;
using DataLayer;
using DataLayer.Model;
using Microsoft.AspNetCore.Mvc;

namespace WebService.Controllers
{
    [Route("api/annotations")]
    [ApiController]
    public class AnnotationsController : Controller
    {
        private readonly IDataService _dataService;
        public AnnotationsController(DataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet("{userid}", Name = nameof(GetAnnotations))]
        public IActionResult GetAnnotations(int userid, int postid, int page = 0, int pageSize = 10)
        {
            var questions = _dataService.GetAnnotations(userid, page, pageSize)
                .Select(x => new
                {
                    Link = Url.Link(nameof(QuestionsController.GetQuestion), new { id = x.PostId }),
                    x.UserId,
                    x.Body,
                    x.CreationDate
                });
            var total = _dataService.GetNumberOfMarks();
            var pages = Math.Ceiling(total / (double)pageSize);
            var prev = page > 0 ? Url.Link(nameof(GetAnnotations), new { page = page - 1, pageSize }) : null;
            var next = page < pages - 1 ? Url.Link(nameof(GetAnnotations), new { page = page + 1, pageSize }) : null;

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

        /*[HttpGet]
        public IActionResult GetAnnotation()
        {
            var annotate = _dataService.GetAnnotation(userid);
            return Ok(annotate);
        }*/


        [HttpPost]
        public IActionResult PostAnnotations([FromBody]Annotations annotations)
        {
            _dataService.CreateAnnotation(annotations.Body, annotations.UserId, annotations.PostId);

            return Created($"api/annotations/{annotations}", annotations);
        }

        [HttpPut]
        public IActionResult UpdateAnnotation(string body ,[FromBody]Annotations annotations)
        {
           
                var anno = _dataService.UpdateAnnotation(annotations.Body, annotations.Id);
                if (anno == false)
                {
                    return NotFound();
                }
                return Ok(anno);
            
          
        }
        /*
        [HttpDelete]

        public IActionResult DeleteAnnotation(string body)
        {
            var delannotation = _dataService.DeleteAnnotation(body);
            if (delannotation==false)
            {
                return NotFound();
            }
            return Ok(delannotation);
        }
        */
    }
}