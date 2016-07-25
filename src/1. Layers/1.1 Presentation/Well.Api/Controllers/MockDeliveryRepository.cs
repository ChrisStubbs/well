using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PH.Well.Api.Controllers
{
    using Common.Extensions;
    using Domain.ValueObjects;
    using Repositories.Contracts;
    public class MockDeliveryRepository : IDeliveryReadRepository
    {
        public IEnumerable<Delivery> GetCleanDeliveries(string userName)
        {
            return GetMockDeliveries();
        }

        public IEnumerable<Delivery> GetResolvedDeliveries(string userName)
        {
            return GetMockDeliveries();
        }

        public IEnumerable<Delivery> GetExceptionDeliveries(string userName)
        {
            return GetMockDeliveries();
        }


        private List<Delivery> GetMockDeliveries()
        {
            var list = new List<Delivery>();
            var model1 = new Delivery
            {
                Id = 1,
                RouteNumber = "001",
                DropId = "01",
                InvoiceNumber = "1363291",
                AccountCode = "4895.002",
                AccountName = "Fags bags and Mags",
                JobStatus = "Incomplete",
                Reason = "ByPassed",
                Assigned = "FLP",
                DateTime = DateTime.Now.ToDashboardDateFormat()
            };

            var model2 = new Delivery
            {
                Id = 2,
                RouteNumber = "001",
                DropId = "01",
                InvoiceNumber = "1363292",
                AccountCode = "4895.002",
                AccountName = "Fags bags and Mags",
                JobStatus = "Pending",
                Reason = "Short",
                Assigned = "FLP",
                DateTime = DateTime.Now.ToDashboardDateFormat()
            };

            var model3 = new Delivery
            {
                Id = 3,
                RouteNumber = "001",
                DropId = "01",
                InvoiceNumber = "2263287",
                AccountCode = "4895.002",
                AccountName = "Fags bags and Mags",
                JobStatus = "Incomplete",
                Reason = "Short",
                Assigned = "N/A",
                DateTime = DateTime.Now.ToDashboardDateFormat()
            };


            for (int i = 0; i < 67; i++)
            {
                list.Add(model1);
                list.Add(model2);
                list.Add(model3);
            }

            return list;
        }
    }
}