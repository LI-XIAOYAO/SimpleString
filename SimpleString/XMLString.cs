﻿using SimpleString.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SimpleString
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
            : this(SimpleString.Config)
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
                int index = 0;
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (prop.IsDefined(typeof(IgnoreSimpleStringAttribute)) || ignoreProps.Contains(prop))
                    {
                        continue;
                    }

                    if (IsDefinedXMLAnnotation(prop))
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
                        if ((prop.PropertyType.IsGenericType || prop.PropertyType.IsArray) && !prop.PropertyType.IsValueType)
                        {
                            stringBuilder.Append($"{GetXMLAnnotationByName(prop)}{_config.Operator}{(isContains ? ToSimpleString(value, null, ignoreProps.Union(new List<PropertyInfo> { prop }).ToArray()) : ToSimpleString(value, null, ignoreProps))}");
                        }
                        // 枚举
                        else if (prop.PropertyType.IsEnum)
                        {
                            var enumName = GetXMLAnnotationByName(value?.GetType().GetField(value.ToString()));

                            stringBuilder.Append($"{GetXMLAnnotationByName(prop)}{_config.Operator}{value?.ToString()}{(string.IsNullOrWhiteSpace(enumName) ? string.Empty : $"[{enumName}]")}");
                        }
                        else
                        {
                            stringBuilder.Append($"{GetXMLAnnotationByName(prop)}{_config.Operator}{(_config.HandCustomType || isContains ? ToSimpleString(value, null, ignoreProps.Union(new List<PropertyInfo> { prop }).ToArray()) : value?.ToString())}");
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
        /// <returns></returns>
        private bool IsDefinedXMLAnnotation(MemberInfo prop)
        {
            var fullName = prop.DeclaringType.FullName.Replace("+", ".");

            return null != prop && (_config.XMLDocContainer?.Any() ?? false) && _config.XMLDocContainer.ContainsKey(fullName) && _config.XMLDocContainer[fullName].ContainsKey($"{fullName}.{prop.Name}");
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