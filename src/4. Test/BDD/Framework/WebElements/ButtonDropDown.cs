namespace PH.Well.BDD.Framework.WebElements
{
    using System;
    using Extensions;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    public class ButtonDropDown : WebElement
    {
        public void Select(string itemToSelect)
        {
            this.Driver.WaitForAjax();
            this.GetElement();
            this.Element.FindElement(By.ClassName("caret")).Click();

            var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(Configuration.DriverTimeoutSeconds));

            var dropItem = wait.Until((d) => d.FindElement(By.LinkText(itemToSelect)));
            dropItem.Click();
        }
    }
}