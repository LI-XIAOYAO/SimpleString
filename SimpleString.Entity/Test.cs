using SimpleString.Attributes;
using SimpleString.Extenisons;
using System.ComponentModel;

namespace SimpleString.Entity
{
    /// <summary>
    /// Test
    /// </summary>
    public class Test
    {
        /// <summary>
        /// 测试类
        /// </summary>
        [Description("测试类")]
        public TestClass TestClass { get; set; }

        /// <summary>
        /// XML属性1
        /// </summary>
        [DisplayName("oooo")]
        [IgnoreSimpleString]
        [Description("属性1")]
        public int MyProperty1 { get; set; }

        /// <summary>
        /// XML属性2
        /// </summary>
        [DisplayName("oooo")]
        [Description("属性2")]
        public int MyProperty2 { get; set; }

        /// <summary>
        /// XML属性3
        /// </summary>
        [Description("属性3")]
        public Dictionary<int, int>? MyProperty3 { get; set; }

        /// <summary>
        /// XML属性4
        /// </summary>
        [Description("属性4")]
        public Test2? MyProperty4 { get; set; }

        /// <summary>
        /// XML属性5
        /// </summary>
        [Description("属性5")]
        public Test2 MyProperty5 { get; set; }

        /// <summary>
        /// XML自身
        /// </summary>
        [Description("自身")]
        public Test Current { get; set; }

        /// <summary>
        /// XMLList
        /// </summary>
        [Description("List")]
        public List<Test2> List { get; set; }

        /// <summary>
        /// XMLList1
        /// </summary>
        [Description("List1")]
        public List<Test> List1 { get; set; }

        /// <summary>
        /// XMLA
        /// </summary>
        public string A;

        /// <summary>
        /// XML结构
        /// </summary>
        [Description("结构")]
        public List<Struct> Structs { get; set; }

        /// <summary>
        /// XML字典
        /// </summary>
        [Description("字典")]
        public Dictionary<int, Test>? Dic { get; set; }

        /// <summary>
        /// XML字符串集合
        /// </summary>
        [Description("字符串集合")]
        public List<string> Strs { get; set; }

        /// <summary>
        /// XML内部类
        /// </summary>
        public class InternalClass
        {
            /// <summary>
            /// XML属性1
            /// </summary>
            [Description("属性1")]
            public int MyProperty { get; set; }
        }

        public int MyProperty6 { get; set; }

        public override string ToString() => this.ToSimpleString();
    }

    /// <summary>
    /// Test2
    /// </summary>
    public class Test2
    {
        /// <summary>
        /// XML属性 1
        /// </summary>
        [Description("属性1")]
        public int MyProperty1 { get; set; }

        public override string ToString() => this.ToSimpleString();
    }

    /// <summary>
    /// Struct
    /// </summary>
    public struct Struct
    {
        /// <summary>
        /// XML属性1
        /// </summary>
        [Description("属性1")]
        public int MyProperty { get; set; }
    }
}