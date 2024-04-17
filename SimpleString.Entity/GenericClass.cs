using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleString.Entity
{
    /// <summary>
    /// 泛型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericClass<T>
    {
        /// <summary>
        /// MyProperty
        /// </summary>
        public T? MyProperty { get; set; }
    }
}