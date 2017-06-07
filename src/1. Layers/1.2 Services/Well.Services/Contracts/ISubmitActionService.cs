﻿namespace PH.Well.Services.Contracts
{
    using Domain.ValueObjects;

    public interface ISubmitActionService
    {
        SubmitActionResult SubmitAction(SubmitActionModel submitAction);
        ActionSubmitSummary GetSubmitSummary(SubmitActionModel submitAction,bool isStopLevel);
    }
}
