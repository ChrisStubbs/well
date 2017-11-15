namespace PH.Well.Api.Infrastructure
{
    using System;
    using System.Web;
    using System.Web.Hosting;
    using Domain.Enums;

    public class BranchProvider : IBranchProvider
    {

        public int? GetBranchId()
        {
            if (HostingEnvironment.IsHosted)
            {
                try
                {
                    var parts = HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.Split('/');
                    int branchId = 0;

                    if (int.TryParse(parts[1], out branchId))
                    {
                        if (!Enum.IsDefined(typeof(Branch), branchId))
                        {
                            throw new Exception($"{branchId} is not a valid branch id");
                        }
                        return branchId;
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception("The correct branch id has not been passed in the URL", ex);
                }
            }
            return null;
        }
    }
}