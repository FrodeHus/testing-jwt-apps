using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SampleApp.Controllers;

[ApiController]
[Route("[controller]")]
public class SampleController : ControllerBase
{
    [Authorize("AllRegisteredUsers")]
    [HttpGet]
    [Route("/")]
    public IActionResult Get()
    {
        return Ok("Hello World!");
    }

    [Authorize("AdminOnly")]
    [HttpGet]
    [Route("/admin")]
    public IActionResult GetAdmin()
    {
        return Ok("Hello Admin!");
    }
}
