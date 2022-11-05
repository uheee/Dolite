using AutoMapper;
using Dolite.Components;
using Microsoft.AspNetCore.Mvc;

namespace Dolite.Examples.Controllers;

/// <summary>
///     Dolite
/// </summary>
[ApiController]
[Route("[controller]")]
public class DoliteController : ControllerBase
{
    public IMapper Mapper { get; init; } = null!;
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

    /// <summary>
    ///     Test Error Handling
    /// </summary>
    /// <param name="errMessage"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("error")]
    public ActionResult Error(string? errMessage)
    {
        if (errMessage is not null) throw new BusinessException(101, errMessage);
        var a = 1;
        var b = 0;
        return Ok(a / b);
    }
}