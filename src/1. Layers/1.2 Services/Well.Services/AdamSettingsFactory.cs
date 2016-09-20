namespace PH.Well.Services
{
    using System;

    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;

    public static class AdamSettingsFactory
    {
        public static AdamSettings GetAdamSettings(Branch branch)
        {
            var settings = new AdamSettings
            {
                Username = AdamConfiguration.AdamUsername,
                Password = AdamConfiguration.AdamPassword,
                Server = AdamConfiguration.AdamServer
            };

            switch (branch)
            {
                case Branch.Medway:
                    settings.Rfs = AdamConfiguration.AdamRfsMedway;
                    settings.Port = AdamConfiguration.AdamPortMedway;
                    break;
                case Branch.Coventry:
                    settings.Rfs = AdamConfiguration.AdamRfsCoventry;
                    settings.Port = AdamConfiguration.AdamPortCoventry;
                    break;
                case Branch.Fareham:
                    settings.Rfs = AdamConfiguration.AdamRfsFareham;
                    settings.Port = AdamConfiguration.AdamPortFareham;
                    break;
                case Branch.Dunfermline:
                    settings.Rfs = AdamConfiguration.AdamRfsDunfermline;
                    settings.Port = AdamConfiguration.AdamPortDunfermline;
                    break;
                case Branch.Leeds:
                    settings.Rfs = AdamConfiguration.AdamRfsLeeds;
                    settings.Port = AdamConfiguration.AdamPortLeeds;
                    break;
                case Branch.Hemel:
                    settings.Rfs = AdamConfiguration.AdamRfsHemel;
                    settings.Port = AdamConfiguration.AdamPortHemel;
                    break;
                case Branch.Birtley:
                    settings.Rfs = AdamConfiguration.AdamRfsBirtley;
                    settings.Port = AdamConfiguration.AdamPortBirtley;
                    break;
                case Branch.Belfast:
                    settings.Rfs = AdamConfiguration.AdamRfsBelfast;
                    settings.Port = AdamConfiguration.AdamPortBelfast;
                    break;
                case Branch.Brandon:
                    settings.Rfs = AdamConfiguration.AdamRfsBrandon;
                    settings.Port = AdamConfiguration.AdamPortBrandon;
                    break;
                case Branch.Plymouth:
                    settings.Rfs = AdamConfiguration.AdamRfsPlymouth;
                    settings.Port = AdamConfiguration.AdamPortPlymouth;
                    break;
                case Branch.Bristol:
                    settings.Rfs = AdamConfiguration.AdamRfsBristol;
                    settings.Port = AdamConfiguration.AdamPortBristol;
                    break;
                case Branch.Haydock:
                    settings.Rfs = AdamConfiguration.AdamRfsHaydock;
                    settings.Port = AdamConfiguration.AdamPortHaydock;
                    break;
                default:
                    throw new ArgumentException("Branch not recognised");
            }

            return settings;
        }
    }
}