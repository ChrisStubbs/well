namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public enum Branches
    {
        [Description("Medway")]
        Med = 2,

        [Description("Coventry")]
        Cov = 3,

        [Description("Fareham")]
        Far = 5,

        [Description("Dunfermline")]
        Dun = 9,

        [Description("Leeds")]
        Lee = 14,

        [Description("Hemel")]
        Hem = 20,

        [Description("Birtley")]
        Bir = 22,

        [Description("Belfast")]
        Bel = 33,

        [Description("Brandon")]
        Bra = 42,

        [Description("Plymouth")]
        Ply = 55,

        [Description("Bristol")]
        Bri = 59,

        [Description("Haydock")]
        Hay = 82,

        [Description("Not Defined")]
        NotDefined = 99
    }
}
