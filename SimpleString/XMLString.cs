using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SimpleStringCore
{
    /// <summary>
    /// 文档注释转换字符串
    /// </summary>
    internal class XMLString : SimpleStringBase
    {
        /// <summary>
        /// 文档注释转换字符串
        /// </summary>
        public XMLString()
            : this(SimpleString.DefaultConfig)
        {
        }

        /// <summary>
        /// 文档注释转换字符串
        /// </summary>
        /// <param name="config">配置</param>
        public XMLString(Config config)
            : base(config)
        {
        }

        /// <summary>
        /// 返回通过XML注释转换后的字符串
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
                    stringBuilder.Append($"{(IsDefinedXMLAnnotation(type.GetField(obj.ToString()), out var annotation) ? annotation : string.Empty)}");

                    return;
                }

                int index = 0;
                foreach (PropertyInfo prop in type.GetProperties())
                {
                    if (!prop.CanRead || prop.IsDefined(typeof(IgnoreSimpleStringAttribute)) || ignoreProps.Contains(prop))
                    {
                        continue;
                    }

                    if (IsDefinedXMLAnnotation(prop, out var annotation))
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
                            stringBuilder.Append($"{annotation}{_config.Operator}{(isContains ? ToSimpleString(value, null, ignoreProps.Union(new List<PropertyInfo> { prop }).ToArray()) : ToSimpleString(value, null, ignoreProps))}");
                        }
                        else
                        {
                            var propertyType = prop.PropertyType;

                            // 枚举
                            if (propertyType.IsEnum || (propertyType = GetNullableUnderlyingType(prop.PropertyType)).IsEnum)
                            {
                                string enumAnnotation = null == value || !IsDefinedXMLAnnotation(propertyType.GetField(value.ToString()), out enumAnnotation) ? string.Empty : enumAnnotation;

                                stringBuilder.Append($"{annotation}{_config.Operator}{value?.ToString()}{(string.IsNullOrWhiteSpace(enumAnnotation) ? string.Empty : $"[{enumAnnotation}]")}");
                            }
                            else
                            {
                                stringBuilder.Append($"{annotation}{_config.Operator}{(_config.HandCustomType || isContains ? ToSimpleString(value, null, ignoreProps.Union(new List<PropertyInfo> { prop }).ToArray()) : value?.ToString())}");
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
        /// IsDefinedXMLAnnotation
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="annotation"></param>
        /// <returns></returns>
        private bool IsDefinedXMLAnnotation(MemberInfo prop, out string annotation)
        {
            annotation = null;

            if (null == _config.XMLDocContainer || 0 == _config.XMLDocContainer.Count)
            {
                return false;
            }

            var fullName = prop.DeclaringType.FullName.Replace("+", ".");

            if (_config.XMLDocContainer.TryGetValue(fullName, out var value))
            {
                return value.TryGetValue($"{fullName}.{prop.Name}", out annotation);
            }

            return false;
        }

        /// <summary>
        /// GetXMLAnnotationByName
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        private string GetXMLAnnotationByName(MemberInfo prop)
        {
            //if (!IsDefinedXMLAnnotation(prop))
            //{
            //    return null;
            //}

            var fullName = prop.DeclaringType.FullName.Replace("+", ".");

            return _config.XMLDocContainer[fullName][$"{fullName}.{prop.Name}"];
        }
    }
}