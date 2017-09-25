using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Well.Domain;

namespace PH.Well.Services.Contracts
{
    public interface IStopService
    {
        void ComputeWellStatus(IList<int> stopId);
        
        bool ComputeAndPropagateWellStatus(Stop stop);

        bool ComputeAndPropagateWellStatus(int stopId);
    }
}
