using AutoMapper;

using DicaNinja.API.Abstracts;
using DicaNinja.API.Models;
using DicaNinja.API.Providers.Interfaces;
using DicaNinja.API.Request;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DicaNinja.API.Controllers;

[Route("[controller]")]
[ApiController]
public class PersonController : ControllerHelper
{
    public PersonController(IMapper mapper, IPersonProvider personProvider)
    {
        Mapper = mapper;
        PersonProvider = personProvider;
    }

    private IMapper Mapper { get; }
    private IPersonProvider PersonProvider { get; }

    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody] PersonRequest request)
    {
        var person = Mapper.Map<Person>(request);
        var updatedPerson = await PersonProvider.UpdatePersonAsync(UserId, person);
        
        return updatedPerson is null ? this.NotFound() : this.Ok(this.Mapper.Map<PersonResponse>(updatedPerson));
    }
}
