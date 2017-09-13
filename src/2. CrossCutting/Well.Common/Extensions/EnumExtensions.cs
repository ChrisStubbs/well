namespace PH.Well.Common.Extensions
{
    using System;
    using System.Collections.Generic;

    public static class Enum<T>
    {
        public static T Parse(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        public static bool TryParse(string value, out T enumValue)
        {
            if (Enum.IsDefined(typeof(T), value))
            {
                enumValue = (T)Enum.Parse(typeof(T), value);
                return true;
            }

            value = value.ToLowerInvariant();

            foreach (T val in Enum<T>.GetValues())
            {
                if (value == val.ToString().ToLowerInvariant())
                {
                    enumValue = val;
                    return true;
                }
            }

            enumValue = default(T);
            return false;
        }

        public static T ParseOrDefault(string value, T defaultValue)
        {
            T parsedValue;
            return TryParse(value, out parsedValue) ? parsedValue : defaultValue;
        }

        public static IList<T> GetValues()
        {
            IList<T> list = new List<T>();

            foreach (object value in Enum.GetValues(typeof(T)))
            {
                list.Add((T)value);
            }

            return list;
        }

        public static IDictionary<T, string> GetValuesAndDescriptions()
        {
            var items = new Dictionary<T, string>();

            foreach (Enum enumConstant in Enum.GetValues(typeof(T)))
            {
                items.Add((T)Enum.Parse(typeof(T), enumConstant.ToString()), GetDescription(enumConstant));
            }

            return items;
        }

        public static string[] GetDescriptions()
        {
            var descriptions = new List<string>();

            foreach (Enum enumConstant in Enum.GetValues(typeof(T)))
            {
                descriptions.Add(GetDescription(enumConstant));
            }

            return descriptions.ToArray();
        }

        /// <summary>
        /// Returns the description for the given enumeration constant, where the constant/member
        /// are tagged with the System.ComponentModel.DescriptionAttribute
        /// </summary>
        /// <param name="enumConstant"></param>
        /// <returns></returns>
        public static string GetDescription(Enum enumConstant)
        {
            System.Reflection.FieldInfo enumMember = enumConstant.GetType().GetField(enumConstant.ToString());
            try
            {
                var descriptionAttribute =
                    enumMember.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false) as System.ComponentModel.DescriptionAttribute[];

                if (descriptionAttribute != null && descriptionAttribute.Length > 0)
                {
                    return descriptionAttribute[0].Description;
                }

                return enumConstant.ToString().SplitCapitalisedWords();
            }
            catch (Exception ex)
            {
                return ex.Message + ": Occurred in Enum.cs (GetDescription)";
                //shadowing erros...bad bad bad 
            }
        }
    }
}