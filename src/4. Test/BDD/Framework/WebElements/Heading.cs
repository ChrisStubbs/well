namespace PH.Well.BDD.Framework.WebElements
{
    public class Heading : WebElement
    {
        public string Content => this.GetElement().Text;
    }
}
