using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infrastructure.Authorization.Requirements.CreatedMultipleRestaurants;

internal class CreatedMultipleRestaurantsRequirementHandler(//ILogger<CreatedMultipleRestaurantsRequirementHandler> logger,
    IRestaurantsRepository restaurantsRepository
    , IUserContext userContext)
    : AuthorizationHandler<CreatedMultipleRestaurantsRequirement>
{


    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CreatedMultipleRestaurantsRequirement requirement)
    {
        var currentUser = userContext.GetCurrentUser();
        //logger.LogInformation("User {Email}: handling MinimumOwnedRestaurantsRequirement", currentUser.Email);

        var restaurants = await restaurantsRepository.GetAllAsync();

        var userRestaurantsCreated = restaurants.Count(r => r.OwnerId == currentUser.Id);

        if (userRestaurantsCreated >= requirement.MinimumRestaurantsCreated)
        {
            //logger.LogInformation("Authorization succeeded");
            context.Succeed(requirement);
        }
        else
        {
            //logger.LogWarning("Authorization failed");
            context.Fail();
        }
    }


}
