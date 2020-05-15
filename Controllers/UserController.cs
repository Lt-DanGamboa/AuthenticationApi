using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenticationApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet] 
        [Authorize(Policy = Policies.User)] 
        public IActionResult GetUserData() 
        { 
            return Ok("This is a response from user method"); 
        }
        [HttpGet] 
        [Authorize(Policy = Policies.Admin)] 
        public IActionResult GetAdminData() 
        { 
            return Ok("This is a response from Admin method"); 
        }
    }
}