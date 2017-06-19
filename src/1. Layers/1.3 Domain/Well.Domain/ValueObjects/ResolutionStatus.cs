namespace PH.Well.Domain.ValueObjects
{
    public sealed class ResolutionStatus
    {
        private readonly int value;
        private readonly string description;

        public ResolutionStatus(int value, string description)
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
                return new ResolutionStatus(1, "Imported");
            }
        }
    }

}
