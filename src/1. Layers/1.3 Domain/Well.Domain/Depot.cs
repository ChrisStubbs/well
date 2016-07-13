namespace PH.Well.Domain
{
    using System;

    [Serializable]
    public class Depot : Entity<int>
    {
        public string Code { get; set; }
    }
}
