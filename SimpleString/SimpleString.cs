using System;
using System.Collections.Generic;
using System.Text;

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
            switch (config.HandleType)
            {
                case HandleType.Attribute:
                    _simpleString = new AttributeString(config);
                    break;

                case HandleType.XML:
                default:
                    _simpleString = new XMLString(config);
                    break;
            }
        }

        /// <summary>
        /// 返回通过指定 <see cref="HandleType"/> 类型转换后的简单字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string ToSimpleString(object obj) => _simpleString.ToSimpleString(obj);

        /// <summary>
        /// 静态配置
        /// </summary>
        public static Config Config { get; } = new Config();

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="action"></param>
        public static void Init(Action<Config> action)
        {
            action?.Invoke(Config);
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