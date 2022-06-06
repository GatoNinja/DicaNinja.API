
using DicaNinja.API.Abstracts;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DicaNinja.API.Controllers;

[Route("[controller]")]
[Authorize]
public class UploadController: ControllerHelper
{

    public UploadController(IWebHostEnvironment environment)
    {
        this.Environment = environment;
    }

    private IWebHostEnvironment Environment { get; }

    [HttpPost]
    public async Task<string> UploadFile(IFormFile file)
    {
        if (file.Length > 0)
        {
            try
            {
                var fullPath = Path.Combine(this.Environment.ContentRootPath, "images", base.UserId.ToString());
                var extension = Path.GetExtension(file.FileName);
                var newName = $"{Guid.NewGuid()}{extension}";

                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }
                using var filestream = System.IO.File.Create(Path.Combine(fullPath, newName));
                await file.CopyToAsync(filestream);
                filestream.Flush();
                
                return $"\\imagens\\{base.UserId}\\{newName}";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        else
        {
            return "Ocorreu uma falha no envio do arquivo...";
        }
    }
}
