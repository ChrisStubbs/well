namespace PH.Well.BDD.Framework.Context
{
    using OpenQA.Selenium;

    public class DriverContext
    {
        public static IWebDriver CurrentDriver
        {
            get
            {
                return FeatureContextWrapper.GetContextObject<IWebDriver>(Configuration.Driver.ToString());
            }
            set
            {
                FeatureContextWrapper.SetContextObject(Configuration.Driver.ToString(), value);
            }
        }
    }
}