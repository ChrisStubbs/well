using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Well.Repositories.Contracts;
using PH.Well.Services.Contracts;

namespace PH.Well.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository activityRepository;
        private readonly IWellStatusAggregator wellStatusAggregator;

        #region Constructors
        public ActivityService(IActivityRepository activityRepository, IWellStatusAggregator wellStatusAggregator)
        {
            this.activityRepository = activityRepository;
            this.wellStatusAggregator = wellStatusAggregator;
        }
        #endregion Constructors

        #region Public methods
        public bool ComputeWellStatus(int activityId)
        {
            // TODO: DIJ - We will not propagate invoice statuses for now
            //var activity = activityRepository.GetActivitySourceById(activityId);
            //if (activity != null)
            //{
            //}
            return false;
        }
        #endregion Public methods
    }
}
