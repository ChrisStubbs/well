namespace PH.Well.BDD.Pages
{
    public class NotificationPage : Page
    {

        protected override string UrlSuffix => "notifications";
    }

    public enum NotificationGrid
    {
        Account = 0,
        PicklistReference = 1,
        InvoiceNumber = 2,
        Contact = 3
    }
}
