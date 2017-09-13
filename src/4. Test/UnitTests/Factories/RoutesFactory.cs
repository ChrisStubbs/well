namespace PH.Well.UnitTests.Factories
{
    using System;
    using System.Collections.ObjectModel;
    using Well.Domain;

    public class RoutesFactory : EntityFactory<RoutesFactory, Routes>
    {
        public RoutesFactory()
        {
            this.Entity.Id = 1;
            this.Entity.FileName = "routefile.xml";
            this.Entity.ImportDate = DateTime.Now;
            this.Entity.RouteHeaders = new Collection<RouteHeader>();
        }
    }
}
