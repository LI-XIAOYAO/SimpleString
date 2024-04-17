using SimpleString.Extenisons.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace SimpleString
{
    /// <summary>
    /// 配置
    /// </summary>
    public class Config
    {
        private Type _attributeType = typeof(DescriptionAttribute);
        private string _name = nameof(DescriptionAttribute.Description);
        private string _operator = " = ";
        private HandleOptions _handleOptions = HandleOptions.XML;

        /// <summary>
        /// XMLDocPath
        /// </summary>
        internal HashSet<string> XMLDocPath { get; } = new HashSet<string>();

        /// <summary>
        /// XMLDocContainer
        /// </summary>
        internal IReadOnlyDictionary<string, Dictionary<string, string>> XMLDocContainer { get; set; }

        /// <summary>
        /// IsInit
        /// </summary>
        internal bool IsInit { get; set; }

        /// <summary>
        /// 特性，默认 <see cref="DescriptionAttribute"/>.
        /// </summary>
        public Type AttributeType
        {
            get => _attributeType;
            set
            {
                CheckInit();

                if (!typeof(Attribute).IsAssignableFrom(value ?? throw new ArgumentNullException(nameof(AttributeType))))
                {
                    throw new ArgumentException($"{value} is not a attribute.");
                }

                _attributeType = value;
            }
        }

        /// <summary>
        /// 特性属性名称，默认 <see cref="DescriptionAttribute.Description"/>.
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                CheckInit();

                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(nameof(Name));
                }

                _name = value;
            }
        }

        /// <summary>
        /// AttributeProp
        /// </summary>
        internal PropertyInfo AttributeProp { get; set; }

        /// <summary>
        /// 间隔符号，默认“ = ”
        /// </summary>
        public string Operator
        {
            get => _operator;
            set
            {
                CheckInit();

                _operator = value;
            }
        }

        /// <summary>
        /// 处理方式，默认 <see cref="HandleOptions.XML"/>
        /// </summary>
        public HandleOptions HandleOptions
        {
            get => _handleOptions;
            set
            {
                CheckInit();

                if (!Enum.IsDefined(typeof(HandleOptions), value))
                {
                    throw new ArgumentException(nameof(HandleOptions));
                }

                _handleOptions = value;
            }
        }

        /// <summary>
        /// 忽略循环引用（存在相同引用只输出一次否则为空），默认false.
        /// </summary>
        public bool IgnoreLoopReference { get; set; } = false;

        /// <summary>
        /// 自定义类型处理，默认 false 调用 <see cref="object.ToString()"/>，否则调用 <see cref="SimpleString.ToSimpleString(object)"/>.
        /// </summary>
        public bool HandCustomType { get; set; } = false;

        /// <summary>
        /// IsChecked
        /// </summary>
        internal bool IsChecked { get; set; }

        /// <summary>
        /// ErrorMsg
        /// </summary>
        internal string ErrorMsg { get; set; }

        /// <summary>
        /// Init
        /// </summary>
        /// <returns></returns>
        internal Config Init()
        {
            if (IsInit)
            {
                return this;
            }

            IsInit = true;

            if (HandleOptions.Attribute == HandleOptions)
            {
                if (null == (AttributeProp = AttributeType.GetProperty(Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)))
                {
                    ErrorMsg = $"Type {AttributeType.Name} not found property {Name}.";

                    return this;
                }

                IsChecked = true;
            }
            else
            {
                this.XMLDocResolver();
            }

            return this;
        }

        /// <summary>
        /// CheckInit
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        private void CheckInit()
        {
            if (IsInit)
            {
                throw new InvalidOperationException("An operation has already been started on the current instance.");
            }
        }

        /// <summary>
        /// XML文档路径，类必须添加注释否则不会解析
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        public Config AddXml(params string[] paths)
        {
            if (paths is null || 0 == paths.Length)
            {
                throw new ArgumentException(nameof(paths));
            }

            CheckInit();

            foreach (var item in paths)
            {
                XMLDocPath.Add(item);
            }

            return this;
        }
    }
}