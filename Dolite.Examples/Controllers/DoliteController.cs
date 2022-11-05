using Microsoft.AspNetCore.Mvc;

namespace Dolite.Examples.Controllers;

/// <summary>
///     Dolite
/// </summary>
[ApiController]
[Route("[controller]")]
public class DoliteController : ControllerBase
{
    public IConfiguration Configuration { get; init; } = null!;

    /// <summary>
    ///     Greet
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public ActionResult Greet()
    {
        return Ok("Hello, Dolite!");
    }
}