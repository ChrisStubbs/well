using System;

namespace PH.Well.Domain.ValueObjects
{
    public class AppSearchInvoiceItem : AppSearchItem
    {
        /// <summary>
        /// Activity Id
        /// </summary>
        public int Id { get;}

        public string Type { get;}    

        public string DocumentNumber { get; }

        public string LocationName { get; }

        public DateTime Date { get; }

        public AppSearchInvoiceItem(int id, string documentNumber, string type, string locationName,
            DateTime date) : base(AppSearchItemType.Invoice)
        {
            Id = id;
            DocumentNumber = documentNumber;
            Type = type;
            LocationName = locationName;
            Date = date;
        }
    }
}