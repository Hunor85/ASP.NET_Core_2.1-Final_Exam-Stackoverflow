using DataLayer;
using DataLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebService.Services;

namespace WebService.Controllers
{
    //[Authorize]

    // URL address
    [Route("api/users")]

    // API attribute
    [ApiController]
    public class UsersController : Controller
    {
        // Instance of DataService
        private readonly IDataService _dataService;

        private IUserService _userService;

        public UsersController(DataService dataService, IUserService userService)
        {
            _dataService = dataService;
            _userService = userService;
        }


        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]User userParam)
        {
            var user = await _userService.Authenticate(userParam.UserName, userParam.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}", Name = nameof(GetUser))]
        public IActionResult GetUser(int id)
        {
            var user = _dataService.GetUser(id);
            if(user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] User user)

        {
            _dataService.CreateUser(user.UserName, user.Password);
            return Created($"api/users/{user}", user);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User user)
        {
           var update =  _dataService.UpdateUser(id, user.UserName, user.Password);
            if (update == false) return NotFound();
            return Ok(update);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var delete = _dataService.DeleteUser(id);
            if (delete == false) return NotFound();
            return Ok(delete);

        }

        public IActionResult Index()
        {
            return View();
        }

    }
}
