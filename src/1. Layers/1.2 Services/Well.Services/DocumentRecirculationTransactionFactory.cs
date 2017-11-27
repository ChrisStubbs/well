namespace PH.Well.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using Contracts;
    using Domain.Enums;
    using Domain.ValueObjects;

    using PH.Well.Common.Contracts;
    using PH.Well.Common.Extensions;

    using Repositories.Contracts;

    public class DocumentRecirculationTransactionFactory : IDocumentRecirculationFactory
    {
        private readonly IUserRepository userRepository;
        private readonly IUserNameProvider userNameProvider;

        public DocumentRecirculationTransactionFactory(
            
            IUserRepository userRepository,
            IUserNameProvider userNameProvider)
        {
            this.userRepository = userRepository;
            this.userNameProvider = userNameProvider;
        }

        public DocumentRecirculationTransaction Build(DateTime routeDeliveryDate,int routeNumber, int stopNumber, int stopId, int branchId)
        {
            var username = this.userNameProvider.GetUserName();
            var user = this.userRepository.GetByIdentity(username);
            //ADAM needs the user initials & well identifier
            var initials = user.Name.GetInitials();
            var wellName = "The Well";

            var endFlag = 0;
            var deliveryDate = routeDeliveryDate.ToShortDateString();

            var transactionHeader =
                $"INSERT INTO WELLHEAD (WELLHDCREDAT, WELLHDGUID, WELLHDRCDTYPE, WELLHDOPERATOR, WELLHDBRANCH, WELLHDCUSTREF, WELLHDPODCODE) " +
                $"VALUES('{deliveryDate}', '{stopId}', '{(int)EventAction.RecirculateDocuments}', '{initials}', {branchId}, {stopNumber}, {routeNumber}');";

            var transaction = new DocumentRecirculationTransaction()
            {
                HeaderSql = transactionHeader,
                BranchId = branchId,
            };

            return transaction;
        }
    }
}
