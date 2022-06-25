using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleString.Entity;
using SimpleStringTests;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleString.Extenisons.Tests
{
    [TestClass()]
    public class SimpleStringExtensionTests
    {
        [TestMethod()]
        public void ToSimpleStringByAttributeTest()
        {
            SimpleString.Init(c =>
            {
                //c.Operator = ": ";
                //c.AttributeType = typeof(System.ComponentModel.DisplayNameAttribute);
                //c.Name = nameof(System.ComponentModel.DisplayNameAttribute.DisplayName);
                c.HandleType = HandleType.Attribute;
                //c.HandCustomType = true;
                //c.IgnoreLoopReference = true;
            });

            var result = new Test
            {
                MyProperty1 = 1,
                MyProperty2 = 2,
                MyProperty3 = new Dictionary<int, int> {
                    { 1, 1 },
                    { 2, 3 }
                },
                MyProperty4 = new Test2
                {
                    MyProperty1 = 3
                },
                MyProperty5 = new Test2
                {
                    MyProperty1 = 4
                },
                List = new List<Test2>
                {
                    new Test2 { MyProperty1 = 22 },
                    new Test2 { MyProperty1 = 33 },
                },
                //List1 = new List<Test>(),
                Structs = new List<Struct>
                {
                    new Struct {
                         MyProperty = 1
                    }
                },
                Dic = new Dictionary<int, Test>
                {
                    {
                        1, new Test {
                            MyProperty1 = 11111
                        }
                    }
                },
                Strs = new List<string> {
                    "111", "222", "333"
                }
            };
            var str0 = result.ToSimpleString();
            Console.WriteLine("str0: {0}", str0);
            Assert.AreNotEqual(null, str0);

            result.Current = result;
            var str = result.ToSimpleString();
            Console.WriteLine("str1: {0}", str);
            Assert.AreNotEqual(null, str);

            result.List1 = new List<Test> { result };
            var str1 = result.ToSimpleString();
            Console.WriteLine("str2: {0}", str1);
            Assert.AreNotEqual(null, str1);

            result.Dic = new Dictionary<int, Test>
            {
                {
                    2, result
                }
            };
            var str3 = result.ToSimpleString();
            Console.WriteLine("str3: {0}", str3);
            Assert.AreNotEqual(null, str3);

            var str4 = new SimpleString(new Config
            {
                HandleType = HandleType.Attribute
            }).ToSimpleString(result);
            Console.WriteLine("str4: {0}", str4);
            Assert.AreNotEqual(null, str4);
        }

        [TestMethod()]
        public void ToSimpleStringByXMLTest()
        {
            SimpleString.Init(c =>
            {
                //c.Operator = ": ";
                //c.AttributeType = typeof(System.ComponentModel.DisplayNameAttribute);
                //c.Name = nameof(System.ComponentModel.DisplayNameAttribute.DisplayName);
                c.XMLDocPath = new HashSet<string> {
                    "SimpleString.Entity.xml"
                };
                //c.HandCustomType = true;
                //c.IgnoreLoopReference = true;
            });

            var result = new Test
            {
                MyProperty1 = 1,
                MyProperty2 = 2,
                MyProperty3 = new Dictionary<int, int> {
                    { 1, 1 },
                    { 2, 3 }
                },
                MyProperty4 = new Test2
                {
                    MyProperty1 = 3
                },
                MyProperty5 = new Test2
                {
                    MyProperty1 = 4
                },
                List = new List<Test2>
                {
                    new Test2 { MyProperty1 = 22 },
                    new Test2 { MyProperty1 = 33 },
                },
                //List1 = new List<Test>(),
                Structs = new List<Struct>
                {
                    new Struct {
                         MyProperty = 1
                    }
                },
                Dic = new Dictionary<int, Test>
                {
                    {
                        1, new Test {
                            MyProperty1 = 11111
                        }
                    }
                },
                Strs = new List<string> {
                    "111", "222", "333"
                }
            };
            var str0 = result.ToSimpleString();
            Console.WriteLine("str0: {0}", str0);
            Assert.AreNotEqual(null, str0);

            result.Current = result;
            var str = result.ToSimpleString();
            Console.WriteLine("str1: {0}", str);
            Assert.AreNotEqual(null, str);

            result.List1 = new List<Test> { result };
            var str1 = result.ToSimpleString();
            Console.WriteLine("str2: {0}", str1);
            Assert.AreNotEqual(null, str1);

            result.Dic = new Dictionary<int, Test>
            {
                {
                    2, result
                }
            };
            var str3 = result.ToSimpleString();
            Console.WriteLine("str3: {0}", str3);
            Assert.AreNotEqual(null, str3);

            var str4 = new SimpleString(new Config
            {
                XMLDocPath = new HashSet<string> {
                    "SimpleString.Entity.xml"
                }
            }).ToSimpleString(result);
            Console.WriteLine("str4: {0}", str4);
            Assert.AreNotEqual(null, str4);
        }

        [TestMethod()]
        public void ToSimpleStringByInjectTest()
        {
            var host = Host.CreateDefaultBuilder()
                 .ConfigureServices(c =>
                 {
                     c.AddSimpleString(c =>
                     {
                         c.Operator = "=";
                         c.XMLDocPath.Add("SimpleString.Entity.xml");
                     });
                     c.AddSimpleString<Config>(c => c.Operator = "=>");
                     c.AddSimpleString<DisplayAttributeConfig>();
                 })
                 .Build();

            var result = new Test
            {
                MyProperty1 = 1,
                MyProperty2 = 2,
                MyProperty3 = new Dictionary<int, int> {
                    { 1, 1 },
                    { 2, 3 }
                },
                MyProperty4 = new Test2
                {
                    MyProperty1 = 3
                },
                MyProperty5 = new Test2
                {
                    MyProperty1 = 4
                },
                List = new List<Test2>
                {
                    new Test2 { MyProperty1 = 22 },
                    new Test2 { MyProperty1 = 33 },
                },
                //List1 = new List<Test>(),
                Structs = new List<Struct>
                {
                    new Struct {
                         MyProperty = 1
                    }
                },
                Dic = new Dictionary<int, Test>
                {
                    {
                        1, new Test {
                            MyProperty1 = 11111
                        }
                    }
                },
                Strs = new List<string> {
                    "111", "222", "333"
                }
            };
            var str0 = result.ToSimpleString();
            Console.WriteLine("str0: {0}", str0);
            Assert.AreNotEqual(null, str0);

            var configString = host.Services.GetRequiredService<SimpleString<Config>>();
            var displayConfigString = host.Services.GetRequiredService<SimpleString<DisplayAttributeConfig>>();

            result.Current = result;
            var str = configString.ToSimpleString(result);
            Console.WriteLine("str1: {0}", str);
            Assert.AreNotEqual(null, str);

            result.List1 = new List<Test> { result };
            var str1 = displayConfigString.ToSimpleString(result);
            Console.WriteLine("str2: {0}", str1);
            Assert.AreNotEqual(null, str1);

            result.Dic = new Dictionary<int, Test>
            {
                {
                    2, result
                }
            };
            var str3 = configString.ToSimpleString(result);
            Console.WriteLine("str3: {0}", str3);
            Assert.AreNotEqual(null, str3);

            var str4 = displayConfigString.ToSimpleString(result);
            Console.WriteLine("str4: {0}", str4);
            Assert.AreNotEqual(null, str4);
        }
    }
}