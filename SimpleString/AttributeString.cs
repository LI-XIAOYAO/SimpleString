using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SimpleStringCore
{
    /// <summary>
    /// 特性转换字符串
    /// </summary>
    internal class AttributeString : SimpleStringBase
    {
        /// <summary>
        /// 特性转换字符串
        /// </summary>
        public AttributeString()
            : this(SimpleString.DefaultConfig)
        {
        }

        /// <summary>
        /// 特性转换字符串
        /// </summary>
        /// <param name="config">配置</param>
        public AttributeString(Config config)
            : base(config)
        {
        }

        /// <summary>
        /// 返回通过特性 <see cref="Config.AttributeType"/> 名称转换后的字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override string ToSimpleString<T>(T obj)
        {
            return base.ToSimpleString(obj);
        }

        /// <summary>
        /// 类型处理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="stringBuilder"></param>
        /// <param name="ignoreProps"></param>
        protected override void TypeHandler<T>(T obj, StringBuilder stringBuilder, params PropertyInfo[] ignoreProps)
        {
            try
            {
                var type = obj.GetType();
                if (type.IsEnum)
                {
                    stringBuilder.Append($"{(type.IsDefined(_config.AttributeType) ? GetAttributeTypeValueByName(type.GetField(obj.ToString())) : string.Empty)}");

                    return;
                }

                int index = 0;
                foreach (PropertyInfo prop in type.GetProperties())
                {
                    if (!prop.CanRead || prop.IsDefined(typeof(IgnoreSimpleStringAttribute)) || ignoreProps.Contains(prop))
                    {
                        continue;
                    }

                    if (prop.IsDefined(_config.AttributeType))
                    {
                        var value = prop.GetValue(obj, null);
                        var isContains = IsContainsStack(value);

                        if (_config.IgnoreLoopReference && isContains)
                        {
                            continue;
                        }

                        if (0 != index++)
                        {
                            stringBuilder.Append(", ");
                        }

                        // 集合、数组
                        if ((prop.PropertyType.IsGenericType || prop.PropertyType.IsArray) && !prop.PropertyType.IsValueType && typeof(IEnumerable).IsAssignableFrom(prop.PropertyType))
                        {
                            stringBuilder.Append($"{GetAttributeTypeValueByName(prop)}{_config.Operator}{(isContains ? ToSimpleString(value, null, ignoreProps.Union(new List<PropertyInfo> { prop }).ToArray()) : ToSimpleString(value, null, ignoreProps))}");
                        }
                        else
                        {
                            var propertyType = prop.PropertyType;

                            // 枚举
                            if (propertyType.IsEnum || (propertyType = GetNullableUnderlyingType(prop.PropertyType)).IsEnum)
                            {
                                var enumName = propertyType.IsDefined(_config.AttributeType) ? GetAttributeTypeValueByName(propertyType.GetField(value.ToString())) : null ?? string.Empty;

                                stringBuilder.Append($"{GetAttributeTypeValueByName(prop)}{_config.Operator}{value?.ToString()}{(string.IsNullOrWhiteSpace(enumName) ? string.Empty : $"[{enumName}]")}");
                            }
                            else
                            {
                                stringBuilder.Append($"{GetAttributeTypeValueByName(prop)}{_config.Operator}{(_config.HandCustomType || isContains ? ToSimpleString(value, null, ignoreProps.Union(new List<PropertyInfo> { prop }).ToArray()) : value?.ToString())}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                stringBuilder.Append($"Error：{ex.Message}");
            }
        }

        /// <summary>
        /// GetAttributeTypeValueByName
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        private string GetAttributeTypeValueByName(MemberInfo prop)
        {
            if (null == prop)
            {
                return null;
            }

            return _config.AttributeProp.GetValue(prop.GetCustomAttribute(_config.AttributeType))?.ToString() ?? string.Empty;
        }
    }
}