using System.ComponentModel;
using PH.Well.Domain.Extensions;

namespace PH.Well.Domain.Enums
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [Serializable]
    public sealed class ResolutionStatus
    {
        [Flags]
        public enum eResolutionStatus
        {
            [Description("Invalid")]
            Invalid = 0,
            [Description("Imported")]
            Imported = 1,
            [Description("Driver Completed")]
            DriverCompleted = 2,
            [Description("Action Required")]
            ActionRequired = 4,
            [Description("Pending Submission")]
            PendingSubmission = 8,
            [Description("Pending Approval")]
            PendingApproval = 16,
            [Description("Approved")]
            Approved = 32,
            [Description("Credited")]
            Credited = 64,
            [Description("Resolved")]
            Resolved = 128,
            [Description("Closed")]
            Closed = 256,
            [Description("Manually Completed")]
            ManuallyCompleted = 512
        }

        #region Static data
        private static readonly Dictionary<eResolutionStatus, ResolutionStatus> Values = new Dictionary<eResolutionStatus, ResolutionStatus>
        {
            { eResolutionStatus.Invalid, new ResolutionStatus(eResolutionStatus.Invalid)},
            { eResolutionStatus.Imported, new ResolutionStatus(eResolutionStatus.Imported)},
            { eResolutionStatus.DriverCompleted, new ResolutionStatus(eResolutionStatus.DriverCompleted)},
            { eResolutionStatus.ActionRequired, new ResolutionStatus(eResolutionStatus.ActionRequired)},
            { eResolutionStatus.PendingSubmission, new ResolutionStatus(eResolutionStatus.PendingSubmission)},
            { eResolutionStatus.PendingApproval, new ResolutionStatus(eResolutionStatus.PendingApproval)},
            { eResolutionStatus.Approved, new ResolutionStatus(eResolutionStatus.Approved)},
            { eResolutionStatus.Credited, new ResolutionStatus(eResolutionStatus.Credited)},
            { eResolutionStatus.Resolved, new ResolutionStatus(eResolutionStatus.Resolved)},
            { eResolutionStatus.Closed, new ResolutionStatus(eResolutionStatus.Closed)},
            { eResolutionStatus.ManuallyCompleted, new ResolutionStatus(eResolutionStatus.ManuallyCompleted)},
        };

        /// <summary>
        /// List of just the statuses that can be combined together
        /// </summary>
        private static readonly List<eResolutionStatus> CombinableStatuses = new List<eResolutionStatus>()
        {
            eResolutionStatus.DriverCompleted,
            eResolutionStatus.Credited,
            eResolutionStatus.Resolved,
            eResolutionStatus.Closed,
            eResolutionStatus.ManuallyCompleted
        };

        private static readonly List<eResolutionStatus> CombinableAndStatuses = new List<eResolutionStatus>()
        {
            eResolutionStatus.DriverCompleted | eResolutionStatus.Closed,
            eResolutionStatus.Credited | eResolutionStatus.Closed,
            eResolutionStatus.Resolved | eResolutionStatus.Closed,
            eResolutionStatus.Closed,
            eResolutionStatus.ManuallyCompleted | eResolutionStatus.Closed
        };

        #endregion Static data

        #region Properties
        public eResolutionStatus eValue { get; }
        public int Value => (int) eValue;
        public string Description { get; }
        public static ResolutionStatus Imported => Values[eResolutionStatus.Imported];
        public static ResolutionStatus DriverCompleted => Values[eResolutionStatus.DriverCompleted];
        public static ResolutionStatus ManuallyCompleted => Values[eResolutionStatus.ManuallyCompleted];
        public static ResolutionStatus ActionRequired => Values[eResolutionStatus.ActionRequired];
        public static ResolutionStatus PendingSubmission => Values[eResolutionStatus.PendingSubmission];
        public static ResolutionStatus PendingApproval => Values[eResolutionStatus.PendingApproval];
        public static ResolutionStatus Approved => Values[eResolutionStatus.Approved];
        public static ResolutionStatus Credited => Values[eResolutionStatus.Credited];
        public static ResolutionStatus Resolved => Values[eResolutionStatus.Resolved];
        public static ResolutionStatus Closed => Values[eResolutionStatus.Closed];
        public static ResolutionStatus Invalid => Values[eResolutionStatus.Invalid];
        public static IList<ResolutionStatus> AllStatus => ResolutionStatus.Values.Select(p => p.Value).ToList();
        #endregion Properties

        #region Constructor
        private ResolutionStatus(eResolutionStatus resolutionStatus)
        {
            this.eValue = resolutionStatus;
            this.Description = EnumExtensions.GetDescription(resolutionStatus);
        }

        private ResolutionStatus(eResolutionStatus resolutionStatus, string description)
        {
            this.eValue = resolutionStatus;
            this.Description = description;
        }
        #endregion Constructor

        #region Operators
        public static ResolutionStatus operator &(ResolutionStatus val1, ResolutionStatus val2)
        {
            if (!(CombinableAndStatuses.Any(p => p == val1.eValue) && CombinableAndStatuses.Any(p => p == val2.eValue)))
            {
                return Invalid;
            }

            int v = val1.Value & val2.Value;
            return (ResolutionStatus)v;
        }

        public static ResolutionStatus operator |(ResolutionStatus val1, ResolutionStatus val2)
        {
            if (!(CombinableStatuses.Any(p => p == val1.eValue) && CombinableStatuses.Any(p => p == val2.eValue)))
            {
                return Invalid;
            }

            return new ResolutionStatus(val1.eValue | val2.eValue, $"{val1.Description} - {val2.Description}");
        }
        
        public static implicit operator ResolutionStatus(int value)
        {
            var valueToHandle = (eResolutionStatus)value;

            if (Values.ContainsKey(valueToHandle))
            {
                return Values[valueToHandle];
            }

            if (valueToHandle == (Closed.eValue | DriverCompleted.eValue))
            {
                return Closed | DriverCompleted;
            }

            if (valueToHandle == (Closed.eValue | Credited.eValue))
            {
                return Closed | Credited;
            }

            if (valueToHandle == (Closed.eValue | Resolved.eValue))
            {
                return Closed | Resolved;
            }

            if (valueToHandle == (Closed.eValue | ManuallyCompleted.eValue))
            {
                return Closed | ManuallyCompleted;
            }

            return Invalid;
        }

        public static bool operator ==(ResolutionStatus val1, ResolutionStatus val2)
        {
            if (object.ReferenceEquals(val1, null) && object.ReferenceEquals(val2, null))
            {
                return true;
            }

            if (object.ReferenceEquals(val1, null) || object.ReferenceEquals(val2, null))
            {
                return false;
            }

            return val1.Value == val2.Value;
        }

        public static bool operator !=(ResolutionStatus val1, ResolutionStatus val2)
        {
            return !(val1 == val2);
        }

        public static bool operator >=(ResolutionStatus val1, ResolutionStatus val2)
        {
            if (object.ReferenceEquals(val1, null) || object.ReferenceEquals(val2, null))
            {
                return false;
            }
            return val1.Value >= val2.Value;
        }

        public static bool operator <=(ResolutionStatus val1, ResolutionStatus val2)
        {
            return val1 == val2 || !(val1 >= val2);
        }

        public override string ToString()
        {
            return $"{this.Value} - {this.Description}";
        }
        #endregion Operators

        #region Overrides
        public override int GetHashCode()
        {
            return this.Value;
        }

        public override bool Equals(object obj)
        {
            var item = obj as ResolutionStatus;

            if (object.ReferenceEquals(item, null))
            {
                return false;
            }

            return this == item;
        }
        #endregion Overrides

    }
}