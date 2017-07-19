namespace PH.Well.Domain.ValueObjects
{
    using System.Collections.Generic;
    using System.Linq;

    public class AmendmentTransaction
    {
        public AmendmentTransaction()
        {
            this.LineSql = new Dictionary<int, string>();
        }

        public string HeaderSql { get; set; }

        public Dictionary<int, string> LineSql { get; set; }

        public bool CanWriteHeader => !this.LineSql.Any();

        public int BranchId { get; set; }

    }
}
