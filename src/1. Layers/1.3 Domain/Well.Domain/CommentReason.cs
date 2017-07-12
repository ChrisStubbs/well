using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.Domain
{
    public class CommentReason : Entity<int>
    {
        public string Description { get; set; }
        public bool IsDefault { get; set; }
    }
}
