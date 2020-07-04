using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BlendedAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        public IActionResult Get()
        {
            var nums = new List<int> { 1, 2, 3, 4, 5 };
            return Ok(nums);
        }
    }
}
