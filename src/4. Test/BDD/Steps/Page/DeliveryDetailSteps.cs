namespace PH.Well.BDD.Steps.Page
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Framework.WebElements;
    using NUnit.Framework;

    using PH.Well.BDD.Pages;

    using TechTalk.SpecFlow;

    [Binding]
    public class DeliveryDetailSteps
    {
        private DeliveryDetailsPage page => new DeliveryDetailsPage();
    
        [Given(@"I open the clean delivery '(.*)'")]
        [When(@"I open the clean delivery '(.*)'")]
        public void WhenIOpenTheCleanDelivery(int deliveryId)
        {
            string routing = "/" + deliveryId;
            page.Open(routing);

            page.ClickCleanTab();
        }

        [Given(@"I open the exception delivery '(.*)'")]
        [When(@"I open the exception delivery '(.*)'")]
        public void WhenIOpenTheExceptionDelivery(int deliveryId)
        {
            string routing = "/" + deliveryId;
            page.Open(routing);

            page.ClickExceptionsTab();
        }

        [Given(@"I click on the first delivery line")]
        [When(@"I click on the first delivery line")]
        public void ClickExceptionDetail()
        {
            IEnumerable<GridRow<DeliveryDetailsGrid>> rows = this.page.Grid.ReturnAllRows();
            rows.First().Click();
        }

        public void ClickSubmitActions()
        {
            page.SubmitActionButton.Click();
            page.ConfirmModalButton.Click();
        }

        [Then(@"I can not submit the delivery")]
        public void ThenICanNotSubmitTheDelivery()
        {
            Assert.AreEqual("true", page.SubmitActionButton.GetElement().GetAttribute("disabled"));
        }

        [Then(@"I am shown the high value check")]
        public void ShownHighValue()
        {   
            // we should have 5 elements that are checked
            Assert.IsTrue(this.page.HasThisNumberOfHighvalueItems(5));
        }

        [Then(@"I am shown the exception detail")]
        public void ShownExceptionDetail(Table table)
        {
            var pageRows = this.page.Grid.ReturnAllRows().ToList();

            Assert.That(pageRows.Count, Is.EqualTo(table.RowCount));

            for (int i = 0; i < table.RowCount; i++)
            {
                Assert.That(pageRows[i].GetColumnValueByIndex((int)DeliveryDetailsGrid.LineNo), Is.EqualTo(table.Rows[i]["LineNo"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)DeliveryDetailsGrid.Product), Is.EqualTo(table.Rows[i]["Product"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)DeliveryDetailsGrid.Description), Is.EqualTo(table.Rows[i]["Description"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)DeliveryDetailsGrid.Value), Is.EqualTo(table.Rows[i]["Value"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)DeliveryDetailsGrid.InvoiceQuantity), Is.EqualTo(table.Rows[i]["InvoiceQuantity"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)DeliveryDetailsGrid.DeliveryQuantity), Is.EqualTo(table.Rows[i]["DeliveryQuantity"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)DeliveryDetailsGrid.DamagedQuantity), Is.EqualTo(table.Rows[i]["DamagedQuantity"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)DeliveryDetailsGrid.ShortQuantity), Is.EqualTo(table.Rows[i]["ShortQuantity"]));
            }
        }

        [Given(@"I open the clean tab")]
        [When(@"I open the clean tab")]
        public void OpenCleanTab()
        {
            Thread.Sleep(1000);
            page.ClickCleanTab();
        }

        [Then(@"the line '(.*)' Short Qty is '(.*)' and Damaged Qty is '(.*)' Del Qty is '(.*)'")]
        public void ThenTheLineShortQtyIsAndDamagedQtyIsDelQtyIs(int line, string shortQty, string damagedQty, string deliveredQty)
        {
            var pageRows = this.page.Grid.ReturnAllRows().ToList();

            var row = line - 1;
            Assert.AreEqual(shortQty, pageRows[row].GetColumnValueByIndex((int) DeliveryDetailsGrid.ShortQuantity));
            Assert.AreEqual(damagedQty, pageRows[row].GetColumnValueByIndex((int) DeliveryDetailsGrid.DamagedQuantity));
            Assert.AreEqual(deliveredQty,
                pageRows[row].GetColumnValueByIndex((int) DeliveryDetailsGrid.DeliveryQuantity));

        }

        [Then(@"the delivery status is '(.*)'")]
        public void ThenTheDeliveryStatusIs(string status)
        {
            Assert.AreEqual(status, page.JobStatusSpan.Text);
        }
    }
}