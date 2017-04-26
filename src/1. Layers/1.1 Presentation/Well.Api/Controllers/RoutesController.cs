namespace PH.Well.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using Common.Contracts;
    using Models;
    using Repositories.Contracts;

    public class RoutesController : BaseApiController
    {
        private readonly IRouteHeaderRepository routeRepository;
        private readonly IServerErrorResponseHandler serverErrorResponseHandler;

        public RoutesController(IRouteHeaderRepository routeRepository,
                IServerErrorResponseHandler serverErrorResponseHandler,
                IUserNameProvider userNameProvider):
            base(userNameProvider)
        {
            this.routeRepository = routeRepository;
            this.serverErrorResponseHandler = serverErrorResponseHandler;
        }

        
        public HttpResponseMessage Get()
        {
            try
            {
                var routes = new List<RouteModel>
                {
                    new RouteModel  { Branch= "Bir (22)", Route= "001", RouteDate = new DateTime(2017,1,10), StopCount= 12, RouteStatus= "In Complete", ExceptionCount= 10, DriverName= "Chris", Assignee= "" },
                    new RouteModel  { Branch= "Bir (22)", Route= "002", RouteDate = new DateTime(2017,1,10), StopCount= 25, RouteStatus= "Complete", ExceptionCount= 11, DriverName= "Bob", Assignee= "" },
                    new RouteModel  { Branch= "Bir (22)", Route= "003", RouteDate = new DateTime(2017,1,10), StopCount= 26, RouteStatus= "Complete", ExceptionCount= 12, DriverName= "John", Assignee= "" },
                    new RouteModel  { Branch= "Bir (22)", Route= "004", RouteDate = new DateTime(2017,1,10), StopCount= 12, RouteStatus= "Complete", ExceptionCount= 5, DriverName= "George", Assignee= "" },
                    new RouteModel  { Branch= "Lee (14)", Route= "005", RouteDate = new DateTime(2017,1,10), StopCount= 32, RouteStatus= "Complete", ExceptionCount= 6, DriverName= "Fiona", Assignee= "" },
                    new RouteModel  { Branch= "Lee (14)", Route= "006", RouteDate = new DateTime(2017,1,10), StopCount= 8, RouteStatus= "In Complete", ExceptionCount= 10, DriverName= "Louise", Assignee= "" },
                    new RouteModel  { Branch= "Lee (14)", Route= "007", RouteDate = new DateTime(2017,1,10), StopCount= 59, RouteStatus= "Complete", ExceptionCount= 10, DriverName= "Henrry", Assignee= "" },
                    new RouteModel  { Branch= "Lee (14)", Route= "008", RouteDate = new DateTime(2017,1,10), StopCount= 12, RouteStatus= "Complete", ExceptionCount= 10, DriverName= "Daviid", Assignee= "" },
                    new RouteModel  { Branch= "Lee (14)", Route= "009", RouteDate = new DateTime(2017,1,10), StopCount= 12, RouteStatus= "Complete", ExceptionCount= 10, DriverName= "Chris", Assignee= "" },
                    new RouteModel  { Branch= "Lee (14)", Route= "010", RouteDate = new DateTime(2017,1,10), StopCount= 12, RouteStatus= "Complete", ExceptionCount= 10, DriverName= "Chris", Assignee= "" },
                    new RouteModel  { Branch= "Lee (14)", Route= "011", RouteDate = new DateTime(2017,1,10), StopCount= 12, RouteStatus= "Complete", ExceptionCount= 10, DriverName= "Chris", Assignee= "" },
                    new RouteModel  { Branch= "Lee (14)", Route= "012", RouteDate = new DateTime(2017,1,10), StopCount= 12, RouteStatus= "Complete", ExceptionCount= 10, DriverName= "Chris", Assignee= "" },
                    new RouteModel  { Branch= "Med (2)", Route= "013", RouteDate = new DateTime(2017,1,10), StopCount= 12, RouteStatus= "Incomplete", ExceptionCount= 10, DriverName= "Chris", Assignee= "" },
                    new RouteModel  { Branch= "Med (2)", Route= "014", RouteDate = new DateTime(2017,1,10), StopCount= 12, RouteStatus= "Complete", ExceptionCount= 10, DriverName= "Chris", Assignee= "" },
                    new RouteModel  { Branch= "Med (2)", Route= "015", RouteDate = new DateTime(2017,1,10), StopCount= 12, RouteStatus= "In Complete", ExceptionCount= 10, DriverName= "Chris", Assignee= "" },
                    new RouteModel  { Branch= "Med (2)", Route= "016", RouteDate = new DateTime(2017,1,10), StopCount= 12, RouteStatus= "Incomplete", ExceptionCount= 10, DriverName= "Chris", Assignee= "" },
                    new RouteModel  { Branch= "Med (2)", Route= "017", RouteDate = new DateTime(2017,1,10), StopCount= 12, RouteStatus= "Complete", ExceptionCount= 10, DriverName= "Chris", Assignee= "" },
                    new RouteModel  { Branch= "Med (2)", Route= "018", RouteDate = new DateTime(2017,1,10), StopCount= 12, RouteStatus= "Incomplete", ExceptionCount= 10, DriverName= "Chris", Assignee= "" },
                    new RouteModel  { Branch= "Med (2)", Route= "019", RouteDate = new DateTime(2017,1,10), StopCount= 12, RouteStatus= "Complete", ExceptionCount= 10, DriverName= "Chris", Assignee= "" },
                    new RouteModel  { Branch= "Med (2)", Route= "020", RouteDate = new DateTime(2017,1,10), StopCount= 12, RouteStatus= "Incomplete", ExceptionCount= 10, DriverName= "Chris", Assignee= "" },
                    new RouteModel  { Branch= "Bir (22)", Route= "021", RouteDate = new DateTime(2017,1,10), StopCount= 12, RouteStatus= "Complete", ExceptionCount= 10, DriverName= "Chris", Assignee= "" },
                    new RouteModel  { Branch= "Bir (22)", Route= "022", RouteDate = new DateTime(2017,1,10), StopCount= 12, RouteStatus= "Complete", ExceptionCount= 10, DriverName= "Chris", Assignee= "" },
                    new RouteModel  { Branch= "Bir (22)", Route= "023", RouteDate = new DateTime(2017,1,10), StopCount= 12, RouteStatus= "Complete", ExceptionCount= 10, DriverName= "Chris", Assignee= "" },
                    new RouteModel  { Branch= "Bir (22)", Route= "024", RouteDate = new DateTime(2017,1,10), StopCount= 12, RouteStatus= "Complete", ExceptionCount= 10, DriverName= "Chris", Assignee= "" },
                };

                if (!routes.Any())
                {
                    return this.Request.CreateResponse(HttpStatusCode.NotFound);
                }
                
                return this.Request.CreateResponse(HttpStatusCode.OK, routes);
            }
            catch (Exception ex)
            {
                return serverErrorResponseHandler.HandleException(Request, ex, "An error occurred when getting routes");
            }
        }


        //public HttpResponseMessage Get(string searchField = null, string searchTerm = null)
        //{
        //    try
        //    {
        //        var routeHeaders = this.routeRepository.GetRouteHeaders();

        //        if (!routeHeaders.Any())
        //        {
        //            return this.Request.CreateResponse(HttpStatusCode.NotFound);
        //        }

        //        var result = routeHeaders
        //                            .Select(p => new
        //                            {
        //                                Route = p.RouteNumber,
        //                                RouteDate = p.RouteDate.Value,
        //                                TotalDrops = p.TotalDrops,
        //                                DeliveryCleanCount = p.CleanJobs,
        //                                DeliveryExceptionCount = p.ExceptionJobs,
        //                                RouteStatusDescription = p.RouteStatusDescription,
        //                                DateTimeUpdated = p.DateUpdated,
        //                                RouteOwnerId = p.RouteOwnerId,
        //                                DriverName = p.DriverName
        //                            })
        //                            .ToList();

        //        return this.Request.CreateResponse(HttpStatusCode.OK, result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return serverErrorResponseHandler.HandleException(Request, ex, "An error occurred when getting routes");
        //    }
        //}
    }
}
