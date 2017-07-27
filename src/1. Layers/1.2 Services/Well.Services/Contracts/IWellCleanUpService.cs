using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.Services.Contracts
{
    public interface IWellCleanUpService
    {
        Task SoftDelete();
    }
}
