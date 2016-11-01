namespace PH.Well.BDD.Steps.Page
{
    using NUnit.Framework;
    using Pages;
    using TechTalk.SpecFlow;

    public static class CreditModalSteps
    {

        public static void CompareModal(Table table, CreditModalComponent modal)
        {
            Assert.AreEqual(table.Rows[0]["ModalTitle"], modal.ModalTitle.Content);
            Assert.AreEqual(table.Rows[0]["ModalMessage"], modal.ModalBody.GetElement().Text);
        }
    }
}
