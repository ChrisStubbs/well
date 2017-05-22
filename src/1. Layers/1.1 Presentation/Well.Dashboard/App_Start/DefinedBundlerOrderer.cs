namespace PH.Well.Dashboard
{
    using System.Collections.Generic;
    using System.Web.Optimization;

    public class DefinedBundlerOrderer : IBundleOrderer
    {
        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }
}