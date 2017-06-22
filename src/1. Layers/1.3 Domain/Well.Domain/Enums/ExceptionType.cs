namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;
    using PH.Well.Domain.Extensions;

    public enum ExceptionType
    {
        [Description("Short")]
        Short = 1,

        [Description("Bypass")]
        Bypass = 2,

        [Description("Damage")]
        Damage = 3
    }

    public static class ExceptionTypeDescriptions
    {
        private static readonly string mShort;
        private static readonly string bypass;
        private static readonly string damage;

        static ExceptionTypeDescriptions()
        {
            mShort = EnumExtensions.GetDescription(ExceptionType.Short);
            bypass = EnumExtensions.GetDescription(ExceptionType.Bypass);
            damage = EnumExtensions.GetDescription(ExceptionType.Damage);
        }

        public static string Description(this ExceptionType value)
        {
            switch (value)
            {
                case ExceptionType.Short:
                    return mShort;

                case ExceptionType.Bypass:
                    return bypass;

                case ExceptionType.Damage:
                    return damage;

                default:
                    return null;
            }
        }
    }
}
