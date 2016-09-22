namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public enum UserRole
    {
        [Description("Branch Manager")]
        BranchManager = 1,
        [Description("Customer Service Manager")]
        CustomerServiceManager = 2,
        [Description("Customer Service User")]
        CustomerServiceUser = 3
    }
}