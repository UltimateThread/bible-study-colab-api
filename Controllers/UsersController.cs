using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BibleStudyColabApi.Entities;
using BibleStudyColabApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BibleStudyColabApi.Controllers
{
   [Route("api/[controller]")]
   public class UsersController : ControllerBase
   {
      private IUserService _userService;

      public UsersController(IUserService userService)
      {
         _userService = userService;
      }

      [AllowAnonymous]
      [HttpPost("authenticate")]
      public IActionResult Authenticate([FromBody]User userParam)
      {
         var user = _userService.Authenticate(userParam.Username, userParam.Password);

         if (user == null)
            return BadRequest(new { message = "Username or password is incorrect" });

         return Ok(user);
      }

      [HttpGet]
      public IActionResult GetAll()
      {
         var users = _userService.GetAll();
         return Ok(users);
      }
   }
}
