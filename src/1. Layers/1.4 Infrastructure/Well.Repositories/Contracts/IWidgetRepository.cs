namespace PH.Well.Repositories.Contracts
{
    using System.Collections;
    using System.Collections.Generic;
    using Domain;

    public interface IWidgetRepository : IRepository<WidgetWarning, int>
    {
        IEnumerable<WidgetWarning> GetAll();
        void Delete(int id);
    }
}
