
using DicaNinja.API.Abstracts;

using DicaNinja.API.Helpers;
using DicaNinja.API.Services;
using DicaNinja.API.Startup;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DicaNinja.API.Controllers;

[AllowAnonymous]
[Route("[controller]")]
public class UtilController : ControllerHelper
{
    private ConfigurationReader Config { get; }

    private SmtpService SmtpService { get; }

    public UtilController(ConfigurationReader config, SmtpService smtpService)
    {
        Config = config;
        SmtpService = smtpService;
    }

    [HttpGet("version")]
    public ActionResult GetVersion()
    {
        var message = new MessageResponse(Config.Info.Version);

        return Ok(message);
    }

    [HttpGet("unixtime")]
    public ActionResult GetUnixTime()
    {
        var unixTime = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
        var message = new MessageResponse(unixTime);

        return Ok(message);
    }

    [HttpGet("email_test")]
    public void EmailTest()
    {
        SmtpService.SendEmail("ygor@ygorlazaro.com", "", "");
    }
    
}
