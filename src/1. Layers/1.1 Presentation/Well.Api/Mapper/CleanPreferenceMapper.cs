namespace PH.Well.Api.Mapper
{
    using PH.Well.Api.Mapper.Contracts;
    using PH.Well.Api.Models;
    using PH.Well.Domain;

    public class CleanPreferenceMapper : ICleanPreferenceMapper
    {
        public CleanPreferenceModel Map(CleanPreference cleanPreference)
        {
            var model = new CleanPreferenceModel
            {
                Id = cleanPreference.Id,
                Days = cleanPreference.Days
            };

            foreach (var branch in cleanPreference.Branches)
            {
                model.BranchName += branch.Name + ", ";
                model.Branches.Add(branch);
            }

            model.BranchName = model.BranchName.TrimEnd(',', ' ');

            return model;
        }
    }
}