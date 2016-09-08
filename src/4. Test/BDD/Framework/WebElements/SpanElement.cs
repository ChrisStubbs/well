namespace PH.Well.BDD.Framework.WebElements
{
    public class SpanElement : WebElement
    {
        public string Content => this.GetElement().Text;
    }
}
