using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.BDD.Factories
{
    using Framework;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.IE;
    using OpenQA.Selenium.PhantomJS;

    public class DriverFactory
    {
        public static IWebDriver Create(Driver driverType, ChromeOptions chromeOptions = null)
        {
            switch (driverType)
            {
                case Driver.Chrome:
                    var driver = chromeOptions == null ? new ChromeDriver() : new ChromeDriver(chromeOptions);
                    return driver;
                case Driver.InternetExplorer:
                    return new InternetExplorerDriver();
                case Driver.PhantomJs:
                    return new PhantomJSDriver();
                default:
                    throw new ArgumentException("Unknown driver type");
            }
        }
    }
}
