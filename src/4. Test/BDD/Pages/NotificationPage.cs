namespace PH.Well.BDD.Pages
{
    using System.Collections.Generic;
    using Framework.WebElements;
    using OpenQA.Selenium;

    public class NotificationPage : Page
    {
        public NotificationPage()
        {
            this.Pager = new PagerControl();
            this.NoNotificationsDiv = new Div { Locator = By.Id("no-notifications") };

            ArchiveModal = new ArchiveModalComponent();
        }
        
        protected override string UrlSuffix => "notifications";
        public PagerControl Pager { get; set; }

        public Div NoNotificationsDiv { get; set; }
        public ArchiveModalComponent ArchiveModal { get; set; }

        public List<NotificationInformation> GetNotifications(int rows, int pageNo)
        {
            var notifications = new List<NotificationInformation>();
            var addForNotificationId = 0;

            if (pageNo > 1)
            {
                switch (pageNo)
                {
                    case 2:
                        addForNotificationId = 3;
                        break;
                    case 3:
                        addForNotificationId = 6;
                        break;
                    case 4:
                        addForNotificationId = 9;
                        break;
                    default:
                        break;
                }
            }

            for (int i = 1; i <= rows; i++)
            {
                notifications.Add(new NotificationInformation(i + addForNotificationId));
            }

            return notifications;
        }

        public class NotificationInformation
        {
            public const string HeaderId = "notification-header-";
            public const string ArchiveId = "notification-archive-";
            public const string PickId = "notification-pick-";
            public const string InvoiceId = "notification-invoice-";
            public const string ContactId = "notification-contact-";
            public const string AccountId = "notification-account-";

            public NotificationInformation(int id)
            {
                this.Header = new NotificationElement { Locator = By.Id(HeaderId + id) };
                this.Account = new NotificationElement { Locator = By.Id(AccountId + id) };
                this.Pick = new NotificationElement { Locator = By.Id(PickId + id) };
                this.Invoice = new NotificationElement { Locator = By.Id(InvoiceId + id) };
                this.Contact = new NotificationElement { Locator = By.Id(ContactId + id) };
                this.Archive = new Button { Locator = By.Id(ArchiveId + id) };
            }

            public NotificationElement Header { get; set; }
            public NotificationElement Account { get; set; }
            public NotificationElement Pick { get; set; }
            public NotificationElement Invoice { get; set; }
            public NotificationElement Contact { get; set; }
            public Button Archive { get; set; }

            public class NotificationElement : WebElement
            {
                public string Text => this.GetElement().Text;
            }
        }
    }
}
