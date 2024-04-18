using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleStringCore.Entity;
using SimpleStringTests;

namespace SimpleStringCore.Extenisons.Tests
{
    [TestClass()]
    public class SimpleStringExtensionTests
    {
        [TestMethod()]
        public void ToSimpleStringByAttributeTest()
        {
            SimpleString.Config(c =>
            {
                //c.Operator = ": ";
                //c.AttributeType = typeof(System.ComponentModel.DisplayNameAttribute);
                //c.Name = nameof(System.ComponentModel.DisplayNameAttribute.DisplayName);
                c.HandleOptions = HandleOptions.Attribute;
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
                    MyProperty1 = 4,
                    InternalClass1 = new Test2.InternalClass
                    {
                        MyProperty = 5
                    }
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
                },
                InternalClass1 = new Test.InternalClass
                {
                    MyProperty = 3
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
                HandleOptions = HandleOptions.Attribute
            }).ToSimpleString(result);
            Console.WriteLine("str4: {0}", str4);
            Assert.AreNotEqual(null, str4);
        }

        [TestMethod()]
        public void ToSimpleStringByXMLTest()
        {
            SimpleString.Config(c =>
            {
                //c.Operator = ": ";
                //c.AttributeType = typeof(System.ComponentModel.DisplayNameAttribute);
                //c.Name = nameof(System.ComponentModel.DisplayNameAttribute.DisplayName);
                c.AddXml("SimpleString.Entity.xml");
                //c.HandCustomType = true;
                //c.IgnoreLoopReference = true;
            });

            var eav = TestEnum.A.ToSimpleString();
            var ts = new TestClass
            {
                TestEnum = TestEnum.A,
                TestEnumNull = null,
                TestStruct = new TestStruct
                {
                    MyProperty = 3,
                    MyProperty1 = 2
                },
                TestStruct1 = new TestStruct
                {
                    MyProperty1 = 1,
                    MyProperty = null
                }
            };
            var tss = ts.ToString();

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
                },
                InternalClass1 = new Test.InternalClass
                {
                    MyProperty = 3
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

            var config = new Config();
            config.AddXml("SimpleString.Entity.xml");

            var str4 = new SimpleString(config).ToSimpleString(result);
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
                         c.AddXml("SimpleString.Entity.xml", "SimpleString.xml");
                     });
                     c.AddSimpleString<Config>(c =>
                     {
                         c.Operator = "=>";
                         c.HandCustomType = true;
                     });
                     c.AddSimpleString<DisplayAttributeConfig>();
                 })
                 .Build();

            var result = new Test
            {
                TestClass = new TestClass
                {
                    MyProperty = 1,
                    TestEnum = TestEnum.B
                },
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
                },
                InternalClass1 = new Test.InternalClass
                {
                    MyProperty = 3
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