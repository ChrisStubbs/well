namespace PH.Well.Api.Models
{
    public class BranchConnection
    {
        public BranchConnection(int branchId, string connectionString)
        {
            BranchId = branchId;
            ConnectionString = connectionString;
        }
        public int BranchId { get; set; }
        public string ConnectionString { get; set; }
    }
}