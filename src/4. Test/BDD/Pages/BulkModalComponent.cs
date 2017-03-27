namespace PH.Well.BDD.Pages
{
    using Framework.WebElements;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    public class BulkModalComponent
    {
        public BulkModalComponent()
        {
            ModalTitle = new Heading() { Locator = By.ClassName("modal-title") };
            ModalBody = new Div() { Locator = By.ClassName("modal-body") };
            ConfirmButton = new Button() { Locator = By.Id("confirm-modal-button") };
            CancelButton = new Button() { Locator = By.Id("cancel-modal-button") };
        }

        public Heading ModalTitle { get; set; }
        public Button ConfirmButton { get; set; }
        public Button CancelButton { get; set; }
        public Div ModalBody { get; set; }

        public SpanElement Source(int index)
        {
            return new SpanElement() { Locator = By.Id($"source{index}") };
        }

        public SpanElement Reason(int index)
        {
            return new SpanElement() { Locator = By.Id($"reason{index}") };
        }
        
    }
}
