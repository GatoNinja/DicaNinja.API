using AutoMapper;

using DicaNinja.API.Abstracts;
using DicaNinja.API.Models;
using DicaNinja.API.Providers.Interfaces;
using DicaNinja.API.Request;

using Microsoft.AspNetCore.Mvc;

namespace DicaNinja.API.Controllers;

[Route("[controller]")]
[ApiController]
public class PersonController : ControllerHelper
{
    public PersonController(IMapper mapper, IPersonProvider personProvider, IUserProvider userProvider)
    {
        Mapper = mapper;
        PersonProvider = personProvider;
        UserProvider = userProvider;
    }

    private IMapper Mapper { get; }
    private IPersonProvider PersonProvider { get; }
    private IUserProvider UserProvider { get; }

    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody] PersonRequest request, CancellationToken cancellationToken)
    {
        var person = Mapper.Map<Person>(request);
        var updatedPerson = await PersonProvider.UpdatePersonAsync(UserId, person, cancellationToken);

        return updatedPerson is null ? NotFound() : Ok(Mapper.Map<DicaNinja.API.Response.PersonResponse>(updatedPerson));
    }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string query, CancellationToken cancellationToken)
    {
        var people = await UserProvider.SearchAsync(UserId, query, cancellationToken);

        return Ok(people);

    }
}
