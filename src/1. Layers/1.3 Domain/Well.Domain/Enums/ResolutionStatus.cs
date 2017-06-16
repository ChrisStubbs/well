using System.Collections.Generic;
using System.Linq;

namespace PH.Well.Domain.Enums
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    public sealed class ResolutionStatus
    {
        private int value;
        private string description;

        private static Dictionary<int, ResolutionStatus> values = new Dictionary<int, ResolutionStatus>
        {
            {1, new ResolutionStatus(1, "Imported")},
            {2, new ResolutionStatus(2, "Driver Completed")},
            {4, new ResolutionStatus(4, "Action Required")},
            {8, new ResolutionStatus(8, "Pending Submission")},
            {16, new ResolutionStatus(16, "Pending Approval")},
            {32, new ResolutionStatus(32, "Credited")},
            {64, new ResolutionStatus(64, "Resolved")},
            {128, new ResolutionStatus(128, "Closed")},
        };

        private static List<int> groupableValues = new List<int>() {2, 32, 64, 128};

        private ResolutionStatus(int value, string description)
        {
            this.value = value;
            this.description = description;
        }

        public int Value
        {
            get { return this.value; }
        }

        public string Description
        {
            get { return this.description; }
        }

        public static ResolutionStatus Imported
        {
            get { return values[1]; }
        }

        public static ResolutionStatus DriverCompleted
        {
            get { return values[2]; }
        }

        public static ResolutionStatus ActionRequired
        {
            get { return values[4]; }
        }

        public static ResolutionStatus PendingSubmission
        {
            get { return values[8]; }
        }

        public static ResolutionStatus PendingApproval
        {
            get { return values[16]; }
        }

        public static ResolutionStatus Credited
        {
            get { return values[32]; }
        }

        public static ResolutionStatus Resolved
        {
            get { return values[64]; }
        }

        public static ResolutionStatus Closed
        {
            get { return values[128]; }
        }

        public static ResolutionStatus Invalid
        {
            get { return new ResolutionStatus(0, "Invalid"); }
        }

        public static ResolutionStatus operator &(ResolutionStatus val1, ResolutionStatus val2)
        {
            if (!(groupableValues.Any(p => p == val1.Value) && groupableValues.Any(p => p == val2.Value)))
            {
                return Invalid;
            }

            return new ResolutionStatus(val1.Value & val2.value, string.Format("{0} - {1}", val1.Description, val2.Description));
        }

        public static ResolutionStatus operator |(ResolutionStatus val1, ResolutionStatus val2)
        {
            if (!(groupableValues.Any(p => p == val1.Value) && groupableValues.Any(p => p == val2.Value)))
            {
                return Invalid;
            }

            return new ResolutionStatus(val1.Value | val2.value, string.Format("{0} - {1}", val1.Description, val2.Description));
        }

        public static explicit operator ResolutionStatus(int value)
        {
            if (values.ContainsKey(value))
            {
                return values[value];
            }

            return Invalid;

        }

        public override int GetHashCode()
        {
            return this.Value;
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
    }
}