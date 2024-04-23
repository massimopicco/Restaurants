﻿namespace Restaurants.Infrastructure.Authorization;

public static class AppClaimTypes
{


    public const string Nationality = "Nationality";

    public const string DateOfBirth = "DateOfBirth";


}


public static class PolicyNames
{


    public const string HasNationality = "HasNationality";

    public const string AtLeast20 = "AtLeast20";

    public const string CreatedAtLeast2Restaurants = "CreatedAtLeast2Restaurants";


}