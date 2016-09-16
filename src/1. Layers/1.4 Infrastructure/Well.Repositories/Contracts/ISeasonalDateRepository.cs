﻿namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;

    using PH.Well.Domain;

    public interface ISeasonalDateRepository : IRepository<SeasonalDate, int>
    {
        IEnumerable<SeasonalDate> GetAll();
    }
}
