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
        private const int ResolutionStatusInvalid = 0;
        private const int ResolutionStatusImported = 1;
        private const int ResolutionStatusDriverCompleted = 2;
        private const int ResolutionStatusActionRequired = 4;
        private const int ResolutionStatusPendingSubmission = 8;
        private const int ResolutionStatusPendingApproval = 16;
        private const int ResolutionStatusApproved = 32;
        private const int ResolutionStatusCredited = 64;
        private const int ResolutionStatusResolved = 128;
        private const int ResolutionStatusClosed = 256;
        private const int ResolutionStatusManuallyCompleted = 512;

    
        private static readonly Dictionary<int, ResolutionStatus> Values = new Dictionary<int, ResolutionStatus>
        {
            { ResolutionStatusInvalid, new ResolutionStatus(ResolutionStatusInvalid, "Invalid")},
            { ResolutionStatusImported, new ResolutionStatus(ResolutionStatusImported, "Imported")},
            { ResolutionStatusDriverCompleted, new ResolutionStatus(ResolutionStatusDriverCompleted, "Driver Completed")},
            { ResolutionStatusActionRequired, new ResolutionStatus(ResolutionStatusActionRequired, "Action Required")},
            { ResolutionStatusPendingSubmission, new ResolutionStatus(ResolutionStatusPendingSubmission, "Pending Submission")},
            { ResolutionStatusPendingApproval, new ResolutionStatus(ResolutionStatusPendingApproval, "Pending Approval")},
            { ResolutionStatusApproved, new ResolutionStatus(ResolutionStatusApproved, "Approved")},
            { ResolutionStatusCredited, new ResolutionStatus(ResolutionStatusCredited, "Credited")},
            { ResolutionStatusResolved, new ResolutionStatus(ResolutionStatusResolved, "Resolved")},
            { ResolutionStatusClosed, new ResolutionStatus(ResolutionStatusClosed, "Closed")},
            { ResolutionStatusManuallyCompleted, new ResolutionStatus(ResolutionStatusManuallyCompleted, "Manually Completed")},
        };

        /// <summary>
        /// List of just the statuses that can be combined together
        /// </summary>
        private static readonly List<int> CombinableStatuses = new List<int>()
        {
            ResolutionStatusDriverCompleted,
            ResolutionStatusCredited,
            ResolutionStatusResolved,
            ResolutionStatusClosed,
            ResolutionStatusManuallyCompleted
        };

        private static readonly List<int> CombinableAndStatuses = new List<int>()
        {
            ResolutionStatusDriverCompleted | ResolutionStatusClosed,
            ResolutionStatusCredited | ResolutionStatusClosed,
            ResolutionStatusResolved | ResolutionStatusClosed,
            ResolutionStatusClosed,
            ResolutionStatusManuallyCompleted | ResolutionStatusClosed
        };

        #region Properties
        public int Value { get; }
        public string Description { get; }
        public static ResolutionStatus Imported => Values[ResolutionStatusImported];
        public static ResolutionStatus DriverCompleted => Values[ResolutionStatusDriverCompleted];
        public static ResolutionStatus ManuallyCompleted => Values[ResolutionStatusManuallyCompleted];
        public static ResolutionStatus ActionRequired => Values[ResolutionStatusActionRequired];
        public static ResolutionStatus PendingSubmission => Values[ResolutionStatusPendingSubmission];
        public static ResolutionStatus PendingApproval => Values[ResolutionStatusPendingApproval];
        public static ResolutionStatus Approved => Values[ResolutionStatusApproved];
        public static ResolutionStatus Credited => Values[ResolutionStatusCredited];
        public static ResolutionStatus Resolved => Values[ResolutionStatusResolved];
        public static ResolutionStatus Closed => Values[ResolutionStatusClosed];
        public static ResolutionStatus Invalid => Values[ResolutionStatusInvalid];
        public static IList<ResolutionStatus> AllStatus => ResolutionStatus.Values.Select(p => p.Value).ToList();
        #endregion Properties

        #region Constructor

        private ResolutionStatus(int resolutionStatus, string description)
        {
            this.Value = resolutionStatus;
            this.Description = description;
        }
        #endregion Constructor

        #region Operators
        public static ResolutionStatus operator &(ResolutionStatus val1, ResolutionStatus val2)
        {
            if (!(CombinableAndStatuses.Any(p => p == val1.Value) && CombinableAndStatuses.Any(p => p == val2.Value)))
            {
                return Invalid;
            }

            int v = val1.Value & val2.Value;
            return (ResolutionStatus)v;
        }

        public static ResolutionStatus operator |(ResolutionStatus val1, ResolutionStatus val2)
        {
            if (!(CombinableStatuses.Any(p => p == val1.Value) && CombinableStatuses.Any(p => p == val2.Value)))
            {
                return Invalid;
            }

            return new ResolutionStatus(val1.Value | val2.Value, $"{val1.Description} - {val2.Description}");
        }
        
        public static implicit operator ResolutionStatus(int value)
        {
            var valueToHandle = value;

            if (Values.ContainsKey(valueToHandle))
            {
                return Values[valueToHandle];
            }

            if (valueToHandle == (Closed.Value | DriverCompleted.Value))
            {
                return Closed | DriverCompleted;
            }

            if (valueToHandle == (Closed.Value | Credited.Value))
            {
                return Closed | Credited;
            }

            if (valueToHandle == (Closed.Value | Resolved.Value))
            {
                return Closed | Resolved;
            }

            if (valueToHandle == (Closed.Value | ManuallyCompleted.Value))
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

        public static bool operator >(int val1, ResolutionStatus val2)
        {
            return val1 > val2.Value;
        }

        public static bool operator <(int val1, ResolutionStatus val2)
        {
            return val1 < val2.Value;
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