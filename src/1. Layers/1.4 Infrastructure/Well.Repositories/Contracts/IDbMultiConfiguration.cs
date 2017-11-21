namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;

    public interface IDbMultiConfiguration
    {
        IList<string> ConnectionStrings { get; }
    }
}