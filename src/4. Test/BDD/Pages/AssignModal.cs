using System;
using System.Linq;

namespace PH.Well.BDD.Pages
{
    using Framework;
    using Framework.Extensions;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    public class AssignModal
    {
        private readonly IWebDriver driver;

        public AssignModal(IWebDriver driver)
        {
            this.driver = driver;
        }

        public IWebElement GetUserFromModal(string username)
        {
            driver.WaitForJavascript();

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Configuration.DriverTimeoutSeconds));

            var elements = wait.Until(d => d.FindElements(By.ClassName("assign-user")));

            return elements.SingleOrDefault(e => string.Equals(e.Text, username, StringComparison.InvariantCultureIgnoreCase));
        }

    }
}
