using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using PH.Well.Domain.ValueObjects;
using PH.Well.Repositories.Contracts;
using PH.Well.Services.Contracts;
using System;
using PH.Well.Domain.Enums;

namespace PH.Well.Services
{
    public class DeliveryLinesClose : IDeliveryLinesAction
    {
        private readonly IJobRepository jobRepository;
        private readonly IUserRepository userRepository;

        public DeliveryLinesClose(IJobRepository jobRepository, IUserRepository userRepository)
        {
            this.jobRepository = jobRepository;
            this.userRepository = userRepository;
        }

        public DeliveryAction Action
        {
            get
            {
                return DeliveryAction.Close;
            }
        }

        public ProcessDeliveryActionResult Execute(
            Func<DeliveryAction, IList<DeliveryLine>> deliveryLines, 
            AdamSettings adamSettings, 
            int branchId)
        {
            var lines = deliveryLines(this.Action);

            if (lines.Any())
            {
                this.jobRepository.ResolveJobAndJobDetails(lines[0].JobId);
                this.userRepository.UnAssignJobToUser(lines[0].JobId);
            }

            return new ProcessDeliveryActionResult();
        }
    }
}
