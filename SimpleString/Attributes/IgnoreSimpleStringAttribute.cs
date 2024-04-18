using System;

namespace SimpleStringCore
{
    /// <summary>
    /// 忽略转换
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Enum | AttributeTargets.Struct)]
    public class IgnoreSimpleStringAttribute : Attribute
    {
    }
}