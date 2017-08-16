using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using PH.Well.Domain.Enums;

namespace PH.Well.Domain
{
    public class SingleLocation
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string AccountAddress { get; set; }
        public IList<SingleLocationItems> Details { get; set; }
    }

    public class SingleLocationItems
    {
        public string Driver { get; set; }
        public DateTime Date { get; set; }
        public int JobTypeId { get; set; }
        public string JobType { get; set; }
        public int JobStatusId { get; set; }
        public string JobStatus { get; set; }
        public bool Cod { get; set; }
        public bool Pod { get; set; }

        private int exceptions;
        public int Exceptions
        {
            get
            {
                if (ResolutionStatus == ResolutionStatus.Imported)
                {
                    return 0;
                }
                return exceptions;
            }
            set { exceptions = value; }
        }

        private int clean;
        public int Clean
        {
            get
            {
                if (ResolutionStatus == ResolutionStatus.Imported)
                {
                    return 0;
                }
                return clean;
            }
            set { clean = value; }
        }
        public int Tba { get; set; }
        public decimal Credit { get; set; }
        public ResolutionStatus ResolutionStatus { get; set; }
        public string Resolution
        {
            get
            {
                return this.ResolutionStatus.Description;
            }
            set { }
        }
        public int ResolutionId
        {
            get
            {
                return this.ResolutionStatus.Value;
            }
            set { }
        }
        public string Invoice { get; set; }
        public int JobId { get; set; }
        public int ActivityId { get; set; }
        public string Assignee { get; set; }
        public bool IsInvoice { get; set; }
        public string AccountNumber { get; set; }
    }
}
