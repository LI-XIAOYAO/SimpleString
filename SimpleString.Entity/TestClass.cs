using System.ComponentModel;

namespace SimpleString.Entity
{
    /// <summary>
    /// 测试类
    /// </summary>
    public class TestClass
    {
        /// <summary>
        /// 属性1
        /// </summary>
        [Description("属性1")]
        public int MyProperty { get; set; }

        /// <summary>
        /// 属性2
        /// </summary>
        [Description("属性2")]
        public int MyProperty2 { get; set; }

        /// <summary>
        /// 枚举
        /// </summary>
        [Description("枚举")]
        public TestEnum TestEnum { get; set; }

        /// <summary>
        /// 枚举
        /// </summary>
        [Description("枚举Null")]
        public TestEnum? TestEnumNull { get; set; }

        /// <summary>
        /// Null struct
        /// </summary>
        public TestStruct? TestStruct { get; set; }

        /// <summary>
        /// Struct
        /// </summary>
        public TestStruct TestStruct1 { get; set; }

        public override string ToString() => this.ToSimpleString();
    }

    /// <summary>
    /// 测试枚举
    /// </summary>
    public enum TestEnum
    {
        /// <summary>
        /// 枚举A
        /// </summary>
        A,

        /// <summary>
        /// 枚举B
        /// </summary>
        B
    }

    /// <summary>
    /// 测试结构
    /// </summary>
    public struct TestStruct
    {
        /// <summary>
        /// Null int
        /// </summary>
        public int? MyProperty { get; set; }

        /// <summary>
        /// Int
        /// </summary>
        public int MyProperty1 { get; set; }

        public override string ToString() => this.ToSimpleString();
    }
}