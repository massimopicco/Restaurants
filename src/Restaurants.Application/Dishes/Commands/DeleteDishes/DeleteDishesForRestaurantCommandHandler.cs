﻿using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.DeleteDishes;

public class DeleteDishesForRestaurantCommandHandler(ILogger<DeleteDishesForRestaurantCommandHandler> logger
    , IRestaurantsRepository restaurantsRepository
    , IDishesRepository dishesRepository
    , IRestauratAuthorizationService restauratAuthorizationService)
    : IRequestHandler<DeleteDishesForRestaurantCommand>
{

    public async Task Handle(DeleteDishesForRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogWarning("Removing all dishes from restaurant with Id: {RestaurantId}", request.RestaurantId);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId);
        if (restaurant is null)
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        if (!restauratAuthorizationService.Authorize(restaurant, ResourceOperation.Update))
            throw new ForbidException();

        await dishesRepository.Delete(restaurant.Dishes);
    }


}