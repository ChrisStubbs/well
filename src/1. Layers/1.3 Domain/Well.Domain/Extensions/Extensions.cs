namespace PH.Well.Domain.Extensions
{
    using System;

    public static class Extensions
    {
        public static string ToShortDateString(this DateTime? dateTime)
        {
            return dateTime?.ToShortDateString() ?? DateTime.MinValue.ToShortDateString();
        }
    }
}