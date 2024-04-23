using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;

namespace Restaurants.Infrastructure.Authorization.Requirements.MinimumAge;

internal class MinimumAgeRequirementHandler(ILogger<MinimumAgeRequirementHandler> logger, IUserContext userContext)
    : AuthorizationHandler<MinimumAgeRequirement>
{


    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("User {Email}, date of birth {DoB}: handling MinimumAgeRequirement", currentUser.Email, currentUser.DateOfBirth);

        if (currentUser.DateOfBirth is null)
        {
            logger.LogWarning("Authorization failed - User DateofBirth is null");
            context.Fail();
            return Task.CompletedTask;
        }

        if (currentUser.DateOfBirth!.Value.AddYears(requirement.MinimumAge) <= DateOnly.FromDateTime(DateTime.Today))
        {
            logger.LogInformation("Authorization succeeded");
            context.Succeed(requirement);
        }
        else
        {
            logger.LogWarning("Authorization failed");
            context.Fail();
        }
        return Task.CompletedTask;
    }


}
