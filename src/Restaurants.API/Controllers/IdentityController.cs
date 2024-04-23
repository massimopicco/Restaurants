using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Users.Commands.AssignUserRole;
using Restaurants.Application.Users.Commands.UnassignUserRole;
using Restaurants.Application.Users.Commands.UpdateUserDetails;
using Restaurants.Domain.Constants;

namespace Restaurants.API.Controllers;


[ApiController]
[Route("api/identity")]
public class IdentityController(IMediator mediator) : ControllerBase
{


    [Authorize]
    [HttpPatch("user")]
    public async Task<IActionResult> UpdateUserDetails(UpdateUserDetailsCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }


    [Authorize(Roles = UserRoles.Admin)]
    [HttpPost("userRole")]
    public async Task<IActionResult> AssignUserRoles(AssignUserRoleCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }


    [Authorize(Roles = UserRoles.Admin)]
    [HttpDelete("userRole")]
    public async Task<IActionResult> UnassignUserRoles(UnassignUserRoleCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }


}
