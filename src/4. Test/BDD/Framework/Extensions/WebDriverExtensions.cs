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

        public static void WaitForJQuery(this IWebDriver driver)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Configuration.DriverTimeoutSeconds));
           
            var jsExecutor = driver as IJavaScriptExecutor;

            wait.Until(d => (bool) jsExecutor.ExecuteScript("return (jQuery.active == 0)" + ""));
        }

        public static void WaitForAngular2(this IWebDriver driver)
        {
            var jsExecutor = driver as IJavaScriptExecutor;
            int count = 10;
            Exception exception = null;
            while (count > 0)
            {
                exception = TryWaitForAngular2(jsExecutor);
                if (exception == null)
                {
                    return;
                }
                Thread.Sleep(1000);
                count--;
            }
            throw new WebDriverTimeoutException("WebDriver timed out while waiting for Angular2", exception);
        }

        public static Exception TryWaitForAngular2(IJavaScriptExecutor jsExecutor)
        {
            string WaitForAllAngular2 = 
                @"  var callback = arguments[0];
                    var testabilities = window.getAllAngularTestabilities();
                    var count = testabilities.length;
                    var decrement = function() {
                        count--;
                        if (count === 0) {
                            callback();
                        }
                    };
                    testabilities.forEach(function(testability) {
                        testability.whenStable(decrement);
                    });";
            try
            {
                jsExecutor.ExecuteAsyncScript(WaitForAllAngular2);
                return null;
            }
            catch(Exception ex)
            {
                return ex;
            }
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
