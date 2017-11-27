namespace PH.Well.Domain.ValueObjects
{
    public class DocumentRecirculationTransaction
    {
        // stop id
        public int Id { get; set; }

        public int BranchId { get; set; }

        public string HeaderSql { get; set; }

    }
}
