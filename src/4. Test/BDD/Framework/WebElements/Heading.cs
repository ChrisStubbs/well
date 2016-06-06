namespace PH.Well.BDD.Framework.WebElements
{
    public class Heading : WebElement
    {
        public string Content {
            get
            {
                return this.GetElement().Text;
            }
        }
    }
}
