
using System.Globalization;

using DicaNinja.API.Abstracts;

using DicaNinja.API.Helpers;
using DicaNinja.API.Startup;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DicaNinja.API.Controllers;

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
        var version = Config.Info.Version;
        if (version is null)
        {
            throw new NullReferenceException();
        }

        var message = new MessageResponse(version);

        return Ok(message);
    }

    [HttpGet("unixtime")]
    public ActionResult GetUnixTime()
    {
        var unixTime = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString(CultureInfo.InvariantCulture);
        var message = new MessageResponse(unixTime);

        return Ok(message);
    }
}
