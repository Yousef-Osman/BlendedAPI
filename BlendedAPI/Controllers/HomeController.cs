using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlendedAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        [HttpGet("AfterAuth")]
        public IActionResult Get()
        {
            var nums = new List<int> { 1, 2, 3, 4, 5 };
            return Ok(nums);
        }

        [AllowAnonymous]
        [HttpGet("Numbers")]
        public IActionResult Nums()
        {
            var nums = new List<int> { 6, 7, 8 };
            return Ok(nums);
        }
    }
}
