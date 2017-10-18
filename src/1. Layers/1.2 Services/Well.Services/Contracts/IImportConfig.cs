using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Well.Domain.Enums;

namespace PH.Well.Services.Contracts
{
    public interface IImportConfig
    {
        bool ProcessDataForBranch(Branch branch);
    }
}
