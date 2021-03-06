﻿namespace PH.Well.Domain.ValueObjects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Extensions;

    public class Assignee
    {
        private const string Unallocated = "Unallocated";

        public int RouteId { get; set; }

        public int StopId { get; set; }

        public int JobId { get; set; }

        private string name;
        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(name))
                {
                    name = Unallocated;
                }

                return name;
            }
            set
            {
                this.name = value;
            }
        }

        public string Initials => Name.GetInitials();

        public string IdentityName { get; set; }

        public static string GetDisplayNames(IEnumerable<Assignee> assignees)
        {
            return assignees == null ? null : GetDisplayNames(assignees.Select(x => x.Name));
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

            Func<string, bool> f = (name) => string.Equals(Unallocated, name, StringComparison.InvariantCultureIgnoreCase);

            var totalUnallocated = assigneeNames.Count(p => f(p));

            var initials = names
                .Select(x =>
                {
                    if (f(x))
                    {
                        return new { Unallocated = 1, Name = $"{x}({totalUnallocated})" };
                    }
                    return new { Unallocated = 0, Name = x.GetInitials() };
                })
                .OrderBy(p => p.Unallocated)
                .Select(p => p.Name);
            return string.Join(", ", initials);
        }
    }
}
