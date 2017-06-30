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
        public string IdentityName { get; set; }

        public static string GetDisplayNames(IEnumerable<Assignee> assignees)
        {
            return GetDisplayNames(assignees.Select(x => x.Name));
        }

        public static string GetDisplayNames(IEnumerable<string> assigneeNames)
        {
            assigneeNames = assigneeNames.ToArray();
            if (!assigneeNames.Any()) return null;

            var names = assigneeNames.Distinct().ToList();

            if (names.Count == 1)
            {
                return names[0];
            }
            var initials = names.Select(x => x.GetInitials());
            return string.Join(", ", initials);
        }
    }
}
