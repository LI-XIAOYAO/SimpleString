using SimpleString;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 注入
    /// </summary>
    public static class InjectExtension
    {
        /// <summary>
        /// 注入默认字符串转换
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IServiceCollection AddSimpleString(this IServiceCollection serviceProvider, Action<Config> config = null)
        {
            SimpleString.SimpleString.Config(config);

            return serviceProvider.AddSingleton<AttributeString>();
        }

        /// <summary>
        /// 注入指定特性字符串转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceProvider"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IServiceCollection AddSimpleString<T>(this IServiceCollection serviceProvider, Action<T> config = null)
            where T : Config, new()
        {
            serviceProvider.Where(c => c.ServiceType == typeof(SimpleString<T>)).ToList()
                .ForEach(c => serviceProvider.Remove(c));

            var conf = new T();
            config?.Invoke(conf);
            conf.HandleOptions = HandleOptions.Attribute;

            return serviceProvider.AddSingleton(new SimpleString<T>(conf));
        }
    }
}