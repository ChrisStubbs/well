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
                    return this.lookupRepository.JobType();

                case LookupType.Driver:
                    return this.lookupRepository.Driver();

                case LookupType.DeliveryAction:
                    return this.GetDeliveryActions();

                case LookupType.JobDetailSource:
                    return this.GetJobDetailSource();

                case LookupType.JobDetailReason:
                    return this.GetJobDetailReason();

                case LookupType.RouteStatus:
                    return this.GetWellStatus().Where(x => x.Key != "2") .ToList();

                case LookupType.WellStatus:
                    return this.GetWellStatus().ToList();

                case LookupType.CommentReason:
                    return this.lookupRepository.CommentReason();

                case LookupType.ResolutionStatus:
                    return this.GetResolutionStatus();

                case LookupType.JobIssueType:
                    return GetJobIssueType();

                default:
                    throw new ArgumentException($"{lookupType}");
            }
        }

        private IList<KeyValuePair<string, string>> GetJobIssueType()
        {
            var sources = Enum.GetValues(typeof(JobIssueType)).Cast<JobIssueType>().ToList();
            var values = sources
                .Select(a => new KeyValuePair<string, string>($"{(int)a}", a.Description()))
                .ToList();

            values.Add(new KeyValuePair<string, string>(
                ((int)(JobIssueType.ActionRequired | JobIssueType.MissingGRN | JobIssueType.PendingSubmission)).ToString(),
                "All Outstanding"));

            return values;
        }

        private IList<KeyValuePair<string, string>> GetJobDetailReason()
        {
            var sources = Enum.GetValues(typeof(JobDetailReason)).Cast<JobDetailReason>().ToList();
            return sources
                .Select(a => new KeyValuePair<string, string>($"{(int)a}", a.Description()))
                .ToList();
        }

        private IList<KeyValuePair<string, string>> GetJobDetailSource()
        {
            var sources = Enum.GetValues(typeof(JobDetailSource)).Cast<JobDetailSource>().ToList();
            return sources
                .Select(a => new KeyValuePair<string, string>($"{(int) a}", a.Description()))
                .ToList();
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

        private IList<KeyValuePair<string, string>> GetWellStatus()
        {
            var sources = Enum.GetValues(typeof(WellStatus)).Cast<WellStatus>().ToList();

            return sources
                .Where(p => p != WellStatus.Unknown)
                .Select(a => new KeyValuePair<string, string>($"{(int)a}", a.Description()))
                .ToList();
        }

        private IList<KeyValuePair<string, string>> GetResolutionStatus()
        {
           return  ResolutionStatus.AllStatus
                .Select(x => new KeyValuePair<string, string>(x.Value.ToString(), x.Description))
                .ToList();

        }
    }
}
