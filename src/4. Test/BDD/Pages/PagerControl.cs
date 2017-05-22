using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.BDD.Pages
{
    using Framework.WebElements;
    using OpenQA.Selenium;

    public class PagerControl
    {
        private PagerContainer container;
        private IReadOnlyCollection<IWebElement> pageLinks;
        public PagerControl()
        {
            container = new PagerContainer { Locator = By.TagName("pagination-controls") };
        }

        public IReadOnlyCollection<IWebElement>  GetPageLinks()
        {
            return pageLinks ?? (pageLinks = container.GetElement().FindElements(By.TagName("li")));
        }

        public void Click(int pageNo)
        {
            // index 0 is the previous item
            var index = pageNo;
            GetPageLinks().ElementAt(index).Click();
        }

        public int NoOfPages()
        {
            //minus 2 discludes previous and next
            return GetPageLinks().Count - 2;
        }
    }
}
