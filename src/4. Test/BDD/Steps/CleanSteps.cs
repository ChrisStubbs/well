namespace PH.Well.BDD.Steps
{
    using TechTalk.SpecFlow;

    [Binding]
    public class CleanSteps
    {
        [When(@"The clean task runs")]
        public void RunTheCleanTask()
        {
            PH.Well.Clean.Program.Main(null);
        }
    }
}