using System;

namespace SimpleString.Attributes
{
    /// <summary>
    /// 忽略转换
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Enum | AttributeTargets.Struct)]
    public class IgnoreSimpleStringAttribute : Attribute
    {
    }
}