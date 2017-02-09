namespace PH.Well.UnitTests.Factories
{
    using Well.Domain;
    public class NotificationFactory : EntityFactory<NotificationFactory, Notification>
    {
        public NotificationFactory()
        {
            this.Entity.Id = 1;
            this.Entity.JobId = 1;
            this.Entity.Type = 1;
            this.Entity.ErrorMessage = "This is what the error is";
            this.Entity.Branch = "55";
            this.Entity.Account = "12345.001";
            this.Entity.InvoiceNumber = "555";
            this.Entity.LineNumber = "1";
            this.Entity.AdamErrorNumber = "99";
            this.Entity.AdamCrossReference = "123";
            this.Entity.UserName = "FP";
        }
    }
}
