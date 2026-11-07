// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using HMI.Common.Instrumentation.ErrorHandling;
namespace HMI.Infrastructure.Presentation.Factories
{
    /// <summary>
    /// 序列化器工厂
    /// </summary>
    /// <param name="serviceProvider">IServiceProvider</param>
    public class SerializerFactory : ISerializerFactory
    {
        private readonly IServiceProvider services;
        private readonly Dictionary<string, Type> serializerTypes = new(StringComparer.Ordinal);
        private readonly Dictionary<string, ISerializer> serializers = new(StringComparer.Ordinal);
        public SerializerFactory(IServiceProvider container)
        {
            Contract.Requires<ArgumentNullException>(container != null, "container");
            services = container;
        }
        /// <summary>
        /// 获取序列化器
        /// </summary>
        /// <typeparam name="TFor"></typeparam>
        /// <returns></returns>
        public ISerializer GetSerializer<TFor>()
        {
            return this.GetSerializer(typeof(TFor));
        }
        /// <summary>
        /// 获取序列化器
        /// </summary>
        /// <param name="forType"></param>
        /// <returns></returns>
        public ISerializer GetSerializer(Type forType)
        {
            var key = forType.FullName ?? throw new ArgumentException("The serializer type must have a full name.", nameof(forType));
            if (serializers.TryGetValue(key, out var serializer))
            {
                return serializer;
            }

            if (serializerTypes.TryGetValue(key, out var serializerType))
            {
                return (ISerializer)ActivatorUtilities.CreateInstance(services, serializerType);
            }

            throw new InvalidOperationException($"No serializer is registered for '{key}'.");
        }

        public void RegisterSerializer<TFor, TSerializer>() where TSerializer : ISerializer
        {
            serializerTypes[typeof(TFor).FullName!] = typeof(TSerializer);
        }
        public void RegisterSerializer<TFor>(ISerializer instance)
        {
            serializers[typeof(TFor).FullName!] = instance ?? throw new ArgumentNullException(nameof(instance));
        }
    }
}
