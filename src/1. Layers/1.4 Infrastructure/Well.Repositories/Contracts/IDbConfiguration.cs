using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.Repositories.Contracts
{
    public interface IDbConfiguration
    {
        string DatabaseConnection { get; }
        int TransactionTimeout { get; }
        int MaxNoOfDeadlockRetries { get; }
        int? CommandTimeout { get; }
    }
}
