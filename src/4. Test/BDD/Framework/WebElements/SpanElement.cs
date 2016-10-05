namespace PH.Well.BDD.Framework.WebElements
{
    public class SpanElement : WebElement
    {
        public string Text => this.GetElement().Text;
    }
}
