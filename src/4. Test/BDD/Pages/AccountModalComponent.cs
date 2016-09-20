namespace PH.Well.BDD.Pages
{
    using Framework.WebElements;
    using OpenQA.Selenium;

    public class AccountModalComponent
    {
        public AccountModalComponent()
        {
            AccountName = new Heading { Locator = By.ClassName("account-name") };
            Street = new SpanElement() { Locator = By.ClassName("street") };
            Town = new SpanElement() { Locator = By.ClassName("town") };
            Postcode = new SpanElement() { Locator = By.ClassName("postcode") };
            ContactName = new Heading() { Locator = By.ClassName("contact-name") };
            Phone = new SpanElement() { Locator = By.ClassName("phone") };
            AltPhone = new SpanElement() { Locator = By.ClassName("altphone") };
            Email = new SpanElement() { Locator = By.ClassName("email") };
        }

        public Heading AccountName { get; set; }
        public SpanElement Street { get; set; }
        public SpanElement Town { get; set; }
        public SpanElement Postcode { get; set; }
        public Heading ContactName { get; set; }
        public SpanElement Phone { get; set; }
        public SpanElement AltPhone { get; set; }
        public SpanElement Email { get; set; }
    }
}
