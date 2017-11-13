namespace PH.Well.Api.Infrastructure
{
    using System;
    using System.Web;
    using Domain.Enums;

    public class BranchProvider : IBranchProvider
    {
        public int GetBranchId()
        {
            try
            {
                var parts = HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.Split('/');
                var branchPart = int.Parse(parts[1]);

                if (!Enum.IsDefined(typeof(Branch), branchPart))
                {
                    throw new Exception($"{branchPart} is not a valid branch id");
                }

                return branchPart;

            }
            catch (Exception e)
            {
                throw new Exception("The correct branch id has not been passed in the URL", e);
            }


        }
    }
}