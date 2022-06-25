using SimpleString.Extenisons.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace SimpleString
{
    /// <summary>
    /// 配置
    /// </summary>
    public class Config
    {
        internal bool IsInit { get; set; }
        internal IReadOnlyDictionary<string, Dictionary<string, string>> XMLDocContainer { get; set; }

        private Type _attributeType = typeof(DescriptionAttribute);

        /// <summary>
        /// 特性，默认 <see cref="DescriptionAttribute"/>.
        /// </summary>
        public Type AttributeType
        {
            get => _attributeType;
            set
            {
                if (IsInit)
                {
                    throw new InvalidOperationException("An operation has already been started on the current instance.");
                }

                if (!value.IsSubclassOf(typeof(Attribute)))
                {
                    throw new ArgumentException($"{value} is not a attribute.");
                }

                _attributeType = value;
            }
        }

        private string _name = nameof(DescriptionAttribute.Description);

        /// <summary>
        /// 特性属性名称，默认 <see cref="DescriptionAttribute.Description"/>.
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                if (IsInit)
                {
                    throw new InvalidOperationException("An operation has already been started on the current instance.");
                }

                _name = value;
            }
        }

        private string _operator = " = ";

        /// <summary>
        /// 间隔符号，默认“ = ”
        /// </summary>
        public string Operator
        {
            get => _operator;
            set
            {
                if (IsInit)
                {
                    throw new InvalidOperationException("An operation has already been started on the current instance.");
                }

                _operator = value;
            }
        }

        private HandleType _handleType = HandleType.XML;

        /// <summary>
        /// 处理方式，默认 <see cref="HandleType.XML"/>
        /// </summary>
        public HandleType HandleType
        {
            get => _handleType;
            set
            {
                if (IsInit)
                {
                    throw new InvalidOperationException("An operation has already been started on the current instance.");
                }

                _handleType = value;
            }
        }

        private HashSet<string> _XMLDocPath = new HashSet<string>();

        /// <summary>
        /// XML文档路径
        /// </summary>
        public HashSet<string> XMLDocPath
        {
            get => _XMLDocPath;
            set
            {
                if (IsInit)
                {
                    throw new InvalidOperationException("An operation has already been started on the current instance.");
                }

                _XMLDocPath = value;
            }
        }

        /// <summary>
        /// 忽略循环引用（存在相同引用只输出一次否则为空），默认false.
        /// </summary>
        public bool IgnoreLoopReference { get; set; } = false;

        /// <summary>
        /// 自定义类型处理，默认调用 ToString() 否则 ToSimpleString().
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
            if (null == this)
            {
                throw new ArgumentNullException(nameof(Config));
            }

            if (IsInit)
            {
                return this;
            }

            IsInit = true;

            if (HandleType.Attribute == HandleType)
            {
                if (null == AttributeType)
                {
                    ErrorMsg = $"SimpleString '{nameof(AttributeType)}' property is null.";

                    return this;
                }

                if (string.IsNullOrWhiteSpace(Name))
                {
                    ErrorMsg = $"SimpleString '{nameof(Name)}' property is null.";

                    return this;
                }

                if (null == AttributeType.GetProperty(Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase))
                {
                    ErrorMsg = $"SimpleString {AttributeType.Name} not found {Name} property.";

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
    }
}