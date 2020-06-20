using System;
using System.Collections.Generic;
using System.Linq;
using RabbitMq_Common.RabbitMq;

namespace Common.RabbitMq.AssemblyScanner
{
    public static class Types
    {
        private static IEnumerable<Type> AllTypes
        {
            get { return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()); }
        }

        private static List<Type> _messageHandlers;

        public static IEnumerable<Type> GetHandlers()
        {
            return _messageHandlers ??= AllTypes.Where(it =>
                    !(it.IsAbstract || it.IsInterface) &&
                    it.GetInterfaces().Any(x =>
                        x.IsGenericType &&
                        x.GetGenericTypeDefinition() ==
                        typeof(IHandleCommand<>)))
                .ToList();
        }
    }
}