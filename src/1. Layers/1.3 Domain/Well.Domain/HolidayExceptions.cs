﻿namespace PH.Well.Domain
{
    using System;

    public class HolidayExceptions : Entity<int>
    {
        public DateTime ExceptionDate { get; set; }

        public string Exception { get; set; }
    }
}
