﻿namespace PH.Well.Domain
{
    using Common.Extensions;

    public class LineItemActionComment : Entity<int>
    {
        public const int CommentReasonIdBulkUpdate = 5;
        public const string NoValue = "No Value";

        public int LineItemActionId { get; set; }

        public int CommentReasonId { get; set; }

        public int? FromQty { get; set; }

        public int ToQty { get; set; }

        public string CommentDescription { get; set; }

        public string DisplayName => this.CreatedBy?.StripDomain();

        public LineItemActionComment Copy()
        {
            return (LineItemActionComment)this.MemberwiseClone();
        }

        public string QtyChangeDescription
        {
            get
            {
                if (!FromQty.HasValue)
                {
                    return $"Quantity: {ToQty} added";
                }
                return $"Quantity changed from: {FromQty}  to {ToQty}";
            }
        }
    }
}