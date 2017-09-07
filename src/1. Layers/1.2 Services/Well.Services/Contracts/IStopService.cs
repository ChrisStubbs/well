﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Well.Domain;

namespace PH.Well.Services.Contracts
{
    public interface IStopService
    {
        /// <summary>
        /// Force the stop to recalculate its status from the child jobs
        /// </summary>
        /// <param name="stopId">PK of stop to check</param>
        /// <returns>true if the current status changed</returns>
        bool ComputeWellStatus(int stopId);

        bool ComputeWellStatus(Stop stop);

        bool ComputeAndPropagateWellStatus(int stopId);

        bool ComputeAndPropagateWellStatus(Stop stop);
    }
}
