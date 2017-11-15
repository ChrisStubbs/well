using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.Task.GlobalUplifts.Data
{
    /// <summary>
    /// This class defines collection of Uplift data providers
    /// </summary>
    public class UpliftDataProvidersCollection : IUpliftDataProvider
    {
        #region Private fields
        private readonly List<IUpliftDataProvider> _providers;
        #endregion Private fields

        #region Constructors
        public UpliftDataProvidersCollection()
        {
            _providers = new List<IUpliftDataProvider>();
        }
        #endregion Constructors

        #region Public methods
        public void Add(IUpliftDataProvider provider)
        {
            _providers.Add(provider);
        }

        public virtual IEnumerable<UpliftDataSet> GetUpliftData()
        {
            foreach (var upliftDataProvider in _providers)
            {
                foreach (var dataSet in upliftDataProvider.GetUpliftData())
                {
                    yield return dataSet;
                }
            }
        }
        #endregion Public methods
    }
}
