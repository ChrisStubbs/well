namespace PH.Well.BDD.Steps.Page
{
    using System.Linq;

    using NUnit.Framework;

    using PH.Well.BDD.Pages;

    using TechTalk.SpecFlow;

    [Binding]
    public class DeliveryDetailSteps
    {
        private DeliveryDetailsPage page => new DeliveryDetailsPage();

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
    }
}