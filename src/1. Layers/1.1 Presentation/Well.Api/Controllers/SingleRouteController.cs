using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PH.Well.Api.Controllers
{
    public class SingleRouteController : ApiController
    {
        public List<SingleRoute> Get(int id)
        {
            return new List<SingleRoute>
            {
                new SingleRoute { Stop = "001", Invoice = "9875654321", JobType = "Frozen", Status = "Clean", Cod = "1", Pod = true, Exceptions = 0, Clean = 10,  Tba = 0, Assignee = "Henrry Pires"},
                new SingleRoute { Stop = "001", Invoice = "324", JobType = "Chiled", Status = "Exceptions", Cod = "1", Pod = true, Exceptions = 8, Clean = 10,  Tba = 0, Assignee = "Henrry Pires"},
                new SingleRoute { Stop = "002", Invoice = "56754", JobType = "Frozen", Status = "Clean", Cod = "1", Pod = true, Exceptions = 0, Clean = 10,  Tba = 0, Assignee = "Henrry Pires"},
                new SingleRoute { Stop = "003", Invoice = "56785", JobType = "Frozen", Status = "Exceptions", Cod = "1", Pod = true, Exceptions = 4, Clean = 2,  Tba = 0, Assignee = "Henrry Pires"},
                new SingleRoute { Stop = "003", Invoice = "561785", JobType = "Ambient", Status = "Exceptions", Cod = "1", Pod = true, Exceptions = 3, Clean = 10,  Tba = 1, Assignee = "Henrry Pires"},
            };
        }
    }

    public class SingleRoute
    {
        public string Stop { get; set; }
        public string Invoice { get; set; }
        public string JobType { get; set; }
        public string Status { get; set; }
        public string Cod { get; set; }
        public bool Pod { get; set; }
        public int Exceptions { get; set; }
        public int Clean { get; set; }
        public int Tba { get; set; }
        public int? Credit { get; set; }
        public string Assignee { get; set; }
    }
}
