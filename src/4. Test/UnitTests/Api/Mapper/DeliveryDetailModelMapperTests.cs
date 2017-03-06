namespace PH.Well.UnitTests.Api.Mapper
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using PH.Well.Api.Mapper;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.UnitTests.Factories;
    using Well.Domain;
    using Well.Domain.Enums;

    [TestFixture]
    public class DeliveryDetailModelMapperTests
    {
        private DeliveryToDetailMapper mapper;

        [SetUp]
        public void Setup()
        {
            this.mapper = new DeliveryToDetailMapper();
        }

        [Test]
        public void ShouldMapLinesAndDetailToViewModel()
        {
            var deliveryDetail = DeliveryDetailFactory.New.Build();

            var line1 = DeliveryLineFactory.New.Build();
            var line2 = DeliveryLineFactory.New.With(x => x.LineNo = 2).Build();

            var lines = new List<DeliveryLine> { line1, line2 };

            var model = this.mapper.Map(lines, deliveryDetail);

            Assert.That(model.Id, Is.EqualTo(deliveryDetail.Id));
            Assert.That(model.AccountCode, Is.EqualTo(deliveryDetail.AccountCode));
            Assert.That(model.AccountName, Is.EqualTo(deliveryDetail.AccountName));
            Assert.That(model.AccountAddress, Is.EqualTo(deliveryDetail.AccountAddress));
            Assert.That(model.InvoiceNumber, Is.EqualTo(deliveryDetail.InvoiceNumber));
            Assert.That(model.ContactName, Is.EqualTo(deliveryDetail.ContactName));
            Assert.That(model.PhoneNumber, Is.EqualTo(deliveryDetail.PhoneNumber));
            Assert.That(model.MobileNumber, Is.EqualTo(deliveryDetail.MobileNumber));
            Assert.That(model.JobStatus, Is.EqualTo(deliveryDetail.JobStatus));
            Assert.That(model.CanAction, Is.EqualTo(deliveryDetail.CanAction));
            Assert.That(model.GrnNumber, Is.EqualTo(deliveryDetail.GrnNumber));

            var modelLine1 = model.ExceptionDeliveryLines[0];
            var modelLine2 = model.ExceptionDeliveryLines[1];

            Assert.That(modelLine1.JobDetailId, Is.EqualTo(line1.JobDetailId));
            Assert.That(modelLine1.JobId, Is.EqualTo(line1.JobId));
            Assert.That(modelLine1.LineNo, Is.EqualTo(line1.LineNo));
            Assert.That(modelLine1.ProductCode, Is.EqualTo(line1.ProductCode));
            Assert.That(modelLine1.ProductDescription, Is.EqualTo(line1.ProductDescription));
            Assert.That(modelLine1.Value, Is.EqualTo(line1.Value.ToString()));
            Assert.That(modelLine1.InvoicedQuantity, Is.EqualTo(line1.InvoicedQuantity));
            Assert.That(modelLine1.DeliveredQuantity, Is.EqualTo(line1.DeliveredQuantity));
            Assert.That(modelLine1.DamagedQuantity, Is.EqualTo(line1.DamagedQuantity));
            Assert.That(modelLine1.ShortQuantity, Is.EqualTo(line1.ShortQuantity));

            Assert.That(modelLine2.JobId, Is.EqualTo(line2.JobId));
            Assert.That(modelLine2.LineNo, Is.EqualTo(line2.LineNo));
            Assert.That(modelLine2.ProductCode, Is.EqualTo(line2.ProductCode));
            Assert.That(modelLine2.ProductDescription, Is.EqualTo(line2.ProductDescription));
            Assert.That(modelLine2.Value, Is.EqualTo(line2.Value.ToString()));
            Assert.That(modelLine2.InvoicedQuantity, Is.EqualTo(line2.InvoicedQuantity));
            Assert.That(modelLine2.DeliveredQuantity, Is.EqualTo(line2.DeliveredQuantity));
            Assert.That(modelLine2.DamagedQuantity, Is.EqualTo(line2.DamagedQuantity));
            Assert.That(modelLine2.ShortQuantity, Is.EqualTo(line2.ShortQuantity));
        }
    }
}