using Catalog.API.Application.Consumers;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Catalog.API.Application.Interfaces;
using Catalog.API.Infrastructure.Persistence;

namespace Catalog.API.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CatalogDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("CatalogDatabase"),
                sqlOptions => sqlOptions.MigrationsAssembly(typeof(CatalogDbContext).Assembly.FullName)));

        services.AddScoped<ICatalogDbContext, CatalogDbContext>();

        services.AddStackExchangeRedisCache(options =>
        {
            var redisConn = configuration.GetConnectionString("Redis")
                            ?? configuration["ConnectionStrings:Redis"]
                            ?? Environment.GetEnvironmentVariable("ConnectionStrings__Redis")
                            ?? "redis:6379";
            options.Configuration = redisConn;
            options.InstanceName = "CatalogCache:";
        });

        services.AddMassTransit(x =>
        {
            x.AddConsumer<OrderPlacedEventConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration["RabbitMq:Host"] ?? "rabbitmq", h =>
                {
                    h.Username(configuration["RabbitMq:Username"] ?? "guest");
                    h.Password(configuration["RabbitMq:Password"] ?? "guest");
                });

                cfg.ReceiveEndpoint("catalog-order-placed-queue", e =>
                {
                    e.ConfigureConsumer<OrderPlacedEventConsumer>(context);
                });
            });
        });

        return services;
    }
}
