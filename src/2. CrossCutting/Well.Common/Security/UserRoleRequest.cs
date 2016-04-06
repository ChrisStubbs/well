namespace PH.Well.Common.Security
{
    using System;
    using System.Collections.Generic;

    public class UserRoleRequest
    {
        public UserRoleRequest()
        {
            Roles = new List<string>();
        }

        public string UserIdentifier { get; set; }
        public Guid ApplicationId { get; set; }
        public List<string> Roles { get; set; }
    }
}
