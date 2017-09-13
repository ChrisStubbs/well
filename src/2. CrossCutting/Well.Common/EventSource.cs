namespace PH.Well.Common
{
    public enum EventSource
    {
        WellApi = 1,

        WellAdamXmlImport = 2,

        WellTaskRunner = 3,

        /// <summary>
        /// Global uplift background task
        /// </summary>,
        WellGlobalUpliftTask = 4,

        WellEpodXmlImport = 5
    }
}
