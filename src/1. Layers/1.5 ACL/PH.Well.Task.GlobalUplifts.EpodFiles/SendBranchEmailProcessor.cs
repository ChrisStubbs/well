using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Shared.Well.Data.EF;

namespace PH.Well.Task.GlobalUplifts.EpodFiles
{
    /// <summary>
    /// Send any unsent, complete, Global Uplift attempt emails
    /// </summary>
    public class SendBranchEmailProcessor : ITaskProcessor
    {
        #region Private fields
        private readonly WellEntities _wellEntities;
        #endregion Private fields

        #region Constructors
        public SendBranchEmailProcessor(WellEntities wellEntities)
        {
            _wellEntities = wellEntities;
        }
        #endregion Constructors

        #region Public methods
        public void Run()
        {
            var globalUpliftAttempts = _wellEntities.GlobalUpliftAttempt.Where(x => x.DateBranchEmailSent == null);
            foreach (var globalUpliftAttempt in globalUpliftAttempts)
            {
                
            }
        }
        #endregion Public methods
    }
}
