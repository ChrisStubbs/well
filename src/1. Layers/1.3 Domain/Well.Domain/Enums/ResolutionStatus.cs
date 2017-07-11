namespace PH.Well.Domain.Enums
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [Serializable]
    public sealed class ResolutionStatus
    {
        private int value;
        private string description;
        private static Dictionary<int, ResolutionStatus> Values = new Dictionary<int, ResolutionStatus>
        {
            { 1, new ResolutionStatus(1, "Imported")},
            { 2, new ResolutionStatus(2, "Driver Completed")},
            { 4, new ResolutionStatus(4, "Action Required")},
            { 8, new ResolutionStatus(8, "Pending Submission")},
            { 16, new ResolutionStatus(16, "Pending Approval")},
            { 32, new ResolutionStatus(32, "Approved")},
            { 64, new ResolutionStatus(64, "Credited")},
            { 128, new ResolutionStatus(128, "Resolved")},
            { 256, new ResolutionStatus(256, "Closed")},
            { 512, new ResolutionStatus(512, "Completed By Well")},
        };

        private static List<int> groupableValues = new List<int>() { 2, 64, 128, 256 };

        private ResolutionStatus(int value, string description)
        {
            this.value = value;
            this.description = description;
        }

        public int Value
        {
            get
            {
                return this.value;
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
        }

        public static ResolutionStatus Imported
        {
            get
            {
                return Values[1];
            }
        }

        public static ResolutionStatus DriverCompleted
        {
            get
            {
                return Values[2];
            }
        }

        public static ResolutionStatus CompletedByWell
        {
            get
            {
                return Values[512];
            }
        }

        public static ResolutionStatus ActionRequired
        {
            get
            {
                return Values[4];
            }
        }

        public static ResolutionStatus PendingSubmission
        {
            get
            {
                return Values[8];
            }
        }

        public static ResolutionStatus PendingApproval
        {
            get
            {
                return Values[16];
            }
        }

        public static ResolutionStatus Approved
        {
            get
            {
                return Values[32];
            }
        }

        public static ResolutionStatus Credited
        {
            get
            {
                return Values[64];
            }
        }

        public static ResolutionStatus Resolved
        {
            get
            {
                return Values[128];
            }
        }

        public static ResolutionStatus Closed
        {
            get
            {
                return Values[256];
            }
        }

        public static ResolutionStatus Invalid
        {
            get
            {
                return new ResolutionStatus(0, "Invalid");
            }
        }

        public static IList<ResolutionStatus> AllStatus
        {
            get
            {
                return ResolutionStatus.Values
                    .Select(p => p.Value)
                    .ToList();
            }
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

        public static implicit operator ResolutionStatus(int value)
        {
            if (Values.ContainsKey(value))
            {
                return Values[value];
            }

            if (value == (Closed.Value | DriverCompleted.Value))
            {
                return Closed | DriverCompleted;
            }

            if (value == (Closed.Value | Credited.Value))
            {
                return Closed | Credited;
            }

            if (value == (Closed.Value | Resolved.Value))
            {
                return Closed | Resolved;
            }

            return Invalid;
        }

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
            
            return val1.value >= val2.value;
        }

        public static bool operator <=(ResolutionStatus val1, ResolutionStatus val2)
        {
            return val1 == val2 || !(val1 >= val2);
        }

        public override string ToString()
        {
            return $"{this.Value} - {this.Description}";
        }

    }
}