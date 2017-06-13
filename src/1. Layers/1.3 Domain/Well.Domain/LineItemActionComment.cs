namespace PH.Well.Domain
{
    using Common.Extensions;

    public class LineItemActionComment: Entity<int>
    {
        public int LineItemActionId { get; set; }
        public int CommentReasonId { get; set; }
        public string CommentDescription { get; set; }
        public string DisplayName => this.CreatedBy.StripDomain();
    }
}