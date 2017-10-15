namespace PH.Well.Domain.ValueObjects
{
    public abstract class AppSearchItem : IAppSearchItem
    {
        public AppSearchItemType ItemType { get; }

        protected AppSearchItem(AppSearchItemType appSearchItem)
        {
            ItemType = appSearchItem;
        }
    }
}
