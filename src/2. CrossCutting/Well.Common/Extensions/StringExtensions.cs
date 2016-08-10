namespace PH.Well.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public static class StringExtensions
    {
        public static bool IsInteger(this string sourceString)
        {
            int output;

            return sourceString != null && int.TryParse(sourceString, out output);
        }

        public static bool IsLong(this string sourceString)
        {
            long output;

            return sourceString != null && long.TryParse(sourceString, out output);
        }

        public static bool IsDecimal(this string sourceString)
        {
            decimal output;

            return sourceString != null && decimal.TryParse(sourceString, out output);
        }

        public static bool IsBool(this string sourceString)
        {
            bool output;

            return sourceString != null && bool.TryParse(sourceString, out output);
        }

        public static bool IsDate(this string source)
        {
            DateTime result;

            return DateTime.TryParse(source, out result);
        }

        public static bool AsBool(this string sourceString)
        {
            bool output;

            if (sourceString == null || !sourceString.IsBool())
            {
                return false;
            }

            bool.TryParse(sourceString, out output);
            return output;
        }

        public static int? AsInteger(this string sourceString)
        {
            int output;

            if (sourceString == null || !sourceString.IsInteger())
            {
                return null;
            }

            int.TryParse(sourceString, out output);
            return output;
        }

        public static decimal? AsDecimal(this string sourceString, decimal? defaultValue = null)
        {
            decimal output;

            if (sourceString == null || !sourceString.IsDecimal())
            {
                return defaultValue;
            }

            decimal.TryParse(sourceString, out output);
            return output;
        }


        public static string GetEnumDescription(Enum en)
        {
            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return en.ToString();
        }

        public static IEnumerable<string> GetDescriptions<T>()
        {
            var attributes = typeof(T).GetMembers()
                .SelectMany(member => member.GetCustomAttributes(typeof(DescriptionAttribute), true).Cast<DescriptionAttribute>())
                .ToList();

            return attributes.Select(x => x.Description);
        }

        public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);

            if (!type.IsEnum)
            {
                throw new InvalidOperationException();
            }

            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

                if (attribute != null)
                {
                    if (attribute.Description.ToLower() == description.ToLower())
                    {
                        return (T)field.GetValue(null);
                    }
                }
                else
                {
                    if (field.Name.ToLower() == description.ToLower())
                    {
                        return (T)field.GetValue(null);
                    }
                }
            }

            throw new ArgumentException("Enum description attribute not found!", description);
        }

        public static bool TryGetValueFromDescription<T>(string description, out T value)
        {
            var type = typeof(T);

            if (!type.IsEnum)
            {
                throw new InvalidOperationException();
            }

            if (string.IsNullOrWhiteSpace(description) == false)
            {
                foreach (var field in type.GetFields())
                {
                    var attribute =
                        Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

                    if (attribute != null)
                    {
                        if (attribute.Description.ToLower() == description.ToLower())
                        {
                            value = (T)field.GetValue(null);
                            return true;
                        }
                    }
                    else
                    {
                        if (field.Name.ToLower() == description.ToLower())
                        {
                            value = (T)field.GetValue(null);
                            return true;
                        }
                    }
                }
            }

            value = default(T);
            return false;
        }
        
        public static string GetFilenameWithoutPath(this string filePath)
        {
            return Path.GetFileName(filePath);
        }
        
        public static string ToDashboardDateFormat(this DateTime dateTime)
        {
            return dateTime.ToString("dd-MM-yyyy HH:mm:ss");
        }
    }
}