namespace PH.Well.Domain.ValueObjects
{
    public class AppSearchLocationItem : AppSearchItem
    {
        /// <summary>
        /// Location id
        /// </summary>
        public int Id { get;}

        public string Name { get; }

        public string AccountNumber { get; }

        public AppSearchLocationItem(int id,string name,string accountName):base(AppSearchItemType.Location)
        {
            Id = id;
            Name = name;
            AccountNumber = accountName;
        }
    }
}