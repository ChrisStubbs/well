namespace PH.Well.Domain.ValueObjects
{
    using System.Collections.Generic;

    public class SubmitActionResult
    {
        public SubmitActionResult()
        {
            Warnings = new List<string>();
        }
        public string Message { get; set; }
        public bool IsValid { get; set; }
        public IList<string> Warnings { get; set; }

    }
}