namespace PH.Well.Api.Models
{
    public class BranchConnection
    {
        public BranchConnection(int branchId, string connectionString, ConnectionType connectionType)
        {
            BranchId = branchId;
            ConnectionString = connectionString;
            Type = connectionType;
        }
        
        public int BranchId { get; set; }
        public string ConnectionString { get; set; }

        public ConnectionType Type { get; set; }
    }
}