﻿namespace PH.Well.BDD.Steps.Page
{
    using System.Linq;
    using NUnit.Framework;
    using Pages;
    using TechTalk.SpecFlow;

    [Binding]
    public class ExceptionPageSteps
    {
        private ExceptionDeliveriesPage ExceptionDeliveriesPage => new ExceptionDeliveriesPage();

        [When(@"I open the exception deliveries")]
        public void WhenIOpenTheExceptionDeliveries()
        {
            ExceptionDeliveriesPage.Open();
        }

        [Then(@"the following exception deliveries will be displayed")]
        public void ThenTheFollowingExceptionDeliveriesWillBeDisplayed(Table table)
        {
            var pageRows = this.ExceptionDeliveriesPage.RoutesGrid.ReturnAllRows().ToList();
            Assert.That(pageRows.Count, Is.EqualTo(table.RowCount));
            for (int i = 0; i < table.RowCount; i++)
            {
                Assert.That(pageRows[i].GetColumnValueByIndex((int)ExceptionDeliveriesGrid.Route), Is.EqualTo(table.Rows[i]["Route"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)ExceptionDeliveriesGrid.Drop), Is.EqualTo(table.Rows[i]["Drop"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)ExceptionDeliveriesGrid.InvoiceNo), Is.EqualTo(table.Rows[i]["InvoiceNo"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)ExceptionDeliveriesGrid.Account), Is.EqualTo(table.Rows[i]["Account"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)ExceptionDeliveriesGrid.AccountName), Is.EqualTo(table.Rows[i]["AccountName"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)ExceptionDeliveriesGrid.Status), Is.EqualTo(table.Rows[i]["Status"]));
                //Assert.That(pageRows[i].GetColumnValueByIndex((int)ExceptionDeliveriesGrid.Reason), Is.EqualTo(table.Rows[i]["Reason"]));
                //Assert.That(pageRows[i].GetColumnValueByIndex((int)ExceptionDeliveriesGrid.Assigned), Is.EqualTo(table.Rows[i]["Assigned"]));
            }
        }


        [When(@"I filter the exception delivery grid with the option '(.*)' and value '(.*)'")]
        public void WhenIFilterTheExceptionDeliveryGridWithTheOptionAndValue(string option, string value)
        {
            this.ExceptionDeliveriesPage.Filter.Apply(option, value);
        }


        //[Then(@"the following exception deliveries will be displayed")]
        //public void ThenTheFollowingExceptionDeliveriesWillBeDisplayed(Table table)
        //{
        //    var pageRows = this.ExceptionDeliveriesPage.RoutesGrid.ReturnAllRows().ToList();
        //    Assert.That(pageRows.Count, Is.EqualTo(table.RowCount));
        //    for (int i = 0; i < table.RowCount; i++)
        //    {
        //        Assert.That(pageRows[i].GetColumnValueByIndex((int)ExceptionDeliveriesGrid.Route), Is.EqualTo(table.Rows[i]["Route"]));
        //        Assert.That(pageRows[i].GetColumnValueByIndex((int)ExceptionDeliveriesGrid.Drop), Is.EqualTo(table.Rows[i]["Drop"]));
        //        Assert.That(pageRows[i].GetColumnValueByIndex((int)ExceptionDeliveriesGrid.InvoiceNo), Is.EqualTo(table.Rows[i]["InvoiceNo"]));
        //        Assert.That(pageRows[i].GetColumnValueByIndex((int)ExceptionDeliveriesGrid.Account), Is.EqualTo(table.Rows[i]["Account"]));
        //        Assert.That(pageRows[i].GetColumnValueByIndex((int)ExceptionDeliveriesGrid.AccountName), Is.EqualTo(table.Rows[i]["AccountName"]));
        //        Assert.That(pageRows[i].GetColumnValueByIndex((int)ExceptionDeliveriesGrid.Status), Is.EqualTo(table.Rows[i]["Status"]));
        //    }
        //}

        [When(@"I click on exception delivery page (.*)")]
        public void WhenIClickOnExceptionDeliveryPage(int pageNo)
        {
            this.ExceptionDeliveriesPage.Pager.Click(pageNo);
        }


        [Then(@"'(.*)' rows of exception delivery data will be displayed")]
        public void ThenRowsOfExceptionDeliveryDataWillBeDisplayed(int noOfRowsExpected)
        {
            var pageRows = this.ExceptionDeliveriesPage.RoutesGrid.ReturnAllRows().ToList();
            Assert.That(pageRows.Count, Is.EqualTo(noOfRowsExpected));
        }

        [Then(@"I will have (.*) pages of exception delivery data")]
        public void CheckNoOfPages(int noOfPages)
        {
            Assert.That(ExceptionDeliveriesPage.Pager.NoOfPages(), Is.EqualTo(noOfPages));
        }

    }
}