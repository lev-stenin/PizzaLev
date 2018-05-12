using System;

namespace PizzaDomain.Infrastructure
{
    public static class EnumExtensions
    {
        public static string GetCustomDescription(object objEnum)
        {
            var fi = objEnum.GetType().GetField(objEnum.ToString());
            var description = fi.GetCustomDescription();
            return description ?? objEnum.ToString();
        }

        public static string GetDescription(this Enum value)
        {
            return GetCustomDescription(value);
        }
    }
}