using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Well.Domain.ValueObjects;

namespace PH.Well.Repositories.Contracts
{
    /// <summary>
    /// Defines factory for creating ADAM sql for global uplifts
    /// </summary>
    public interface IGlobalUpliftTransactionFactory
    {
        string LineSql(GlobalUpliftTransaction transaction);

        string HeaderSql(GlobalUpliftTransaction transaction);
    }
}
