﻿namespace PH.Well.Api.Models
{
    using System.Collections.Generic;
    using Domain.Enums;
    using Domain.ValueObjects;

    public class SingleRouteItem
    {
        public SingleRouteItem()
        {
            this.Assignees = new List<Assignee>();
        }

        public int JobId { get; set; }
        public string Stop { get; set; }
        public string StopStatus { get; set; }
        public string Previously { get; set; }
        public int Tba { get; set; }
        public string StopAssignee { get; set; }
        public string Resolution { get; set; }
        public int ResolutionId { get; set; }
        public string Invoice { get; set; }
        public int InvoiceId { get; set; }
        public string JobType { get; set; }
        public int JobTypeId { get; set; }
        public string Cod { get; set; }
        public bool Pod { get; set; }
        public int Exceptions { get; set; }

        public int InvoicedQty => Exceptions + Clean;

        public int Clean { get; set; }

        public decimal? Credit { get; set; }

        public string Assignee => Domain.ValueObjects.Assignee.GetDisplayNames(Assignees);

        public List<Assignee> Assignees { get; set; }

        public string JobStatusDescription { get; set; }
        public JobStatus JobStatus { get; set; }
        public int StopId { get; set; }
        public string Account { get; set; }
        public string AccountName { get; set; }
        public WellStatus WellStatus { get; set; }
        public string WellStatusDescription { get; set; }
        public int GrnProcessType { get; set; }
        public string GrnNumber { get; set; }
        public string PrimaryAccountNumber { get; set; }
        public int LocationId { get; set; }
        public bool HasUnresolvedActions { get; set; }
        public bool CompletedOnPaper => JobStatus == JobStatus.CompletedOnPaper;

    }
}