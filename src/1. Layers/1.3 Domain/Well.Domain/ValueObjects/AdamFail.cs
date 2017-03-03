﻿namespace PH.Well.Domain.ValueObjects
{
    public class AdamFail
    {

        public int Id { get; set; }

        public int JobId { get; set; }

        public string JobParameters { get; set; }

        public string Operator { get; set; }

        public string ErrorMessage { get; set; }
    }
}