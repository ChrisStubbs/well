﻿namespace PH.Well.BDD.Framework.WebElements
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using OpenQA.Selenium;
    using Well.BDD.Framework.Extensions;

    public class Grid<T> : WebElement
    {
        public By RowLocator { get; set; }

        public IList<IWebElement> Header => this.GetElement()
            .FindElements(By.TagName("th"))
            .Select(p => p)
            .ToList();

        private IEnumerable<IWebElement> rowElements;

        public IEnumerable<IWebElement> RowElements()
        {
            this.Driver.WaitForJavascript();

            this.rowElements = this.GetElement().FindElements(this.RowLocator);

            return this.rowElements;
        }

        public IEnumerable<GridRow<T>> ReturnAllRows()
        {
            this.RowElements();

            return this.rowElements.Select(row => new GridRow<T>(row)).ToList();
        }
    }

    public class GridRow<T> : WebElement
    {
        private readonly IWebElement row;

        private readonly string selectedLocator = "sorbet";

        private Dictionary<string, string> columnContents { get; set; }

        public Dictionary<string, string> ColumnContents
        {
            get
            {
                this.columnContents = this.columnContents ?? new Dictionary<string, string>();

                return this.columnContents;
            }
        }

        private const string LocatorTemplate = "[data-qa*='column{0}']";

        public GridRow(IWebElement row)
        {
            this.row = row;
        }

        public Dictionary<string, string> GetRowContents()
        {
            foreach (var columnName in (T[])Enum.GetValues(typeof(T)))
            {
                this.ColumnContents.Add(columnName.ToString(), this.ReturnColumnValue(columnName));
            }

            return this.ColumnContents;
        }

        public string ReturnColumnValue(T columnName)
        {
            if (columnName.ToString().EndsWith("Bool"))
            {
                string result;
                try
                {
                    this.row.FindElement(
                    By.CssSelector(string.Format(LocatorTemplate, columnName))).FindElement(By.CssSelector(".true"));
                    result = "true";
                }
                catch (Exception)
                {
                    this.row.FindElement(
                    By.CssSelector(string.Format(LocatorTemplate, columnName))).FindElement(By.CssSelector(".false"));
                    result = "false";
                }

                return result;
            }

            return this.row.FindElement(By.CssSelector(string.Format(LocatorTemplate, columnName))).Text;
        }

        public IWebElement GetItemInRowByClass(string className)
        {
            return this.row.FindElement(By.ClassName(className));
        }

        public IWebElement GetItemInRowById(string id)
        {
            var elements = row.FindElements(By.Id(id));
            return elements != null && elements.Count > 0 ? elements[0] : null;
        }

        public string GetColumnValueByIndex(int idx)
        {
            return this.row.FindElements(By.XPath(".//*"))[idx].Text;
        }

        public string GetTdValueByIndex(int index)
        {
            return this.row.FindElements(By.TagName("td"))[index].Text;
        }

        public override void Click()
        {
            this.Driver.ExecuteJavaScript(string.Format("window.scrollTo(0, {0});", this.row.Location.Y));
            this.row.Click();
        }

        public bool IsSelected()
        {
            return this.Driver.IsSelected(this.row, this.selectedLocator);
        }
    }
}