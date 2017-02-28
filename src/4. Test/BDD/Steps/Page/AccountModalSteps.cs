namespace PH.Well.BDD.Steps.Page
{
    using NUnit.Framework;
    using Pages;
    using TechTalk.SpecFlow;

    public static class AccountModalSteps
    {
        public static void CompareModal(Table table, AccountModalComponent modal)
        {
            Assert.AreEqual(table.Rows[0]["Account name"], modal.AccountName.Content);
            Assert.AreEqual(table.Rows[0]["Street"], modal.Street.Text);
            Assert.AreEqual(table.Rows[0]["Town"], modal.Town.Text);
            Assert.AreEqual(table.Rows[0]["Postcode"], modal.Postcode.Text);

            Assert.AreEqual(table.Rows[0]["Contact name"], modal.ContactName.Content);
            Assert.AreEqual(table.Rows[0]["Phone"], modal.Phone.Text);
            Assert.AreEqual(table.Rows[0]["Alt Phone"], modal.AltPhone.Text);
            Assert.AreEqual(table.Rows[0]["Email"], modal.Email.Text);
        }
    }
}
