namespace PH.Well.BDD.Pages
{
    using Framework.WebElements;
    using OpenQA.Selenium;

    public class ArchiveModalComponent
    {
        public ArchiveModalComponent()
        {
            ModalTitle = new Heading() {Locator = By.ClassName("modal-title")};
            YesButton = new Button() {Locator = By.Id("modal-yes")};
            NoButton = new Button() { Locator = By.Id("modal-no") };
        }

        public Heading ModalTitle { get; set; }
        public Button YesButton { get; set; }
        public Button NoButton { get; set; }
    }
}
