using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Common.RabbitMQ.Extensions
{
    public static class Registration
    {
        public static class DynamicallyInstaller
        {
            private static IEnumerable<Type> AllTypes
            {
                get { return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()); }
            }

            public static void NeedToInstallConfig()
            {
                var typesToRegister = AllTypes
                    .Where(it => !(it.IsAbstract || it.IsInterface)
                                 && typeof(IBasicConsumer).IsAssignableFrom(it));

                // foreach (var item in typesToRegister)
                // {
                //     var service = (IBasicConsumer) Activator.CreateInstance(item);
                //
                //     service.HandleBasicDeliver(services);
                // }
            }
        }
    }
}