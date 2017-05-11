namespace PH.Well.Domain.ValueObjects
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Extensions;

    public class Assignee
    {
        public int RouteId { get; set; }
        public int StopId { get; set; }
        public int JobId { get; set; }
        public string Name { get; set; }
        public string Initials => Name.GetInitials();

        public static string GetDisplayNames(IEnumerable<Assignee> assignees)
        {
            if (!assignees.Any()) return null;

            var names = assignees.Select(x => x.Name).Distinct().ToList();

            if (names.Count == 1)
            {
                return names[0];
            }
            var initials = names.Select(x => x.GetInitials());
            return string.Join(", ", initials);
        }
    }
}
