namespace PH.Well.Domain.ValueObjects
{
    using System;
    using System.Globalization;
    using System.Linq;

    public class DirectoryListing
    {
        public DirectoryListing(string directoryListing)
        {
            var parts = directoryListing.Split(' ');

            var nonEmptyParts = parts.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

            this.Date = nonEmptyParts[0];
            this.Time = nonEmptyParts[1];
            this.Size = nonEmptyParts[2];
            this.Filename = nonEmptyParts[3];
        }

        public string Date { get; set; }

        public string Time { get; set; }

        public string Size { get; set; }

        public string Filename { get; set; }

        public DateTime Datetime
            =>
            DateTime.ParseExact(
                string.Join(" ", this.Date, this.Time),
                "MM-dd-yy h:mmtt",
                CultureInfo.InvariantCulture);
    }
}