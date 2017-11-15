using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Well.Repositories.Contracts;
using PH.Well.Services.Contracts;

namespace PH.Well.Services
{
    //TODO: this class it's not been used. Check git history and talk to whoever wrote this code
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
            //TODO: see what's to be done with this. Check git history and talk to whoever wrote this code
            throw new NotImplementedException();
        }
    }
}
