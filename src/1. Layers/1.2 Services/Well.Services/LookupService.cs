using System;
using System.Collections.Generic;
using PH.Well.Domain.Enums;
using PH.Well.Repositories.Contracts;
using PH.Well.Services.Contracts;

namespace PH.Well.Services
{
    using System.Linq;
    using System.Runtime.Remoting.Messaging;
    using Common.Extensions;

    public class LookupService : ILookupService
    {
        private readonly ILookupRepository lookupRepository;

        public LookupService(ILookupRepository lookupRepository)
        {
            this.lookupRepository = lookupRepository;
        }

        public IList<KeyValuePair<string, string>> GetLookup(LookupType lookupType)
        {
            switch (lookupType)
            {
                case LookupType.ExceptionType:
                    return this.lookupRepository.ExceptionTypes().OrderBy(x=> x.Value).ToList();

                case LookupType.ExceptionAction:
                    return this.lookupRepository.ExceptionActions().OrderBy(x => x.Value).ToList();

                case LookupType.JobStatus:
                    return this.lookupRepository.JobStatus().OrderBy(x => x.Value).ToList();

                case LookupType.JobType:
                    return this.lookupRepository.JobType().OrderBy(x => x.Value).ToList();

                case LookupType.Driver:
                    return this.lookupRepository.Driver().OrderBy(x => x.Value).ToList();

                case LookupType.DeliveryAction:
                    return this.GetDeliveryActions().OrderBy(x => x.Value).ToList();

                case LookupType.JobDetailSource:
                    return this.GetJobDetailSource().OrderBy(x => x.Value).ToList();

                case LookupType.JobDetailReason:
                    return this.GetJobDetailReason().OrderBy(x => x.Value).ToList();

                case LookupType.RouteStatus:
                    return this.GetWellStatus().OrderBy(x => x.Value).Where(x => x.Key != "2") .ToList();

                case LookupType.WellStatus:
                    return this.GetWellStatus().OrderBy(x => x.Value).ToList();

                case LookupType.CommentReason:
                    return this.lookupRepository.CommentReason().OrderBy(x => x.Value).ToList();

                case LookupType.ResolutionStatus:
                    return this.GetResolutionStatus();

                default:
                    throw new ArgumentException($"{lookupType}");
            }
        }

        private IList<KeyValuePair<string, string>> GetJobDetailReason()
        {
            return Enum<JobDetailReason>.GetValuesAndDescriptions().Select(x =>
                   new KeyValuePair<string, string>($"{(int)x.Key}", x.Value)).ToList();
        }

        private IList<KeyValuePair<string, string>> GetJobDetailSource()
        {
            var sources = Enum.GetValues(typeof(JobDetailSource)).Cast<JobDetailSource>().ToList();
            return sources.Select(a =>
                    new KeyValuePair<string, string>($"{(int) a}", StringExtensions.GetEnumDescription(a))).ToList();
        }

        private IList<KeyValuePair<string, string>> GetDeliveryActions()
        {
            IEnumerable<DeliveryAction> actions = new List<DeliveryAction>()
                {
                    DeliveryAction.NotDefined,
                    DeliveryAction.Credit,
                    DeliveryAction.Close
                };
            return actions.Select(a =>
                new KeyValuePair<string, string>($"{(int)a}", StringExtensions.GetEnumDescription(a))).ToList();
        }

        private IList<KeyValuePair<string, string>> GetRouteStatus()
        {
            return Enum<RouteStatus>.GetValuesAndDescriptions().Select(x =>
                   new KeyValuePair<string, string>($"{(int)x.Key}", x.Value)).ToList();
        }

        private IList<KeyValuePair<string, string>> GetWellStatus()
        {
            return Enum<WellStatus>.GetValuesAndDescriptions().Select(x =>
                new KeyValuePair<string, string>($"{(int)x.Key}", x.Value)).ToList();
        }

        private IList<KeyValuePair<string, string>> GetResolutionStatus()
        {
           return  ResolutionStatus.Values.Select(x =>
                new KeyValuePair<string, string>(x.Value.Value.ToString(), x.Value.Description)).ToList();

        }
    }
}
