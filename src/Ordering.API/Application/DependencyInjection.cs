using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.API.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(typeof(DependencyInjection).Assembly);
        return services;
    }
}
