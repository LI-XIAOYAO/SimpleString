using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleString.Attributes
{
    /// <summary>
    /// 忽略转换
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class IgnoreSimpleStringAttribute : Attribute
    {
    }
}