using SimpleStringCore;

namespace System
{
    /// <summary>
    /// 简单字符串
    /// </summary>
    public static class SimpleStringExtension
    {
        private static SimpleStringBase _simpleString;
        private static readonly object _lock = new object();

        /// <summary>
        /// 返回转换后的字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToSimpleString(this object obj)
        {
            if (null == _simpleString)
            {
                lock (_lock)
                {
                    if (null == _simpleString)
                    {
                        switch (SimpleString.DefaultConfig.HandleOptions)
                        {
                            case HandleOptions.Attribute:
                                _simpleString = new AttributeString();
                                break;

                            case HandleOptions.XML:
                            default:
                                _simpleString = new XMLString();
                                break;
                        }
                    }
                }
            }

            return _simpleString.ToSimpleString(obj);
        }
    }
}