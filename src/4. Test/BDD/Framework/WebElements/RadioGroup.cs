namespace PH.Well.BDD.Framework.WebElements
{
    using OpenQA.Selenium;

    public class RadioGroup : WebElementList
    {
        public RadioGroup(string name)
        {
            Locator = By.Name(name);
        }
    }
}