namespace PH.Well.Domain.ValueObjects
{
    using System.Collections.Generic;
    using PH.Well.Domain.Enums;

    public class ProcessDeliveryActionResult
    {
        public ProcessDeliveryActionResult()
        {
            this.Warnings = new List<string>();
            this.AdamIsDown = false;
        }
        
        public List<string> Warnings { get; set; }

        public bool AdamIsDown { get; set; }
    }
}