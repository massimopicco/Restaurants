using FluentValidation;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryValidator : AbstractValidator<GetAllRestaurantsQuery>
{


    private int[] allowedPageSizes = [ 5, 10, 15, 30 ];
    private string[] allowedSortByColumns = [ nameof(Restaurant.Name), nameof(Restaurant.Description), nameof(Restaurant.Category) ];

    public GetAllRestaurantsQueryValidator()
    {

        RuleFor(r => r.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(r => r.PageSize)
            .Must(value => allowedPageSizes.Contains(value))
            .WithMessage($"Page size must be in [{string.Join(',', allowedPageSizes)}]");

        RuleFor(r => r.SortBy)
            .Must(value => allowedSortByColumns.Contains(value))
            .When(q => q.SortBy is not null)
            .WithMessage($"Sort by is optional, or must be in [{string.Join(',', allowedSortByColumns)}]");

    }


}
