using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Well.Domain;

namespace PH.Well.Repositories.Contracts
{
    public interface IDateThresholdRepository
    {
        IList<DateThreshold> Get();
        Task<IEnumerable<DateThreshold>> GetAsync();
        void Update(DateThreshold dateThreshold, string conectionString);
    }
}
