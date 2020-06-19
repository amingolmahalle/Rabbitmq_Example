using System;
using Common.RabbitMq.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Common.RabbitMq.Extensions
{
    public static class RouterRootExtension
    {
        public static IServiceCollection UseRouteProvider(
            this IServiceCollection service,
            Action<IRouteProvider> populateRouterAction)
        {
            var router = new RouteProvider();
            
            populateRouterAction?.Invoke(router);

            service.AddSingleton(typeof(IRouteProvider), router);

            return service;
        }
    }
}