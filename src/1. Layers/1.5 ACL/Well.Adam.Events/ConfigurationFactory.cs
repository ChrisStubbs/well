namespace PH.Well.Adam.Events
{
    using System;

    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;

    public static class ConfigurationFactory
    {
        public static AdamConfiguration GetAdamConfiguration(Branch branch)
        {
            var adamConfiguration = new AdamConfiguration
            {
                Username = Configuration.AdamUsername,
                Password = Configuration.AdamPassword,
                Server = Configuration.AdamServer
            };

            switch (branch)
            {
                case Branch.MedwayIsDown:
                    adamConfiguration.Rfs = Configuration.AdamRfsMedway;
                    adamConfiguration.Port = Configuration.AdamPortMedway;
                    break;
                case Branch.Coventry:
                    adamConfiguration.Rfs = Configuration.AdamRfsCoventry;
                    adamConfiguration.Port = Configuration.AdamPortCoventry;
                    break;
                case Branch.Fareham:
                    adamConfiguration.Rfs = Configuration.AdamRfsFareham;
                    adamConfiguration.Port = Configuration.AdamPortFareham;
                    break;
                case Branch.Dunfermline:
                    adamConfiguration.Rfs = Configuration.AdamRfsDunfermline;
                    adamConfiguration.Port = Configuration.AdamPortDunfermline;
                    break;
                case Branch.Leeds:
                    adamConfiguration.Rfs = Configuration.AdamRfsLeeds;
                    adamConfiguration.Port = Configuration.AdamPortLeeds;
                    break;
                case Branch.Hemel:
                    adamConfiguration.Rfs = Configuration.AdamRfsHemel;
                    adamConfiguration.Port = Configuration.AdamPortHemel;
                    break;
                case Branch.Birtley:
                    adamConfiguration.Rfs = Configuration.AdamRfsBirtley;
                    adamConfiguration.Port = Configuration.AdamPortBirtley;
                    break;
                case Branch.Belfast:
                    adamConfiguration.Rfs = Configuration.AdamRfsBelfast;
                    adamConfiguration.Port = Configuration.AdamPortBelfast;
                    break;
                case Branch.Brandon:
                    adamConfiguration.Rfs = Configuration.AdamRfsBrandon;
                    adamConfiguration.Port = Configuration.AdamPortBrandon;
                    break;
                case Branch.Plymouth:
                    adamConfiguration.Rfs = Configuration.AdamRfsPlymouth;
                    adamConfiguration.Port = Configuration.AdamPortPlymouth;
                    break;
                case Branch.Bristol:
                    adamConfiguration.Rfs = Configuration.AdamRfsBristol;
                    adamConfiguration.Port = Configuration.AdamPortBristol;
                    break;
                case Branch.Haydock:
                    adamConfiguration.Rfs = Configuration.AdamRfsHaydock;
                    adamConfiguration.Port = Configuration.AdamPortHaydock;
                    break;
                default: throw new ArgumentException("Branch not recognised");
            }

            return adamConfiguration;
        }
    }
}