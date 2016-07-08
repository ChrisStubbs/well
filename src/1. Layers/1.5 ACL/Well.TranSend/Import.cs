namespace PH.Well.TranSend
{
    using Contracts;
    using StructureMap;
    using global::PH.Well.Domain;

    public class Import
    {
        public void Process(IContainer container)
        {
            var ePodFtpProvider = container.GetInstance<IEpodFtpProvider>();
            ePodFtpProvider.ListFilesAndProcess();

        }
    }
}
