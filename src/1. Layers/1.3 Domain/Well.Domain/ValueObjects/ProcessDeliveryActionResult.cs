namespace PH.Well.Domain.ValueObjects
{
    using System.Collections.Generic;
    using PH.Well.Domain.Enums;

    public class ProcessDeliveryActionResult
    {
        public ProcessDeliveryActionResult()
        {
            this.Warnings = new List<string>();
            this.AdmamIsDown = false;
        }
        
        public List<string> Warnings { get; set; }

        public bool AdmamIsDown { get; set; }
    }
}