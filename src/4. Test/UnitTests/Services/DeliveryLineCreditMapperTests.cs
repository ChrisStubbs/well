namespace PH.Well.UnitTests.Services
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using Well.Domain.Enums;
    using Well.Domain.ValueObjects;
    using Well.Services.Mappers;

    [TestFixture]
    public class DeliveryLineCreditMapperTests
    {
        [Test]
        public void ShouldMapLineItemActionSubmitModel()
        {
       
            var lineItemActions = new List<LineItemActionSubmitModel>
            {
                new LineItemActionSubmitModel{ JobId  = 1,BranchId =2, ProductCode = "Prod1",Reason = JobDetailReason.RecallProduct,Source = JobDetailSource.Assembler,Quantity = 5},
                new LineItemActionSubmitModel{ JobId  = 2,BranchId =3, ProductCode = "Prod3",Reason = JobDetailReason.RecallProduct,Source = JobDetailSource.Assembler,Quantity = 5}
            };

            var results = new DeliveryLineCreditMapper().Map(lineItemActions);

            Assert.That(results.Count,Is.EqualTo(2));
            Assert.That(results[0].JobId,Is.EqualTo(lineItemActions[0].JobId));
            Assert.That(results[0].Reason, Is.EqualTo((int)lineItemActions[0].Reason));
            Assert.That(results[0].Source, Is.EqualTo((int)lineItemActions[0].Source));
            Assert.That(results[0].Quantity, Is.EqualTo(lineItemActions[0].Quantity));
            Assert.That(results[0].ProductCode, Is.EqualTo(lineItemActions[0].ProductCode));
        }
        
    }
}