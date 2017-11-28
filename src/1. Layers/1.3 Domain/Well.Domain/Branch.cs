namespace PH.Well.Domain
{
    using System;
    using System.Collections.Generic;
    using ValueObjects;

    public class Branch : Entity<int>
    {
        public string Name { get; set; }

        public int PreferenceId { get; set; }

        public string BranchName => GetBranchName(Id, Name);

        public static string GetBranchName(int id, string name)
        {
            return $"{name} ({id})";
        }

        public static explicit operator Branch(int v)
        {
            throw new NotImplementedException();
        }
    }
}