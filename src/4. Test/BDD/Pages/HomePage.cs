using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.BDD.Pages
{
    using Framework.WebElements;
    using OpenQA.Selenium;

    public class HomePage : Page
    {
        public HomePage()
        {
            this.H1Heading = new Heading {Locator = By.TagName("H1")};
        }

        protected override string UrlSuffix
            {
                get
                {
                    return string.Empty;
                }
            }

        public Heading H1Heading { get; set; }
    }
}
