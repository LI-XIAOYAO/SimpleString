using System;

namespace SimpleString
{
    /// <summary>
    /// 简单字符串
    /// </summary>
    public class SimpleString
    {
        private readonly SimpleStringBase _simpleString;

        /// <summary>
        /// 简单字符串
        /// </summary>
        /// <param name="config">配置</param>
        public SimpleString(Config config)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            switch (config.HandleOptions)
            {
                case HandleOptions.Attribute:
                    _simpleString = new AttributeString(config);
                    break;

                case HandleOptions.XML:
                default:
                    _simpleString = new XMLString(config);
                    break;
            }
        }

        /// <summary>
        /// 返回通过指定 <see cref="HandleOptions"/> 类型转换后的简单字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string ToSimpleString(object obj) => _simpleString.ToSimpleString(obj);

        /// <summary>
        /// 默认配置
        /// </summary>
        public static Config DefaultConfig { get; } = new Config();

        /// <summary>
        /// 配置默认
        /// </summary>
        /// <param name="action"></param>
        public static void Config(Action<Config> action)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            action(DefaultConfig);
        }
    }

    /// <summary>
    /// 简单字符串
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SimpleString<T> : SimpleString
        where T : Config
    {
        /// <summary>
        /// 简单字符串
        /// </summary>
        /// <param name="config"></param>
        public SimpleString(T config)
            : base(config)
        {
        }
    }
}