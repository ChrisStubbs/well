namespace PH.Well.Domain.Extensions
{
    using System;
    using ValueObjects;

    public static class Extensions
    {
        public static string ToShortDateString(this DateTime? dateTime)
        {
            return dateTime?.ToShortDateString() ?? DateTime.MinValue.ToShortDateString();
        }

        public static Notification ToNotification(this AdamFail failure)
        {
            string[] substrings = failure.JobParameters.Split(',');
            int type;
            Int32.TryParse(substrings[0], out type);

            return new Notification
            {
                JobId = failure.JobId,
                ErrorMessage = failure.ErrorMessage,
                Type = type,
                Branch = substrings[1],
                Account = substrings[2],
                InvoiceNumber = substrings[3],
                LineNumber = substrings[4],
                AdamErrorNumber = substrings[5],
                AdamCrossReference = substrings[6],
                UserName = failure.Operator,
                Source = "ADAMCSS"
            };
        }
    }
}