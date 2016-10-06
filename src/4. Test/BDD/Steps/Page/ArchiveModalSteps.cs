namespace PH.Well.BDD.Steps.Page
{
    using NUnit.Framework;
    using Pages;
    using TechTalk.SpecFlow;

    public static class ArchiveModalSteps
    {
        public static void CompareModal(Table table, ArchiveModalComponent modal)
        {
            Assert.AreEqual(table.Rows[0]["ModalTitle"], modal.ModalTitle.Content);
        }

        public static void ClickYes(ArchiveModalComponent modal)
        {
            modal.YesButton.Click();
        }


    }
}
