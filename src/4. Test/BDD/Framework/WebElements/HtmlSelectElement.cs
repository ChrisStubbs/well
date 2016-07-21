namespace PH.Well.BDD.Framework.WebElements
{
    using System;
    using System.Linq;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using Well.BDD.Framework;
    using Well.BDD.Framework.Extensions;

    public class HtmlSelectElement : WebElement
    {
        public void Select(string itemToSelect)
        {
            this.GetElement();
            this.Driver.WaitForAjax();
            var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(Configuration.DriverTimeoutSeconds));
            var option = wait.Until(d => Element.FindElements(By.TagName("option")).First(x => x.Text == itemToSelect));
            option.Click();
        }

        public void SelectAll()
        {
            this.GetElement();
            var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(Configuration.DriverTimeoutSeconds));
            this.Driver.WaitForAjax();
            var options = wait.Until(d => Element.FindElements(By.TagName("option")));
            foreach (var option in options)
            {
                option.Click();  
            }
        }
    }
}
 