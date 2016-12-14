namespace PH.Well.UnitTests.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using NUnit.Framework;
    using Well.Domain;
    using Well.Domain.Enums;

    [TestFixture]
    public class JobDetailTests
    {
        public class CreateAuditEntryTests : JobDetailTests
        {
            [Test]
            public void GivenQtyAndDamagesAdded_ThenAuditEntryCreated()
            {
                var jobDetail = new JobDetail()
                {
                    PhProductCode = "12345",
                    ProdDesc = "Ind Potato Gratin 400g",
                    ShortQty = 1,
                    JobDetailDamages =
                        new List<JobDetailDamage>(new List<JobDetailDamage>()
                        {
                            new JobDetailDamage() {Qty = 1, JobDetailReason = JobDetailReason.Administration}
                        })
                };
                var originalJobDetail = new JobDetail() {ShortQty = 0};


                string invoiceNumber = "123456";
                string accountCode = "987654";
                string accountName = "BOB BING BANG";
                DateTime deliveryDate = DateTime.Now;
                var audit = jobDetail.CreateAuditEntry(originalJobDetail, invoiceNumber, accountCode, deliveryDate);

                string expectedEntry = $"Product: {jobDetail.PhProductCode} - {jobDetail.ProdDesc}. " +
                                       $"Short Qty changed from {originalJobDetail.ShortQty} to {jobDetail.ShortQty}. " +
                                       $"Damages added {jobDetail.JobDetailDamages[0].GetDamageString()}. ";
                Assert.AreEqual(expectedEntry, audit.Entry);
                Assert.AreEqual(AuditType.DeliveryLineUpdate, audit.Type);
                Assert.AreEqual(invoiceNumber, audit.InvoiceNumber);
                Assert.AreEqual(accountCode, audit.AccountCode);
                Assert.AreEqual(deliveryDate, audit.DeliveryDate);
            }

            [Test]
            public void GivenQtyAndDamagesRemoved_ThenAuditEntryCreated()
            {
                var jobDetail = new JobDetail()
                {
                    PhProductCode = "12345",
                    ProdDesc = "Ind Potato Gratin 400g",
                    ShortQty = 0
                };
                var originalJobDetail = new JobDetail() { ShortQty = 1,
                    JobDetailDamages =
                        new List<JobDetailDamage>(new List<JobDetailDamage>()
                        {
                            new JobDetailDamage() {Qty = 1, JobDetailReason = JobDetailReason.Administration}
                        })
                };

                var audit = jobDetail.CreateAuditEntry(originalJobDetail, "", "", DateTime.Now);

                string expectedEntry = $"Product: {jobDetail.PhProductCode} - {jobDetail.ProdDesc}. " +
                                       $"Short Qty changed from {originalJobDetail.ShortQty} to {jobDetail.ShortQty}. " +
                                       $"Damages removed, old damages {originalJobDetail.JobDetailDamages[0].GetDamageString()}. ";
                    
                Assert.AreEqual(expectedEntry, audit.Entry);
            }

            [Test]
            public void GivenDamagesChanged_ThenAuditEntryCreated()
            {
                var jobDetail = new JobDetail()
                {
                    PhProductCode = "12345",
                    ProdDesc = "Ind Potato Gratin 400g",
                    ShortQty = 0,
                    JobDetailDamages =
                        new List<JobDetailDamage>(new List<JobDetailDamage>()
                        {
                            new JobDetailDamage() {Qty = 1, JobDetailReason = JobDetailReason.Administration}
                        })
                };
                var originalJobDetail = new JobDetail()
                {
                    ShortQty = 0,
                    JobDetailDamages =
                        new List<JobDetailDamage>(new List<JobDetailDamage>()
                        {
                            new JobDetailDamage() {Qty = 4, JobDetailReason = JobDetailReason.Administration}
                        })
                };

                var audit = jobDetail.CreateAuditEntry(originalJobDetail, "", "", DateTime.Now);

                string expectedEntry = $"Product: {jobDetail.PhProductCode} - {jobDetail.ProdDesc}. " +
                                       $"Damages changed from '{originalJobDetail.JobDetailDamages[0].GetDamageString()}' " +
                                       $"to '{jobDetail.JobDetailDamages[0].GetDamageString()}'. ";
                                       
                Assert.AreEqual(expectedEntry, audit.Entry);
            }

            [Test]
            public void GivenNochanges_ThenAuditHasNoEntry()
            {
                var jobDetail = new JobDetail(){ShortQty = 0};
                var originalJobDetail = new JobDetail() { ShortQty = 0 };

                var audit = jobDetail.CreateAuditEntry(originalJobDetail, "", "", DateTime.Now);

                Assert.AreEqual(false, audit.HasEntry);
            }
        }

    }
}
