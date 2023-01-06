
using DicaNinja.API.Abstracts;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DicaNinja.API.Controllers;

[Route("[controller]")]
[Authorize]
public class UploadController : ControllerHelper
{

    public UploadController(IWebHostEnvironment environment)
    {
        Environment = environment;
    }

    private IWebHostEnvironment Environment { get; }

    [HttpPost]
    public async Task<string> UploadFile(IFormFile file)
    {
        if (file is not null && file.Length > 0)
        {
            try
            {
                var fullPath = Path.Combine(Environment.ContentRootPath, "images", GetUserId().ToString());
                var extension = Path.GetExtension(file.FileName);
                var newName = $"{Guid.NewGuid()}{extension}";

                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }

                using (var filestream = System.IO.File.Create(Path.Combine(fullPath, newName)))
                {
                    await file.CopyToAsync(filestream).ConfigureAwait(false);
                    await filestream.FlushAsync().ConfigureAwait(false);
                }

                return $"\\imagens\\{GetUserId()}\\{newName}";
            }
            catch (FileLoadException ex)
            {
                return ex.ToString();
            }
        }

        return "Ocorreu uma falha no envio do arquivo...";
    }

}
