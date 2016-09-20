using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Assert.AreEqual(table.Rows[0]["Street"], modal.Street.Content);
            Assert.AreEqual(table.Rows[0]["Town"], modal.Town.Content);
            Assert.AreEqual(table.Rows[0]["Postcode"], modal.Postcode.Content);

            Assert.AreEqual(table.Rows[0]["Contact name"], modal.ContactName.Content);
            Assert.AreEqual(table.Rows[0]["Phone"], modal.Phone.Content);
            Assert.AreEqual(table.Rows[0]["Alt Phone"], modal.AltPhone.Content);
            Assert.AreEqual(table.Rows[0]["Email"], modal.Email.Content);
        }

    }
}
