namespace PH.Well.Api.Models
{
    using System.Collections.Generic;

    public class ErrorModel
    {
        public ErrorModel()
        {
            Errors = new List<string>();
        }

        public string Message { get; set; }
        public List<string> Errors { get; set; }
    }
}