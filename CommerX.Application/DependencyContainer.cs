using Microsoft.Extensions.DependencyInjection;
using CommerX.Application.Customers.Ports;
using CommerX.Application.Customers.UseCases;

namespace CommerX.Application
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ICreateCustomerInputPort, CreateCustomerUseCase>();
            services.AddScoped<IUpdateCustomerInputPort, UpdateCustomerUseCase>();
            return services;
        }
    }
}
