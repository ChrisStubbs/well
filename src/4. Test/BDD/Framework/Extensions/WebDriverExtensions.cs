namespace PH.Well.BDD.Framework.Extensions
{
    using System;
    using System.Linq;
    using System.Diagnostics;
    using System.Threading;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    public static class WebDriverExtensions
    {
        public static void NavigateToUrl(this IWebDriver driver, string url)
        {
            driver.Navigate().GoToUrl(url);

            while (driver.PageSource.Contains("This is the initial start page for the"))
            {
                Thread.Sleep(100);
            }
        }

        public static bool PageContainsText(this IWebDriver driver, string text)
        {
            return driver.PageSource.Contains(text);
        }

        public static void SwitchToDefaultContent(this IWebDriver driver)
        {
            var windows = driver.WindowHandles;

            driver.SwitchTo().Window(windows.Last());
        }

        public static void WaitForAjax(this IWebDriver driver)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Configuration.DriverTimeoutSeconds));

            var jsExecutor = driver as IJavaScriptExecutor;

            wait.Until(d => (bool)jsExecutor.ExecuteScript("return (jQuery.active == 0)" + ""));
        }

        public static string ReturnAnyJScriptErrors(this IWebDriver driver)
        {
            var js = driver as IJavaScriptExecutor;

            return (string)js.ExecuteScript("return window.jsErrors");
        }

        public static void ExecuteJavaScript(this IWebDriver driver, string jscript)
        {
            var js = driver as IJavaScriptExecutor;

            Debug.Assert(js != null, "js != null");

            js.ExecuteScript(jscript);
        }

        public static T QueryJScriptReturn<T>(this IWebDriver driver, string jscript)
        {
            var js = driver as IJavaScriptExecutor;

            Debug.Assert(js != null, "js != null");

            return (T)js.ExecuteScript(jscript);
        }

        public static bool IsSelected(this IWebDriver driver, IWebElement element, string selectedLocator)
        {
            var jsExecutor = driver as IJavaScriptExecutor;

            return ((string)jsExecutor.ExecuteScript("return arguments[0].className;", element)).Contains(selectedLocator);
        }
    }
}
