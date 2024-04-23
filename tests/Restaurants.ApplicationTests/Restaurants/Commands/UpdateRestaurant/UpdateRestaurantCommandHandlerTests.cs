using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using System.Xml.Linq;
using Xunit;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant.Tests;

public class UpdateRestaurantCommandHandlerTests
{


    private readonly Mock<ILogger<UpdateRestaurantCommandHandler>> _loggerMock;

    private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock;

    private readonly Mock<IMapper> _mapperMock;

    private readonly Mock<IRestauratAuthorizationService> _restaurantAuthorizationServiceMock;

    private readonly UpdateRestaurantCommandHandler _commandHandler;


    public UpdateRestaurantCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
        _restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        _mapperMock = new Mock<IMapper>();
        _restaurantAuthorizationServiceMock = new Mock<IRestauratAuthorizationService>();

        _commandHandler = new UpdateRestaurantCommandHandler(
            _loggerMock.Object, 
            _mapperMock.Object, 
            _restaurantsRepositoryMock.Object, 
            _restaurantAuthorizationServiceMock.Object);
    }


    [Fact()]
    public async Task Handle_ForNonExistingRestaurant_ShouldThrowNotFoundException()
    {

        // arrange

        var restaurantId = 1;

        var command = new UpdateRestaurantCommand()
        {
            Id = restaurantId,
            //Name = "New name",
            //Description = "New description",
            //HasDelivery = true
        };


        _restaurantsRepositoryMock
            .Setup(mock => mock.GetByIdAsync(command.Id))
            .ReturnsAsync((Restaurant?)null);


        // act
        Func<Task> act = async () => await _commandHandler.Handle(command, CancellationToken.None);

        // assert
        await act.Should().ThrowAsync<NotFoundException>();
    }


    [Fact()]
    public async Task Handle_ForUnauthorizedUser_ShouldThrowForbidException()
    {

        // arrange

        var restaurantId = 1;

        var command = new UpdateRestaurantCommand()
        {
            Id = restaurantId,
            //Name = "New name",
            //Description = "New description",
            //HasDelivery = true
        };

        var restaurant = new Restaurant()
        {
            //Id = restaurantId,
            //Name = "Old name",
            //Description = "Old description",
            //HasDelivery = false
        };

        _restaurantsRepositoryMock
            .Setup(mock => mock.GetByIdAsync(command.Id))
            .ReturnsAsync(restaurant);

        _restaurantAuthorizationServiceMock
            .Setup(mock => mock.Authorize(restaurant, ResourceOperation.Update))
            .Returns(false);

        // act
        Func<Task> act = async () => await _commandHandler.Handle(command, CancellationToken.None);

        // assert
        await act.Should().ThrowAsync<ForbidException>();
    }


    [Fact()]
    public async Task Handle_ForValidCommand_ShouldUpdateRestaurant()
    {

        // arrange

        var restaurantId = 1;

        var command = new UpdateRestaurantCommand()
        {
            Id = restaurantId,
            //Name = "New name",
            //Description = "New description",
            //HasDelivery = true
        };

        var restaurant = new Restaurant()
        {
            //Id = restaurantId,
            //Name = "Old name",
            //Description = "Old description",
            //HasDelivery = false
        };

        _restaurantsRepositoryMock
            .Setup(mock => mock.GetByIdAsync(command.Id))
            .ReturnsAsync(restaurant);

        _restaurantsRepositoryMock
            .Setup(mock => mock.SaveChanges());

        _restaurantAuthorizationServiceMock
            .Setup(mock => mock.Authorize(restaurant, ResourceOperation.Update))
            .Returns(true);


        // act
        await _commandHandler.Handle(command, CancellationToken.None);

        // assert
        _mapperMock.Verify(mock => mock.Map(command, restaurant), Times.Once);
        _restaurantsRepositoryMock.Verify(r => r.SaveChanges(), Times.Once());
    }


}