namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;
    using Domain;
    using Domain.ValueObjects;

    public interface ISubmitActionValidation
    {
        SubmitActionResult Validate(int[] jobsId, IEnumerable<Job> jobs);
    }
}