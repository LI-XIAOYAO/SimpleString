# SimpleString
通过XML注释或指定特性转换成字符串输出。

<!--TOC-->
- [SimpleString](#simplestring)
    - [安装](#安装)
    - [配置](#配置)
    - [特性](#特性)
    - [食用方法](#食用方法)
    - [示例](#示例)
<!--/TOC-->
---

#### 安装
> Install-Package SimpleString

#### 配置
> 基础配置
````
var config = new Config
{
    AttributeType = typeof(DescriptionAttribute), // 解析的特性，默认DescriptionAttribute（HandleType.Attribute时生效）
    Name = nameof(DescriptionAttribute.Description), // 解析的特性属性，默认DescriptionAttribute.Description（HandleType.Attribute时生效）
    HandleType = HandleType.Attribute, // 处理方式 Attribute|XML（默认）
    HandCustomType = true, // 自定义类型处理，默认调用 ToString() 否则 ToSimpleString().
    IgnoreLoopReference = true, // 忽略循环引用（存在相同引用只输出一次否则为空），默认false.
    Operator = "= ", // 间隔符号，默认“ = ”
    XMLDocPath = new HashSet<string> { 
        "xxx.xml"
    } // XML文档路径，类必须添加注释否则不会解析
};
````
> 自定义配置
````
// 自定义配置只需要继承基础配置也可设置默认值
public class ENConfig : Config
{
    public ENConfig()
    {
        AttributeType = typeof(ENAttribute);
        Name = nameof(ENAttribute.ENName);
        IgnoreLoopReference = true;
    }
}

// 自定义特性
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public class ENAttribute : Attribute
{
    public ENAttribute(string eNName)
    {
        ENName = eNName;
    }

    public string ENName { get; set; }
}
````

#### 特性
>  忽略：[IgnoreSimpleString]

#### 食用方法
> 通用
```
 // 初始化，默认全局
SimpleString.SimpleString.Init(c =>
{
    // 默认使用XML解析 需要指定XML文档
    c.XMLDocPath.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "xxx.xml"));
});

// 直接调用
var str = new Test().ToSimpleString();

// 重写类ToString()方法（推荐）
var str = new Test().ToString();

// 创建实例调用
var str = new SimpleString.SimpleString(new Config
{
    HandleType = HandleType.Attribute,
    Operator = ": "
}).ToSimpleString(test);
```

> .Net Core 优雅食用方法
````
// 默认静态全局注入
services.AddSimpleString(c =>
{
    // 默认XML 需要指定XML文档
    c.XMLDocPath.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WebApplication1.xml"));
});
// 指定特性配置注入
services.AddSimpleString<Config>();
// 指定特性配置注入
services.AddSimpleString<ENConfig>(c =>
{
    c.Operator = ": ";
});

// 构造函数注入调用
private readonly SimpleString<Config> _simpleString;
private readonly SimpleString<ENConfig> _enSimpleString;

public HomeController(ILogger<HomeController> logger, SimpleString<Config> simpleString, SimpleString<ENConfig> enSimpleString)
{
    this._logger = logger;
    this._simpleString = simpleString;
    this._enSimpleString = enSimpleString;
}

var str = _simpleString.ToSimpleString(test);
var str = _enSimpleString.ToSimpleString(test);

````

#### 示例
````
/// <summary>
/// 测试 （XML解析时类注释必须添加）
/// </summary>
public class Test
{
    /// <summary>
    /// XML属性1
    /// </summary>
    [EN("MyProperty1")]
    [Description("属性1")]
    public int MyProperty1 { get; set; }

    /// <summary>
    /// XML属性2
    /// </summary>
    [EN("MyProperty2")]
    [Description("属性2")]
    public string MyProperty2 { get; set; }

    /// <summary>
    /// XML属性3
    /// </summary>
    [IgnoreSimpleString]
    [EN("MyProperty3")]
    [Description("属性3")]
    public string MyProperty3 { get; set; }

    /// <summary>
    /// XML属性4
    /// </summary>
    [EN("MyProperty4")]
    [Description("属性4")]
    public string MyProperty4 { get; set; }

    /// <summary>
    /// XML属性5
    /// </summary>
    [EN("MyProperty5")]
    [Description("属性5")]
    public string MyProperty5 { get; set; }

    /// <summary>
    /// XML属性6
    /// </summary>
    [EN("MyProperty6")]
    [Description("属性6")]
    public string MyProperty6 { get; set; }

    /// <summary>
    /// XML属性7
    /// </summary>
    [EN("MyProperty7")]
    [Description("属性7")]
    public string MyProperty7 { get; set; }

    /// <summary>
    /// 重写ToString 调用默认全局转换
    /// </summary>
    /// <returns></returns>
    public override string ToString() => this.ToSimpleString();
}

var test = new Test
{
    MyProperty1 = 1,
    MyProperty2 = "2",
    MyProperty3 = "33",
    MyProperty4 = "444",
    MyProperty5 = "5555",
    MyProperty6 = "66666"
};

var str1 = test.ToString();
var str2 = test.ToSimpleString();
var str3 = _simpleString.ToSimpleString(test);
var str4 = _enSimpleString.ToSimpleString(test);
var str5 = new SimpleString.SimpleString(new Config
{
    HandleType = HandleType.Attribute,
    Operator = ": "
}).ToSimpleString(test);

// 输出
str1:[XML属性1 = 1, XML属性2 = 2, XML属性4 = 444, XML属性5 = 5555, XML属性6 = 66666, XML属性7 = ]
str2:[XML属性1 = 1, XML属性2 = 2, XML属性4 = 444, XML属性5 = 5555, XML属性6 = 66666, XML属性7 = ]
str3:[属性1 = 1, 属性2 = 2, 属性4 = 444, 属性5 = 5555, 属性6 = 66666, 属性7 = ]
str4:[MyProperty1: 1, MyProperty2: 2, MyProperty4: 444, MyProperty5: 5555, MyProperty6: 66666, MyProperty7: ]
str5:[属性1: 1, 属性2: 2, 属性4: 444, 属性5: 5555, 属性6: 66666, 属性7: ]
````
