namespace PH.Well.Domain
{
    public class RouteAttributeException :Entity<int>
    {
        public string ObjectType { get; set; }

        public string AttributeName { get; set; }

    }
}
