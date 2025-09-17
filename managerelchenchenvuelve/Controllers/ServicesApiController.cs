using managerelchenchenvuelve.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
 
[ApiController]
[Route("api/[controller]")]
public class ServicesApiController : ControllerBase
{
    private readonly ToyoNoToyContext _context;
    private readonly ILogger<ToyoNoToyContext> _logger;

    public ServicesApiController(ToyoNoToyContext context, ILogger<ToyoNoToyContext> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("users")]
    public IActionResult GetUsers()
    {
        var usuarios = _context.Users
            .Select(u => new { u.Id, u.UserName })
            .ToList();

        return Ok(usuarios);
    }
}