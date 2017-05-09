using System.Collections.Generic;
using System.Web.Http;

namespace PH.Well.Api.Controllers
{
    public class StopsController : ApiController
    {
        public IList<Stop> Get(int id)
        {
            return new List<Stop>
            {
                new Stop { Id= 1, Job = "001", Invoice = "123456789", Account = "2345", Product = "Cakes",           Type = "DEL-TOB", Description = "Account Number 65651+621", Value = 50M,   Invoiced = 12,  Delivered = 6, Damages = 15, Shorts = 0,  Checked = true,  HeightValue = false, BarCode = null },
                new Stop { Id= 2, Job = "001", Invoice = "993456789", Account = "568",  Product = "More Cakes",      Type = "DEL-ALC", Description = "Account Number 69^9",      Value = 80M,   Invoiced = 22,  Delivered = 0, Damages = 0,  Shorts = 22, Checked = false, HeightValue = false, BarCode = "321651651652165165" },
                new Stop { Id= 3, Job = "002", Invoice = "65",        Account = "7890", Product = "Even More Cakes", Type = "UPL-SAN", Description = "Account Number +yyyyyy",   Value = 22.3M, Invoiced = 0,  Delivered = 1,  Damages = 0,  Shorts = 0,  Checked = false, HeightValue = true,  BarCode = "5489349034" }
            };
        }
    }

    public class Stop
    {
        public int Id { get; set; }
        public string Job { get; set; }
        public string Invoice { get; set; }
        public string Account { get; set; }
        public string Product { get; set; }
        public string @Type { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public int Invoiced { get; set; }
        public int Delivered { get; set; }
        public int Damages { get; set; }
        public int Shorts { get; set; }
        public bool Checked { get; set; }
        public bool HeightValue { get; set; }
        public string BarCode { get; set; }
    }
}
