using System;
using Microsoft.Extensions.DependencyInjection;
using RabbitMq_Common.RabbitMq.Routing;

namespace RabbitMq_Common.RabbitMq.Extensions
{
    public static class RouterRootExtension
    {
        public static IServiceCollection UseRouteProvider(
            this IServiceCollection service,
            Action<IRouteProvider> populateRouterAction)
        {
            var router = RouteProvider.GetInstance();
            
            populateRouterAction?.Invoke(router);

            service.AddSingleton(typeof(IRouteProvider), router);

            return service;
        }
    }
}