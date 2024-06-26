﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SimpleStringCore
{
    /// <summary>
    /// 简单字符串
    /// </summary>
    internal abstract class SimpleStringBase
    {
        protected readonly Config _config;
        protected readonly List<object> _stacks = new List<object>();

        /// <summary>
        /// 文档注释转换字符串
        /// </summary>
        protected SimpleStringBase()
            : this(SimpleString.DefaultConfig)
        {
        }

        /// <summary>
        /// 文档注释转换字符串
        /// </summary>
        /// <param name="config">配置</param>
        protected SimpleStringBase(Config config)
        {
            _config = config.Init();
        }

        /// <summary>
        /// 返回转换后的字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual string ToSimpleString<T>(T obj)
            where T : new()
        {
            if (null == obj)
            {
                return string.Empty;
            }

            return _config.IsChecked ? ToSimpleString(obj, null) : _config.ErrorMsg;
        }

        /// <summary>
        /// ToSimpleString
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="stringBuilder"></param>
        /// <param name="ignoreProps"></param>
        /// <returns></returns>
        protected virtual string ToSimpleString<T>(T obj, StringBuilder stringBuilder = null, params PropertyInfo[] ignoreProps)
            where T : new()
        {
            if (null == obj)
            {
                return string.Empty;
            }

            var type = obj.GetType();

            // 忽略
            if (type.IsDefined(typeof(IgnoreSimpleStringAttribute)))
            {
                return string.Empty;
            }

            if (null == stringBuilder)
            {
                stringBuilder = new StringBuilder();
            }

            // 值类型、string
            if (IsValueType(type))
            {
                stringBuilder.Append(obj.ToString());

                return stringBuilder.ToString();
            }

            stringBuilder.Append('[');
            AddStack(type, obj);

            if ((type.IsGenericType || type.IsArray) && typeof(IEnumerable).IsAssignableFrom(type))
            {
                if (typeof(IDictionary).IsAssignableFrom(type))
                {
                    DictionaryHandler(stringBuilder, obj as IDictionary, ignoreProps);
                }
                else
                {
                    ListHandler(stringBuilder, obj as IEnumerable, ignoreProps);
                }
            }
            else
            {
                TypeHandler(obj, stringBuilder, ignoreProps);
            }

            RemoveStack(type);
            stringBuilder.Append(']');

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 类型处理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="stringBuilder"></param>
        /// <param name="ignoreProps"></param>
        protected abstract void TypeHandler<T>(T obj, StringBuilder stringBuilder, params PropertyInfo[] ignoreProps);

        /// <summary>
        /// 处理 List
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <param name="collectionVals"></param>
        /// <param name="ignoreProps"></param>
        protected virtual void ListHandler(StringBuilder stringBuilder, IEnumerable collectionVals, params PropertyInfo[] ignoreProps)
        {
            if (null == collectionVals)
            {
                return;
            }

            int index = 0;
            foreach (var collectionVal in collectionVals)
            {
                if (_config.IgnoreLoopReference && IsContainsStack(collectionVal))
                {
                    return;
                }

                if (0 != index++)
                {
                    stringBuilder.Append(", ");
                }

                ToSimpleString(collectionVal, stringBuilder, ignoreProps);
            }
        }

        /// <summary>
        /// 处理 Dictionary
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <param name="collectionVals"></param>
        /// <param name="ignoreProps"></param>
        protected virtual void DictionaryHandler(StringBuilder stringBuilder, IDictionary collectionVals, params PropertyInfo[] ignoreProps)
        {
            if (null == collectionVals || 0 == collectionVals.Count)
            {
                return;
            }

            int index = 0;
            foreach (DictionaryEntry item in collectionVals)
            {
                if (0 != index++)
                {
                    stringBuilder.Append(", ");
                }

                if (_config.HandCustomType || IsContainsStack(item.Key))
                {
                    ToSimpleString(item.Key, stringBuilder, ignoreProps);
                }
                else
                {
                    stringBuilder.Append(item.Key);
                }

                stringBuilder.Append(_config.Operator);

                if (_config.HandCustomType || IsContainsStack(item.Value))
                {
                    ToSimpleString(item.Value, stringBuilder, ignoreProps);
                }
                else
                {
                    stringBuilder.Append(item.Value);
                }
            }
        }

        /// <summary>
        /// AddStack
        /// </summary>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        protected virtual void AddStack(Type type, object obj)
        {
            if (type.IsPrimitive || type.IsValueType || type == typeof(string))
            {
                return;
            }

            _stacks.Add(obj);
        }

        /// <summary>
        /// RemoveStack
        /// </summary>
        /// <param name="type"></param>
        protected virtual void RemoveStack(Type type)
        {
            if (type.IsPrimitive || type.IsValueType || type == typeof(string))
            {
                return;
            }

            _stacks.RemoveAt(_stacks.Count - 1);
        }

        /// <summary>
        /// IsContainsStack
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected virtual bool IsContainsStack(object obj)
        {
            return null != obj && _stacks.Contains(obj);
        }

        /// <summary>
        /// IsValueType
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected static bool IsValueType(Type type)
        {
            return type.IsValueType && type.IsPrimitive || type == typeof(string) || (type.IsGenericType && typeof(Nullable<>) == type.GetGenericTypeDefinition() && IsValueType(Nullable.GetUnderlyingType(type)));
        }

        /// <summary>
        /// GetNullableUnderlyingType
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected static Type GetNullableUnderlyingType(Type type)
        {
            if (type.IsGenericType && typeof(Nullable<>) == type.GetGenericTypeDefinition())
            {
                return Nullable.GetUnderlyingType(type);
            }

            return type;
        }

        /// <summary>
        /// GetNullableUnderlyingTypeValue
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected static object GetNullableUnderlyingTypeValue(Type type, object value)
        {
            if (null == value)
            {
                return value;
            }

            if (type.IsGenericType && typeof(Nullable<>) == type.GetGenericTypeDefinition())
            {
                return type.GetProperty("Value").GetValue(value);
            }

            return value;
        }
    }
}