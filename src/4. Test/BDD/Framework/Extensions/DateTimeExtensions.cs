using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.BDD.Framework.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ParseBritishDate(this string date)
        {
            return DateTime.Parse(date, System.Globalization.CultureInfo.GetCultureInfo("en-GB"));
        }

    }
}
