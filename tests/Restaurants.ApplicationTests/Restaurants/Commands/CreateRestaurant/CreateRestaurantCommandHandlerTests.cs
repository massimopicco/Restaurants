using AutoMapper;
using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant.Tests;

public class CreateRestaurantCommandHandlerTests
{


    [Fact()]
    public async Task Handle_ForValidCommand_ReturnsCreatedRestaurantId()
    {

        // arrage
        var loggerMock = new Mock<ILogger<CreateRestaurantCommandHandler>>();


        var currentUser = new CurrentUser("1", "test@test.com", [], null, null);
        var userContextMock = new Mock<IUserContext>();

        userContextMock
            .Setup(mock => mock.GetCurrentUser())
            .Returns(currentUser);


        var command = new CreateRestaurantCommand();
        var restaurant = new Restaurant();

        var mapperMock = new Mock<IMapper>();
        mapperMock
            .Setup(mock => mock.Map<Restaurant>(command))
            .Returns(restaurant);


        var restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        restaurantsRepositoryMock
            .Setup(mock => mock.Create(It.IsAny<Restaurant>()))
            .ReturnsAsync(1);
        
        var commandHandler = new CreateRestaurantCommandHandler(loggerMock.Object, userContextMock.Object, mapperMock.Object, restaurantsRepositoryMock.Object);


        // act
        int result = await commandHandler.Handle(command, CancellationToken.None);

        // assert
        result.Should().Be(1);
        restaurant.OwnerId.Should().Be(currentUser.Id);
        restaurantsRepositoryMock.Verify(r => r.Create(restaurant), Times.Once());

    }


}