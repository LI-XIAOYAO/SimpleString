using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

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
    }
}