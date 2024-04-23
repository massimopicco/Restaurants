using MediatR;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Dishes.Commands.DeleteDishes;

public class DeleteDishesForRestaurantCommand(int restaurantId) : IRequest
{


    public int RestaurantId { get; } = restaurantId;


}
