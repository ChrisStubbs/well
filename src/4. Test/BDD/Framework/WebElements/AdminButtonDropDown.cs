﻿namespace PH.Well.BDD.Framework.WebElements
{
    using System;
    using System.Threading;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using Well.BDD.Framework;
    using Well.BDD.Framework.Extensions;

    public class AdminButtonDropDown : WebElement
    {
        public void SelectBranchSelection()
        {
            this.Driver.WaitForJavascript();
            this.GetElement().FindElement(By.Id("admin-dropdown-anchor")).Click();

            var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(Configuration.DriverTimeoutSeconds));
            
            var dropItem = wait.Until((d) => d.FindElement(By.Id("branch-selection")));

            dropItem.Click();
        }

        public void SelectBranchParameters()
        {
            this.Driver.WaitForJavascript();
            this.GetElement().FindElement(By.Id("admin-dropdown-anchor")).Click();

            var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(Configuration.DriverTimeoutSeconds));

            var dropItem = wait.Until((d) => d.FindElement(By.Id("branch-preference")));

            dropItem.Click();
        }

        public void SelectUserThreshold()
        {
            this.Driver.WaitForJavascript();
            
            this.GetElement().FindElement(By.Id("admin-dropdown-anchor")).Click();

            var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(Configuration.DriverTimeoutSeconds));

            var dropItem = wait.Until((d) => d.FindElement(By.Id("user-threshold")));

            dropItem.Click();
        }

    }
}