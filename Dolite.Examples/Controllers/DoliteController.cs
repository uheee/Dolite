using System.Security.Claims;
using Dolite.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dolite.Examples.Controllers;

/// <summary>
///     Dolite
/// </summary>
[ApiController]
[Route("[controller]")]
public class DoliteController : ControllerBase
{
    public IFreeSql FreeSql { get; init; } = null!;
    public ExceptionFactory ExceptionFactory { get; init; } = null!;

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
        if (errMessage is not null) throw ExceptionFactory.Business(1001, errMessage);
        var a = 1;
        var b = 0;
        return Ok(a / b);
    }

    /// <summary>
    ///     Get Token
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("token")]
    public ActionResult GetToken(string name)
    {
        var token = TokenManager.Generate(
            "user",
            TimeSpan.FromDays(3),
            new Claim("Hello", "World"));
        return Ok(token);
    }

    /// <summary>
    ///     Validate
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("validate")]
    [Authorize]
    public ActionResult Validate(string name)
    {
        return Ok($"Hello, {name}");
    }
}