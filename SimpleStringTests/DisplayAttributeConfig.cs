using SimpleString;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SimpleStringTests
{
    /// <summary>
    /// DisplayAttributeConfig
    /// </summary>
    public class DisplayAttributeConfig : Config
    {
        public DisplayAttributeConfig()
        {
            AttributeType = typeof(DisplayNameAttribute);
            Name = nameof(DisplayNameAttribute.DisplayName);
            Operator = ": ";
        }
    }
}