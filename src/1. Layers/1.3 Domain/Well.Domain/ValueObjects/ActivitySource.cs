﻿namespace PH.Well.Domain.ValueObjects
{
    using System;
    using System.Collections.Generic;
    using Enums;

    public class ActivitySource
    {
        public ActivitySource()
        {
            Details = new List<ActivitySourceDetail>();
            Assignees = new List<string>();
        }
        public int ActivityId { get; set; }
        public string Branch { get; set; }
        public int BranchId { get; set; }
        public string PrimaryAccount { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountAddress { get; set; }
        public bool Pod { get; set; }
        public bool Cod { get; set; }
        public string ItemNumber { get; set; }
        public bool IsInvoice { get; set; }
        public DateTime Date { get; set; }
        public string Driver { get; set; }
        public string Assignee => ValueObjects.Assignee.GetDisplayNames(Assignees);
        public int Tba { get; set; }
        public List<ActivitySourceDetail> Details { get; set; }
        public List<string> Assignees { get; set; }
        public ResolutionStatus ResolutionStatus { get; set; }
        public string Resolution => ResolutionStatus.Description;
        public int ResolutionId => ResolutionStatus.Value;
        public int LocationId { get; set; }
        public string InitialDocument { get; set; }

    }

    public class ActivitySourceDetail
    {
        public int ActivityId { get; set; }
        public string Product { get; set; }
        public string Type { get; set; }
        public string BarCode { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public int Expected { get; set; }

        public int Actual
        {
            get
            {
                if (ResolutionStatus == ResolutionStatus.Imported)
                {
                    return 0;
                }

                return Expected - (Damaged + Shorts + Bypass);
            }
        }

        public int Bypass { get; set; }

        public int Damaged { get; set; }
        public int Shorts { get; set; }
        public bool Checked { get; set; }
        public bool HighValue { get; set; }
        public int StopId { get; set; }
        public string Stop { get; set; }
        public DateTime StopDate { get; set; }
        public int JobId { get; set; }
        public string JobType { get; set; }
        public string JobTypeAbbreviation { get; set; }
        public int LineItemId { get; set; }
        public ResolutionStatus ResolutionStatus { get; set; }
        public string Resolution => ResolutionStatus.Description;
        public int ResolutionId => ResolutionStatus.Value;
        public bool HasUnresolvedActions { get; set; }
        public bool CompletedOnPaper { get; set; }
        public bool HasNoDefinedActions { get; set; }
        public int JobStatusId { get; set; }
        public bool HasLineItemActions { get; set; }
        public int UpliftAction { get; set; }
        public int JobTypeId { get; set; }
    }
}
