using System.ComponentModel;
using System.Reflection;

namespace PizzaDomain.Infrastructure
{
    public static class InfoExtensions
    {
        public static string GetCustomDescription(this FieldInfo fi)
        {
            var attributes = (DescriptionAttribute[]) fi?.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes?.Length > 0 ? attributes[0].Description : fi?.Name;
        }

        public static string GetCustomDescription(this PropertyInfo pi)
        {
            var attributes = (DescriptionAttribute[]) pi?.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes?.Length > 0 ? attributes[0].Description : pi?.Name;
        }
    }
}