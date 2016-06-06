namespace PH.Well.BDD.Framework.WebElements
{
    public class Button : WebElement
    {
        public void Click()
        {
            this.GetElement().Click();
        }
    }
}