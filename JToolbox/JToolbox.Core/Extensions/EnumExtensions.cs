﻿using System;
using System.ComponentModel;
using System.Linq;

namespace JToolbox.Core.Extensions
{
    public static class EnumExtensions
    {
        public static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
            return (T)attributes.FirstOrDefault();
        }

        public static DescriptionAttribute GetDescription(this Enum value)
        {
            return value.GetAttribute<DescriptionAttribute>();
        }
    }
}