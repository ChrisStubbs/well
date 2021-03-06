﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PH.Shared.Well.Data.EF;
using PH.Well.Domain.Enums;
using PH.Well.Domain.ValueObjects;

namespace PH.Well.Task.GlobalUplifts.EpodFiles
{
    /// <summary>
    /// Generate global uplift ADAM events for any unsent, complete, Global uplifts or Global Uplift attempts
    /// </summary>
    public class GenerateGlobalUpliftEventProcessor : ITaskProcessor
    {
        #region Constants
        private const int TWO_WEEKS = 14;
        #endregion Constants

        #region Private fields
        private readonly WellEntities _wellEntities;
        #endregion Private fields

        #region Constructors
        public GenerateGlobalUpliftEventProcessor(WellEntities wellEntities)
        {
            _wellEntities = wellEntities;
        }
        #endregion Constructors

        #region public events
        public void Run()
        {
            var globalUpliftAttempts = _wellEntities.GlobalUpliftAttempt.Include("GlobalUplift").Where(x => x.DateSentToAdam == null).ToList();
            foreach (var globalUpliftAttempt in globalUpliftAttempts)
            {
                // Tell ADAM
                SendGlobalUpliftAttempt(globalUpliftAttempt, globalUpliftAttempt.GlobalUplift);
            }
        }
        #endregion public events

        #region private helper methods

        private void SendGlobalUpliftAttempt(GlobalUpliftAttempt attempt, GlobalUplift globalUplift)
        {
            var upliftEvent = new GlobalUpliftEvent()
            {
                Id = attempt.Id,
                BranchId = globalUplift.BranchId,
                AccountNumber = globalUplift.PHAccount,
                ProductCode = int.Parse(globalUplift.PHProductCode),
                Quantity = attempt.CollectedQty.GetValueOrDefault(),
                CreditReasonCode = "24",
                CsfNumber = int.Parse(globalUplift.CsfReference),
                CustomerReference = globalUplift.CustomerReference,
                WriteLine = true,
                WriteHeader = true,
                StartDate = globalUplift.StartDate.GetValueOrDefault(attempt.DateAttempted),
                EndDate = globalUplift.EndDate.GetValueOrDefault(attempt.DateAttempted.AddDays(TWO_WEEKS))
            };

            // Make sure we have not already sent this event
            var @event = _wellEntities.ExceptionEvent.FirstOrDefault(
                x => x.ExceptionActionId == (int)EventAction.GlobalUplift && x.SourceId == attempt.Id.ToString());
            if (@event == null)
            {
                // Add to the event log
                var exceptionEvent = new ExceptionEvent
                {
                    Event = JsonConvert.SerializeObject(upliftEvent),
                    ExceptionActionId = (int)EventAction.GlobalUplift,
                    DateCanBeProcessed = DateTime.Now,
                    SourceId = attempt.Id.ToString()
                };

                _wellEntities.ExceptionEvent.Add(exceptionEvent);
                attempt.DateSentToAdam = DateTime.Now;
                _wellEntities.SaveChanges();
            }
        }
        #endregion private helper methods
    }
}
