﻿
using Restaurants.Domain.Exceptions;

namespace Restaurants.API.Middlewares;

public class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) : IMiddleware
{


    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (NotFoundException notFoundEx)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync(notFoundEx.Message);

            logger.LogWarning(notFoundEx.Message);
        }
        catch (ForbidException forbidEx)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Access forbidden");

            logger.LogWarning(forbidEx.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Something went wrong");
        }
    }


}
