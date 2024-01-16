using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Wallet.Common.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum enumValue, params object?[] args)
        {
            string enumText = enumValue.ToString();

            FieldInfo fi = enumValue.GetType().GetField(enumText)!;

            string? description = fi.GetCustomAttributes(typeof(DescriptionAttribute), false)
                         .Cast<DescriptionAttribute>()
                         .Select(x => x.Description)
                         .FirstOrDefault();

            if (string.IsNullOrEmpty(description))
            {
                return enumText;
            }

            try
            {
                return args.Length > 0 ? string.Format(CultureInfo.InvariantCulture, description, args) : description;
            }
            catch (FormatException)
            {
                return enumText;
            }
            catch (ArgumentNullException)
            {
                return enumText;
            }
        }
        
        public static string GetDescription(this Type enumType, string enumText, params object?[] args)
        {
            FieldInfo fi = enumType.GetField(enumText)!;

            string? description = fi.GetCustomAttributes(typeof(DescriptionAttribute), false)
                .Cast<DescriptionAttribute>()
                .Select(x => x.Description)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(description))
            {
                return enumText;
            }

            try
            {
                return args.Length > 0 ? string.Format(CultureInfo.InvariantCulture, description, args) : description;
            }
            catch (FormatException)
            {
                return enumText;
            }
            catch (ArgumentNullException)
            {
                return enumText;
            }
        }
        
        public static string? GetDescriptionByString<T>(this string? value)
            where T : struct, Enum
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            if (!Enum.TryParse<T>(value, out T result) || !Enum.IsDefined(typeof(T), value))
            {
                return null;
            }

            return result.GetDescription();
        }
        
        public static int GetValueByString<T>(this string value)
            where T : struct, Enum
        {
            if (!Enum.TryParse<T>(value, out T result) || !Enum.IsDefined(typeof(T), value))
            {
                return 0;
            }

            return result.GetHashCode();
        }

    }
}
