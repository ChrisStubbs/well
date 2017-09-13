namespace PH.Well.UnitTests.Services
{
    using System.Collections.Generic;
    using Factories;
    using NUnit.Framework;
    using Well.Domain;
    using Well.Domain.Enums;
    using Well.Domain.ValueObjects;
    using Well.Services.Mappers;

    [TestFixture]
    public class DeliveryLineCreditMapperTests
    {
        [Test]
        public void ShouldMapLineItemActionSubmitModel()
        {
       
            var creditAction1 = LineItemFactory.New.AddCreditAction().Build();
            var creditAction2 = LineItemFactory.New.AddCreditAction().Build();

            var job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.Invalid /*doesn't really matter the status*/)
                .Build();
            job.LineItems.Add(creditAction1);
            job.LineItems.Add(creditAction2);


            var results = new DeliveryLineCreditMapper().Map(job);

            Assert.That(results.Count,Is.EqualTo(2));

            Assert.That(results[0].JobId,Is.EqualTo(job.Id));
            Assert.That(results[0].Reason, Is.EqualTo((int)creditAction1.LineItemActions[0].Reason));
            Assert.That(results[0].Source, Is.EqualTo((int)creditAction1.LineItemActions[0].Source));
            Assert.That(results[0].Quantity, Is.EqualTo(creditAction1.LineItemActions[0].Quantity));
            Assert.That(results[0].ProductCode, Is.EqualTo(creditAction1.ProductCode));
        }
        
    }
}