namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public enum GrnRefused
    {
        [Description("Incorrect")]
        Inc = 1,

        [Description("NotNeeded")]
        NotNd = 2,
    }
}
