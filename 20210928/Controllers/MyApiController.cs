using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace _20210928.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MyApiController : ControllerBase
    {
        /// <summary>
        /// Test action for API
        /// </summary>
        /// <returns>Always returns list of [1, 2, 3]</returns>
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(new List<int>() { 1, 2, 3 });
        }
        [HttpGet]
        public IActionResult Test1()
        {
            return Ok(new { A = 1, B = 2, C = 3 });
        }
        [HttpGet]
        public IActionResult Test2()
        {
            return Ok(new Dictionary<int, int> { { 1, 501 }, { 2, 502 }, { 3, 503 } });
        }

        /// <summary>
        /// Sums two given numbers
        /// </summary>
        /// <param name="x">A number</param>
        /// <param name="y">Other number</param>
        /// <returns>Sum of the numbers</returns>
        [HttpGet]
        public IActionResult Test3(int x, int y)
        {
            return Ok(new { X = x, Y = y, Z = x + y });
        }
    }
}