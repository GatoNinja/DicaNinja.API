using BookSearch.API.Abstracts;
using BookSearch.API.Helpers;
using BookSearch.API.Startup;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookSearch.API.Controller;

[AllowAnonymous]
[Route("[controller]")]
public class UtilController : ControllerHelper
{
    private ConfigurationReader Config { get; }

    public UtilController(ConfigurationReader config)
    {
        Config = config;
    }

    [HttpGet("version")]
    public ActionResult GetVersion()
    {
        var message = new MessageResponse(Config.Info.Version);

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
