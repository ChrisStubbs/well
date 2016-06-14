namespace PH.Well.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Base;

    public class AccountAttribute :AttributeBase
    {
        public int AccountId { get; set; }
    }
}
