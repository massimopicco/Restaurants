using AutoMapper;
using FluentAssertions;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Domain.Entities;
using Xunit;

namespace Restaurants.Application.Restaurants.Dtos.Tests;

public class RestaurantsProfileTests
{


    private IMapper _mapper;


    public RestaurantsProfileTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<RestaurantsProfile>();
        });

        _mapper = configuration.CreateMapper();
    }



    [Fact()]
    public void CreateMap_FromRestaurantToRestaurantDto_MapsCorrectly()
    {

        // arrange

        var restaurant = new Restaurant()
        {
            Id = 1,
            Name = "Test restaurant",
            Description = "Test description",
            Category = "Test category",
            HasDelivery = true,
            ContactEmail = "test@test.com",
            ContactNumber = "123456789",
            Address = new Address()
            {
                City = "Test city",
                Street = "Test street",
                PostalCode = "12-345"
            }
        };


        // act
        var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);


        // assert
        restaurantDto.Should().NotBeNull();
        restaurantDto.Id.Should().Be(restaurant.Id);
        restaurantDto.Name.Should().Be(restaurant.Name);
        restaurantDto.Description.Should().Be(restaurant.Description);
        restaurantDto.Category.Should().Be(restaurant.Category);
        restaurantDto.HasDelivery.Should().Be(restaurant.HasDelivery);
        restaurantDto.City.Should().Be(restaurant.Address.City);
        restaurantDto.Street.Should().Be(restaurant.Address.Street);
        restaurantDto.PostalCode.Should().Be(restaurant.Address.PostalCode);

    }


    [Fact()]
    public void CreateMap_FromCreateRestaurantCommandToRestaurant_MapsCorrectly()
    {

        // arrange
        var command = new CreateRestaurantCommand()
        {
            Name = "Test restaurant",
            Description = "Test description",
            Category = "Test category",
            HasDelivery = true,
            ContactEmail = "test@test.com",
            ContactNumber = "123456789",
            City = "Test city",
            Street = "Test street",
            PostalCode = "12-345"
        };


        // act
        var restaurant = _mapper.Map<Restaurant>(command);


        // assert
        restaurant.Should().NotBeNull();
        restaurant.Name.Should().Be(command.Name);
        restaurant.Description.Should().Be(command.Description);
        restaurant.Category.Should().Be(command.Category);
        restaurant.HasDelivery.Should().Be(command.HasDelivery);
        restaurant.Address.Should().NotBeNull();
        restaurant.Address.City.Should().Be(command.City);
        restaurant.Address.Street.Should().Be(command.Street);
        restaurant.Address.PostalCode.Should().Be(command.PostalCode);

    }


    [Fact()]
    public void CreateMap_FromUpdateRestaurantCommandToRestaurant_MapsCorrectly()
    {

        // arrange
        var command = new UpdateRestaurantCommand()
        {
            Id = 1,
            Name = "Test restaurant",
            Description = "Test description",
            HasDelivery = true
        };


        // act
        var restaurant = _mapper.Map<Restaurant>(command);


        // assert
        restaurant.Should().NotBeNull();
        restaurant.Id.Should().Be(command.Id);
        restaurant.Name.Should().Be(command.Name);
        restaurant.Description.Should().Be(command.Description);
        restaurant.HasDelivery.Should().Be(command.HasDelivery);

    }

}