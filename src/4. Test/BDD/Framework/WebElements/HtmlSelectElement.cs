namespace PH.Well.BDD.Framework.WebElements
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using Well.BDD.Framework;
    using Well.BDD.Framework.Extensions;

    public class HtmlSelectElement : WebElement
    {
        public void Select(string itemToSelect)
        {
            var element = this.GetElement();
            this.Driver.WaitForJavascript();
            var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(Configuration.DriverTimeoutSeconds));
            var option = wait.Until(d => element.FindElements(By.TagName("option")).First(x => x.Text == itemToSelect));
            option.Click();
        }

        public void SelectAll()
        {
            var element = this.GetElement();
            var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(Configuration.DriverTimeoutSeconds));
            this.Driver.WaitForJavascript();
            var options = wait.Until(d => element.FindElements(By.TagName("option")));
            foreach (var option in options)
            {
                option.Click();  
            }
        }

        public List<string> GetOptions()
        {
            IEnumerable<string> options = new List<string>();
            var element = this.GetElement();
            this.Driver.WaitForJavascript();
            var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(Configuration.DriverTimeoutSeconds));
            wait.Until(d => options = element.FindElements(By.TagName("option")).Select(x => x.Text));
            return options.ToList();
        }
    }
}
 