namespace PH.Well.Domain.Extensions
{
    using System;
    using System.ComponentModel;
    using Common.Extensions;

    public static class EnumExtensions
    {
        /// <summary>
        /// Returns the description for the given enumeration constant, where the constant/member
        /// are tagged with the System.ComponentModel.DescriptionAttribute
        /// </summary>
        /// <param name="enumConstant"></param>
        /// <returns></returns>
        public static string GetDescription(Enum enumConstant)
        {
            System.Reflection.FieldInfo enumMember = enumConstant.GetType().GetField(enumConstant.ToString());

            var descriptionAttribute =
                enumMember.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (descriptionAttribute != null && descriptionAttribute.Length > 0)
            {
                return descriptionAttribute[0].Description;
            }

            return enumConstant.ToString().SplitCapitalisedWords();
        }

        public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            return default(T);
        }
    }
}