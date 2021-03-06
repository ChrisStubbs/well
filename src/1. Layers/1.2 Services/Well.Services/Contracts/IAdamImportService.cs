﻿namespace PH.Well.Services.Contracts
{
    using System;
    using PH.Well.Domain;

    public interface IAdamImportService
    {
        void Import(RouteDelivery route, string fileName, IImportConfig config, out bool hasErrors);
    }
}