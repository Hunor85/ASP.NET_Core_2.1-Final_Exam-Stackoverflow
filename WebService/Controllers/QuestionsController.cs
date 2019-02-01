using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Model;
using Microsoft.AspNetCore.Mvc;
using WebService.DTO;

namespace WebService.Controllers
{
    [Route("api/questions")]
    [ApiController]
    public class QuestionsController : Controller
    {
        private readonly IDataService _dataService;
        public QuestionsController(DataService dataService)
        {
            _dataService = dataService;
        }
        
        [HttpGet(Name =nameof(GetQuestions))]
        public IActionResult GetQuestions(int page = 0, int pageSize = 10)
        {
            var questions = _dataService.GetQuestions(page, pageSize)
                .Select(x => new
                {
                    Link =Url.Link(
                        nameof(GetQuestion),
                        new {x.Id}),
                        x.Title
                    
                });
            var total = _dataService.GetNumberOfQuestions();
            var pages = Math.Ceiling(total / (double)pageSize);
            var cur = Url.Link(nameof(GetQuestions), new { page, pageSize });
            var prev = page > 0 ? Url.Link(nameof(GetQuestions), new { page = page - 1, pageSize }) : null;
            var next = page < pages - 1 ? Url.Link(nameof(GetQuestions), new { page = page + 1, pageSize }) : null;

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
        
        [HttpGet("{id}", Name = nameof(GetQuestion))]
        public IActionResult GetQuestion(int id)
        {
            var question = _dataService.GetQuestion(id);
            if (question == null) return NotFound();
            
            
                var result = new
                {
                    Link = Url.Link(nameof(GetQuestion), new { question.Id }),
                    question.Title,
                    question.CreationDate,
                    question.Score,
                    question.Body,
                    AcceptedAnswer = Url.Link(nameof(AnswersController.GetAnswer)
                    , new {id = question.AcceptedAnswerId }),
                 
                    Answers = Url.Link(nameof(AnswersController.GetAnswersByParent)
                    , new {id = question.Id}),
                    Comments = Url.Link(nameof(GetQuestionComment), new { id = question.Id}),
                    PostLink = Url.Link(nameof(GetQuestionPostLinks), new { id = question.Id})
                    
                };
                return Ok(result);
    
        }

        [HttpGet("links/{id}", Name =nameof(GetQuestionPostLinks))]
        public IActionResult GetQuestionPostLinks(int id)
        {
            var questionpostlinks = _dataService.GetPostLinksByQuestion(id);               
            if (questionpostlinks.Count == 0) return NotFound();

            List<PostLinkDto> postLinks = new List<PostLinkDto>();
            foreach(var item in questionpostlinks)
            {
                var linklist = new PostLinkDto()
                {
                    LinkedUrl = Url.Link(nameof(GetQuestion), new { id = item.PostLinkId })
                };
                postLinks.Add(linklist);
            }
           
            return Ok(postLinks);
        }

        [HttpGet("comments/{id}", Name = nameof(GetQuestionComment))]
        public IActionResult GetQuestionComment(int id)
        {
            var questioncomments = _dataService.GetQuestionComments(id).Select(x => new
            {
                Link = Url.Link(
                        nameof(GetQuestion),
                        new { x.Id }),
                   Score = x.Score,
                   CreationDate = x.CreationDate,
                   Body = x.Body

            });

            var result = new
            {
                items = questioncomments
            };

            return Ok(result);



        }

       
        [HttpGet("name/{name}", Name= nameof(GetQuestionByName))]
        public IActionResult GetQuestionByName(string name, int page = 0, int pageSize = 10)
        {//need to show the same amount o info as get posts
            var question = _dataService.GetQuestionsByString(name, page, pageSize)
                 .Select(x => new
                 {
                     Link = Url.Link(
                        nameof(GetQuestion),
                        new { x.Id }),
                        x.Title,
                        x.Body

                 }); ;
            var numberOfItems = _dataService.GetNumberOfQuestions();
            var terms = name;
            var total = _dataService.GetNumberOfQuestions();
            var pages = Math.Ceiling(total / (double)pageSize);
            var cur = Url.Link(nameof(GetQuestionByName), new { page, pageSize });
            var prev = page > 0 ? Url.Link(nameof(GetQuestionByName), new { page = page - 1, pageSize }) : null;
            var next = page < pages - 1 ? Url.Link(nameof(GetQuestionByName), new { page = page + 1, pageSize }) : null;

            if (question == null) return NotFound();

            var result = new
            {
                total,
                pages,
                prev,
                next,
                terms,
                items = question
            };
            return Ok(result);


        }
    }
  
}