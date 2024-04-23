using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Infrastructure.Authorization.Requirements.CreatedMultipleRestaurants.Tests;

public class CreatedMultipleRestaurantsRequirementHandlerTests
{


    [Fact()]
    public async Task HandleRequirementAsync_UserHasCreatedMultipleRestaurants_ShouldSucceed()
    {

        // arrange

        //var loggerMok = new Mock<ILogger<CreatedMultipleRestaurantsRequirementHandler>>();

        var currentUser = new CurrentUser("1", "test@test.com", [], null, null);

        var userContextMock = new Mock<IUserContext>();
        userContextMock
            .Setup(mock => mock.GetCurrentUser())
            .Returns(currentUser);


        var restaurants = new List<Restaurant>()
        {
            new()
            {

                OwnerId = currentUser.Id
            },
             new()
            {

                OwnerId = currentUser.Id
            },
            new()
            {

                OwnerId = "2"
            }
        };

        var restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        restaurantsRepositoryMock
            .Setup(mock => mock.GetAllAsync())
            .ReturnsAsync(restaurants);

        var requirement = new CreatedMultipleRestaurantsRequirement(2);

        var requirementHandler = new CreatedMultipleRestaurantsRequirementHandler(restaurantsRepositoryMock.Object, userContextMock.Object);

        var context = new AuthorizationHandlerContext([requirement], null, null);


        // act
        await requirementHandler.HandleAsync(context);


        // assert
        context.HasSucceeded.Should().BeTrue();

    }


    [Fact()]
    public async Task HandleRequirementAsync_UserHasNotCreatedMultipleRestaurants_ShouldFail()
    {

        // arrange

        //var loggerMok = new Mock<ILogger<CreatedMultipleRestaurantsRequirementHandler>>();

        var currentUser = new CurrentUser("1", "test@test.com", [], null, null);

        var userContextMock = new Mock<IUserContext>();
        userContextMock
            .Setup(mock => mock.GetCurrentUser())
            .Returns(currentUser);


        var restaurants = new List<Restaurant>()
        {
            new()
            {

                OwnerId = currentUser.Id
            },
            new()
            {

                OwnerId = "2"
            }
        };

        var restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        restaurantsRepositoryMock
            .Setup(mock => mock.GetAllAsync())
            .ReturnsAsync(restaurants);

        var requirement = new CreatedMultipleRestaurantsRequirement(2);

        var requirementHandler = new CreatedMultipleRestaurantsRequirementHandler(restaurantsRepositoryMock.Object, userContextMock.Object);

        var context = new AuthorizationHandlerContext([requirement], null, null);


        // act
        await requirementHandler.HandleAsync(context);


        // assert
        context.HasFailed.Should().BeTrue();

    }


}