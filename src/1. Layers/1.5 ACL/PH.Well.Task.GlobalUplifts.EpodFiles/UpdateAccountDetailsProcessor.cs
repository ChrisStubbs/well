using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Shared.EmailService.Client.Rest;
using PH.Shared.EmailService.Models;
using PH.Shared.Well.Data.EF;
using PH.Well.Services;
using Branch = PH.Well.Domain.Enums.Branch;

namespace PH.Well.Task.GlobalUplifts.EpodFiles
{
    /// <summary>
    /// Fetch any missing account details for Global Uplifts
    /// </summary>
    public class UpdateAccountDetailsProcessor : ITaskProcessor
    {
        #region Private fields
        private readonly WellEntities _wellEntities;
        #endregion Private fields

        #region Constructors
        public UpdateAccountDetailsProcessor(WellEntities wellEntities)
        {
            _wellEntities = wellEntities;
        }
        #endregion Constructors

        #region Public methods
        public void Run()
        {
            var globalUpliftAttempts = _wellEntities.GlobalUpliftAttempt.Include("GlobalUplift").Where(x => x.DateBranchEmailSent == null && x.CollectedQty > 0 && x.GlobalUplift.HasAccountInfo);
            foreach (var globalUpliftAttempt in globalUpliftAttempts)
            {
                // Use the Account service to fetch account details for these global uplifts

            }
        }
        #endregion Public methods
    }
}
