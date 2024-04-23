using Azure.Core;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Restaurants.APITests;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Restaurants.API.Controllers.Tests;

public class RestaurantsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{


    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock = new();


    public RestaurantsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder => 
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantsRepository), _ => _restaurantsRepositoryMock.Object));
            });
        });
    }


    [Fact()]
    public async Task GetAll_ForValidRequest_ShouldReturn200OK()
    {

        // arrange
        var client = _factory.CreateClient();


        // act
        var response = await client.GetAsync("/api/restaurants?pageNumber=1&pageSize=10");


        //assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

    }


    [Fact()]
    public async Task GetAll_ForInvalidRequest_ShouldReturn400BadRequest()
    {

        // arrange
        var client = _factory.CreateClient();


        // act
        var response = await client.GetAsync("/api/restaurants");


        //assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

    }


    [Fact()]
    public async Task GetById_ForNonExistingId_ShouldReturn404NotFound()
    {

        // arrange
        var id = 99;

        _restaurantsRepositoryMock
            .Setup(mock => mock.GetByIdAsync(id))
            .ReturnsAsync((Restaurant?)null);

        var client = _factory.CreateClient();


        // act
        var response = await client.GetAsync($"/api/restaurants/{id}");


        //assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

    }


    [Fact()]
    public async Task GetById_ForExistingId_ShouldReturn200Ok()
    {

        // arrange
        var id = 88;

        var restaurant = new Restaurant()
        {
            Id = id,
            Name = "Test",
            Description = "Test description"
        };

        _restaurantsRepositoryMock
            .Setup(mock => mock.GetByIdAsync(id))
            .ReturnsAsync(restaurant);

        var client = _factory.CreateClient();


        // act
        var response = await client.GetAsync($"/api/restaurants/{id}");
        var restaurantDto = await response.Content.ReadFromJsonAsync<RestaurantDto>(CancellationToken.None);


        //assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        restaurantDto.Should().NotBeNull();
        restaurantDto.Id.Should().Be(id);
        restaurantDto.Name.Should().Be(restaurant.Name);
        restaurantDto.Description.Should().Be(restaurant.Description);

    }


}