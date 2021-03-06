﻿namespace PH.Well.Domain.Enums
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Extensions;

    /// <summary>
    /// These enums should sync with the table WellStatus
    /// </summary>
    public enum WellStatus
    {
        [Description("Not defined")]
        Unknown = 0,

        [Description("Planned")]
        Planned = 1,

        [Description("Invoiced")]
        Invoiced = 2,

        [Description("Complete")]
        Complete = 3,

        [Description("Bypassed")]
        Bypassed = 4,

        [Description("In Progress")]
        RouteInProgress = 5,

        [Description("Replanned")]
        Replanned = 7
    }

    public static class WellStatuses
    {
        public static WellStatus[] OrderedWellStatuses =
        {
            WellStatus.Planned,
            WellStatus.Invoiced,
            WellStatus.Replanned,
            WellStatus.RouteInProgress,
            WellStatus.Complete,
            WellStatus.Bypassed
        };

        public static WellStatus[] OrderedJobStatuses =
        {
            WellStatus.Planned,
            WellStatus.Invoiced,
            WellStatus.Replanned,
            WellStatus.RouteInProgress,
            WellStatus.Complete,
            WellStatus.Bypassed
        };

        public static WellStatus[] OrderedStopStatuses =
        {
            WellStatus.Planned, 
            WellStatus.Invoiced,
            WellStatus.RouteInProgress, 
            WellStatus.Complete, 
            WellStatus.Bypassed
        };

        public static WellStatus[] OrderedRouteStatuses =
        {
            WellStatus.Planned,
            WellStatus.Invoiced,
            WellStatus.RouteInProgress,
            WellStatus.Complete,
            WellStatus.Bypassed
        };

        /// <summary>
        /// Private cache of strings by enum
        /// </summary>
        private static readonly Dictionary<WellStatus, string> descriptions;

        /// <summary>
        /// Static constructor fills cache using reflected descriptions
        /// </summary>
        static WellStatuses()
        {
            descriptions = new Dictionary<WellStatus, string>();
            foreach (WellStatus item in System.Enum.GetValues(typeof(WellStatus)))
            {
                descriptions.Add(item, EnumExtensions.GetDescription(item));
            }
        }

        /// <summary>
        /// Return the name of the specified WellStatus enum
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Description(this WellStatus value)
        {
            return descriptions[value];
        }

        /// <summary>
        /// Return an ordered list of key/value pairs for the specified list of WellStatus enums
        /// </summary>
        /// <param name="statuses"></param>
        /// <returns></returns>
        /// <remarks>Used for drop-down list generation</remarks>
        public static IEnumerable<KeyValuePair<string, string>> GetKeyValuePairs(IEnumerable<WellStatus> statuses)
        {
            foreach (var wellStatus in statuses)
            {
                yield return new KeyValuePair<string, string>(((int)wellStatus).ToString(), Description(wellStatus));
            }
        }
    }
}
