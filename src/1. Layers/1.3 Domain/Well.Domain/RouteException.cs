using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.Domain
{
    public class RouteException : Entity<int>
    {
        public int NoOfExceptions { get; set; }
        public int Assigned { get; set; }
        public int Outstanding { get; set; }
        public int OnHold{ get; set; }
        public int Notifications { get; set; }
    }
}
