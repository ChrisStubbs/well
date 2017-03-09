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
                Password = AdamConfiguration.AdamPassword
            };

            switch (branch)
            {
                case Branch.Medway:
                    settings.Rfs = AdamConfiguration.AdamRfsMedway;
                    settings.Port = AdamConfiguration.AdamPortMedway;
                    settings.Server = AdamConfiguration.AdamServerMedway;
                    break;
                case Branch.Coventry:
                    settings.Rfs = AdamConfiguration.AdamRfsCoventry;
                    settings.Port = AdamConfiguration.AdamPortCoventry;
                    settings.Server = AdamConfiguration.AdamServerCoventry;
                    break;
                case Branch.Fareham:
                    settings.Rfs = AdamConfiguration.AdamRfsFareham;
                    settings.Port = AdamConfiguration.AdamPortFareham;
                    settings.Server = AdamConfiguration.AdamServerFareham;
                    break;
                case Branch.Dunfermline:
                    settings.Rfs = AdamConfiguration.AdamRfsDunfermline;
                    settings.Port = AdamConfiguration.AdamPortDunfermline;
                    settings.Server = AdamConfiguration.AdamServerDunfermline;
                    break;
                case Branch.Leeds:
                    settings.Rfs = AdamConfiguration.AdamRfsLeeds;
                    settings.Port = AdamConfiguration.AdamPortLeeds;
                    settings.Server = AdamConfiguration.AdamServerLeeds;
                    break;
                case Branch.Hemel:
                    settings.Rfs = AdamConfiguration.AdamRfsHemel;
                    settings.Port = AdamConfiguration.AdamPortHemel;
                    settings.Server = AdamConfiguration.AdamServerHemel;
                    break;
                case Branch.Birtley:
                    settings.Rfs = AdamConfiguration.AdamRfsBirtley;
                    settings.Port = AdamConfiguration.AdamPortBirtley;
                    settings.Server = AdamConfiguration.AdamServerBirtley;
                    break;
                case Branch.Belfast:
                    settings.Rfs = AdamConfiguration.AdamRfsBelfast;
                    settings.Port = AdamConfiguration.AdamPortBelfast;
                    settings.Server = AdamConfiguration.AdamServerBelfast;
                    break;
                case Branch.Brandon:
                    settings.Rfs = AdamConfiguration.AdamRfsBrandon;
                    settings.Port = AdamConfiguration.AdamPortBrandon;
                    settings.Server = AdamConfiguration.AdamServerBrandon;
                    break;
                case Branch.Plymouth:
                    settings.Rfs = AdamConfiguration.AdamRfsPlymouth;
                    settings.Port = AdamConfiguration.AdamPortPlymouth;
                    settings.Server = AdamConfiguration.AdamServerPlymouth;
                    break;
                case Branch.Bristol:
                    settings.Rfs = AdamConfiguration.AdamRfsBristol;
                    settings.Port = AdamConfiguration.AdamPortBristol;
                    settings.Server = AdamConfiguration.AdamServerBristol;
                    break;
                case Branch.Haydock:
                    settings.Rfs = AdamConfiguration.AdamRfsHaydock;
                    settings.Port = AdamConfiguration.AdamPortHaydock;
                    settings.Server = AdamConfiguration.AdamServerHaydock;
                    break;
                default:
                    throw new ArgumentException("Branch not recognised");
            }

            return settings;
        }
    }
}