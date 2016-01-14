using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;

namespace CBlokHerkansing.Enum
{
    [Flags]
    public enum BestelStatus
    {
        [Description("Pending")]
        PENDING = 0,
        [Description("Processing")]
        PROCESSING = 1,
        [Description("Delivered")]
        DELIVERED = 2,
    }

    public enum BestelTijd
    {
        [Description("3 Days")]
        SHORT = 0,
        [Description("7 Days")]
        DEFAULT = 1,
        [Description("10 Days")]
        MEDIUM = 2,
        [Description("11 Days or more")]
        LONG = 3
    }

    public class BestelEnum
    {
        public static string GetDescription(BestelTijd value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            DescriptionAttribute attribute
                    = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                        as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}