namespace PH.Well.Domain.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AttributeBase: Entity<int>
    {
        public string Code { get; set; }
        public string Value { get; set; }
    }
}
