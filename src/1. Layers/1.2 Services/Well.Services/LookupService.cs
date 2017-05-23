using System;
using System.Collections.Generic;
using PH.Well.Domain.Enums;
using PH.Well.Repositories.Contracts;
using PH.Well.Services.Contracts;

namespace PH.Well.Services
{
    using System.Linq;
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
                    return this.lookupRepository.ExceptionTypes();

                case LookupType.ExceptionAction:
                    return this.lookupRepository.ExceptionActions();

                case LookupType.JobStatus:
                    return this.lookupRepository.JobStatus();

                case LookupType.JobType:
                    return this.lookupRepository.JobType();

                case LookupType.Driver:
                    return this.lookupRepository.Driver();

                case LookupType.DeliveryAction:
                    return this.GetDeliveryActions();

                case LookupType.JobDetailSource:
                    return this.GetJobDetailSource();

                case LookupType.JobDetailReason:
                    return this.GetJobDetailReason();

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
                    DeliveryAction.MarkAsBypassed,
                    DeliveryAction.MarkAsDelivered
                };
            return actions.Select(a =>
                new KeyValuePair<string, string>($"{(int)a}", StringExtensions.GetEnumDescription(a))).ToList();
        }
    }
}
