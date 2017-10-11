namespace PH.Well.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Contracts;
    using Repositories;
    using Repositories.Contracts;
    using System.Linq;

    public class AmendmentService : IAmendmentService
    {
        private readonly IAmendmentRepository amendmentRepository;
        private readonly IAmendmentFactory amendmentFactory;
        private readonly IExceptionEventRepository exceptionEventRepository;

        public AmendmentService(IAmendmentRepository amendmentRepository, IAmendmentFactory amendmentFactory, IExceptionEventRepository exceptionEventRepository)
        {
            this.amendmentRepository = amendmentRepository;
            this.amendmentFactory = amendmentFactory;
            this.exceptionEventRepository = exceptionEventRepository;
        }

       
        public void ProcessAmendments(IEnumerable<int> jobIds)
        {
            var amendments = this.amendmentRepository.GetAmendments(jobIds);

            foreach (var amendment in amendments)
            {
                if (amendment.AmendmentLines.Count > 0)
                {
                    var amendmentTransaction = this.amendmentFactory.Build(amendment);
                    // send amendment to ExceptionEvent table
                    this.exceptionEventRepository.InsertAmendmentTransaction(amendmentTransaction);
                }
            }
        }
    }
}
