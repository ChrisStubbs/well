namespace PH.Well.UnitTests.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Web.Http;
    using PH.Well.Common.Contracts;
    using Moq;
    using NUnit.Framework;
    using PH.Well.Api.Controllers;
    using PH.Well.Repositories.Contracts;
    using Well.Api.Models;
    using Well.Domain;
    using Well.Domain.Enums;

    [TestFixture]
    public class AuditControllerTests : BaseControllerTests<AuditController>
    {
        private Mock<IAuditRepository> auditRepo;
        private Mock<IServerErrorResponseHandler> serverErrorResponseHandler;

        [SetUp]
        public void Setup()
        {
            this.auditRepo = new Mock<IAuditRepository>(MockBehavior.Strict);
            this.serverErrorResponseHandler = new Mock<IServerErrorResponseHandler>(MockBehavior.Strict);

            this.Controller = new AuditController(this.auditRepo.Object, 
                this.serverErrorResponseHandler.Object);
            SetupController();
        }

        public class TheGetMethod : AuditControllerTests
        {
            [Test]
            public void HasGetAttribute()
            {
                MethodInfo controllerMethod = GetMethod(c => c.Get());

                var attribute = GetAttributes<HttpGetAttribute>(controllerMethod).FirstOrDefault();
                Assert.IsNotNull(attribute);
            }

            [Test]
            public void HasCorrectRouteAttribute()
            {
                MethodInfo controllerMethod = GetMethod(c => c.Get());

                var routeAttribute = GetAttributes<RouteAttribute>(controllerMethod).FirstOrDefault();
                Assert.IsNotNull(routeAttribute);
                Assert.AreEqual("audits", routeAttribute.Template);
            }

            [Test]
            public void GivenNoAudits_ThenReturnsNotFound()
            {
                auditRepo.Setup(a => a.Get()).Returns((List<Audit>)null);

                var results = Controller.Get();

                Assert.AreEqual(HttpStatusCode.NotFound, results.StatusCode);
            }

            [Test]
            public void GivenAudits_ThenReturnsOKWithAudits()
            {
                var audits = new List<Audit>()
                {
                    new Audit()
                    {
                        Entry = "Something changed to something else",
                        Type = AuditType.DeliveryLineUpdate,
                        InvoiceNumber = "123456",
                        AccountName = "Bob",
                        AccountCode = "987654",
                        DeliveryDate = new DateTime(2016, 09, 14),
                        CreatedBy = "Dirk.Diggler",
                        DateCreated = new DateTime(2016, 10, 20)
                    },
                    new Audit() {}
                };
                auditRepo.Setup(a => a.Get()).Returns(audits);

                var results = Controller.Get();

                var response = GetResponseObject<List<AuditModel>>(results);
                Assert.AreEqual(HttpStatusCode.OK, results.StatusCode);

                Assert.AreEqual(audits[0].Entry, response[0].Entry);
                Assert.AreEqual(audits[0].Type.ToString(), response[0].Type);
                Assert.AreEqual(audits[0].InvoiceNumber, response[0].InvoiceNumber);
                Assert.AreEqual(audits[0].AccountName, response[0].AccountName);
                Assert.AreEqual(audits[0].AccountCode, response[0].AccountCode);
                Assert.AreEqual(audits[0].DeliveryDate, response[0].DeliveryDate);
                Assert.AreEqual(audits[0].DateCreated, response[0].AuditDate);
                Assert.AreEqual(audits[0].CreatedBy, response[0].AuditBy);
            }
        }
    }
}