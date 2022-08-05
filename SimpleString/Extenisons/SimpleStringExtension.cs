using SimpleString;
using System;
using System.Collections.Generic;

namespace System
{
    /// <summary>
    /// 简单字符串
    /// </summary>
    public static class SimpleStringExtension
    {
        /// <summary>
        /// 返回转换后的字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToSimpleString(this object obj)
        {
            switch (SimpleString.SimpleString.Config.HandleType)
            {
                case HandleType.Attribute:
                    return new AttributeString().ToSimpleString(obj);

                case HandleType.XML:
                default:
                    return new XMLString().ToSimpleString(obj);
            }
        }
    }
}