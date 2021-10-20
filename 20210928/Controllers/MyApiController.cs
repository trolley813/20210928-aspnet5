using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[ApiController]
[Route("[controller]/[action]")]
public class MyApiController : ControllerBase
{
    public IActionResult Index()
    {
        return Ok(new List<int>() { 1, 2, 3 });
    }

    public IActionResult Test1()
    {
        return Ok(new { A = 1, B = 2, C = 3 });
    }

    public IActionResult Test2()
    {
        return Ok(new Dictionary<int, int>{ {1, 501}, {2, 502}, {3, 503} });
    }
}