namespace PH.Well.Services
{
    using System.Collections.Generic;
    using Contracts;
    using Repositories;
    using Repositories.Contracts;

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
                var amendmentTransaction = this.amendmentFactory.Build(amendment);
                // send amendment to ExceptionEvent table
                this.exceptionEventRepository.InsertAmendmentTransaction(amendmentTransaction);
            }
        }
    }
}
