using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Well.Repositories.Contracts;
using PH.Well.Services.Contracts;

namespace PH.Well.Services
{
    public class ActivityInvoice : IActivityService
    {
        private readonly IActivityRepository activityRepository;
        private readonly IWellStatusAggregator wellStatusAggregator;

        public ActivityInvoice(IActivityRepository activityRepository, IWellStatusAggregator wellStatusAggregator)
        {
            this.activityRepository = activityRepository;
            this.wellStatusAggregator = wellStatusAggregator;
        }

        public bool ComputeWellStatus(int activityId)
        {
            throw new NotImplementedException();
        }
    }
}
