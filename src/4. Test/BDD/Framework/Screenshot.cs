namespace PH.Well.BDD.Framework
{
    using System;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Text;
    using Common.Contracts;
    using OpenQA.Selenium;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Tracing;

    public class Screenshot
    {
        public static void TakeScreenshot(IWebDriver driver, ILogger logger)
        {
            try
            {
                var fileName = ScenarioContext.Current.ScenarioInfo.Title.ToIdentifier() + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");

                var pageSource = driver.PageSource;

                var sourceFilePath = Path.Combine(Configuration.PathToScreenshots, fileName + ".html");

                File.WriteAllText(sourceFilePath, pageSource, Encoding.UTF8);
                
                var takesScreenshot = driver as ITakesScreenshot;

                if (takesScreenshot != null)
                {
                    var screenshot = takesScreenshot.GetScreenshot();

                    var screenshotFilePath = Path.Combine(Configuration.PathToScreenshots, fileName + ".png");

                    screenshot.SaveAsFile(screenshotFilePath, ImageFormat.Png);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Problem taking screenshot!", ex);
            }
        }
    }
}