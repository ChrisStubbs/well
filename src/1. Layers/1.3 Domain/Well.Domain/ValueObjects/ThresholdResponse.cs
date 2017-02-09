namespace PH.Well.Domain.ValueObjects
{
    public class ThresholdResponse
    {
        public bool CanUserCredit { get; set; }

        public bool IsInError { get; set; }

        public string ErrorMessage { get; set; }
    }
}
