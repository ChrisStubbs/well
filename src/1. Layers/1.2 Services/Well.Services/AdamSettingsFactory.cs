namespace PH.Well.Services
{
    using System;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;

    public static class AdamSettingsFactory
    {
        /// <summary>
        /// Get the AdamSettings for a specified branch (or the adamDefault settings)
        /// </summary>
        /// <param name="branch">Branch enumeration</param>
        /// <returns>The AdamSettings for the specified branch, or the default branch settings, or an exception</returns>
        public static AdamSettings GetAdamSettings(Branch branch)
        {
            switch (branch)
            {
                case Branch.Medway:
                    return AdamConfiguration.AdamMedway;
                case Branch.Coventry:
                    return AdamConfiguration.AdamCoventry;
                case Branch.Fareham:
                    return AdamConfiguration.AdamFareham;
                case Branch.Dunfermline:
                    return AdamConfiguration.AdamDunfermline;
                case Branch.Leeds:
                    return AdamConfiguration.AdamLeeds;
                case Branch.Hemel:
                    return AdamConfiguration.AdamHemel;
                case Branch.Birtley:
                    return AdamConfiguration.AdamBirtley;
                case Branch.Belfast:
                    return AdamConfiguration.AdamBelfast;
                case Branch.Brandon:
                    return AdamConfiguration.AdamBrandon;
                case Branch.Plymouth:
                    return AdamConfiguration.AdamPlymouth;
                case Branch.Bristol:
                    return AdamConfiguration.AdamBristol;
                case Branch.Haydock:
                    return AdamConfiguration.AdamHaydock;
                case Branch.Default:
                    return AdamConfiguration.AdamDefault;
                default:
                    throw new ArgumentException("Branch not recognised");
            }
        }
    }
}
