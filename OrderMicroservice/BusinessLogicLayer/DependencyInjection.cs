using BusinessLogicLayer.RabbitMQ;
using eCommerce.ordersMicroservice.BusinessLogicLayer.Mappers;
using eCommerce.ordersMicroservice.BusinessLogicLayer.Services;
using eCommerce.OrdersMicroservice.BusinessLogicLayer.ServiceContracts;
using eCommerce.OrdersMicroservice.BusinessLogicLayer.Validators;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace eCommerce.OrdersMicroservice.BusinessLogicLayer;

public static class DependencyInjection
{
  public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services, IConfiguration configuration)
  {
    //TO DO: Add business logic layer services into the IoC container
    services.AddValidatorsFromAssemblyContaining<OrderAddRequestValidator>();

    services.AddAutoMapper(typeof(OrderAddRequestToOrderMappingProfile).Assembly);

    services.AddScoped<IOrdersService, OrdersService>();

    services.AddTransient<IRabbitMQProductNameUpdateConsumer, RabbitMQProductNameUpdateConsumer>();
    services.AddTransient<IRabbitMQProductDeletionConsumer, RabbitMQProductDeletionConsumer>();

    services.AddHostedService<RabbitMQProductNameUpdateHostedService>();
    services.AddHostedService<RabbitMQProductDeletionHostedService>();

    services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = $"{configuration["REDIS_HOST"]}:{configuration["REDIS_PORT"]}";
    });

    return services;
  }
}
