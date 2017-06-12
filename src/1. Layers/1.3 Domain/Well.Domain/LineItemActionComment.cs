namespace PH.Well.Domain
{
    public class LineItemActionComment: Entity<int>
    {
        public int LineItemActionId { get; set; }
        public int CommentReasonId { get; set; }
        public string CommentDescription { get; set; }
    }
}