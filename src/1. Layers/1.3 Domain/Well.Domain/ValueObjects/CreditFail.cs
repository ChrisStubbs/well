namespace PH.Well.Domain.ValueObjects
{
    public class CreditFail
    {

        public int Id { get; set; }

        public int JobId { get; set; }

        public int TransactionType { get; set; }

        public string Operator { get; set; }

        public string ErrorMessage { get; set; }
    }
}
