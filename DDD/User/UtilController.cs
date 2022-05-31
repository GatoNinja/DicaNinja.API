
using BookSearch.API.Abstracts;
using BookSearch.API.Helpers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookSearch.API.DDD.User;

[AllowAnonymous]
[Route("[controller]")]
public class UtilController : ControllerHelper
{
    private IConfiguration Configuration { get; }

    public UtilController(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    [HttpGet("version")]
    public ActionResult GetVersion()
    {
        var version = Configuration["Info:Version"];
        var message = new MessageResponse(version);

        return Ok(message);
    }

    [HttpGet("unixtime")]
    public ActionResult GetUnixtime()
    {
        var unixTime = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
        var message = new MessageResponse(unixTime);

        return Ok(message);
    }
}