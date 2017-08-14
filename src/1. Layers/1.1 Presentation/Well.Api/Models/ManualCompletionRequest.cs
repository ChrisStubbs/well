namespace PH.Well.Api.Models
{
    using System.Collections.Generic;
    using Domain.Enums;

    public class ManualCompletionRequest
    {
        public ManualCompletionRequest()
        {
            JobIds = new List<int>();
        }

        public IList<int> JobIds { get; set; }
        public ManualCompletionType ManualCompletionType { get; set; }
    }

}