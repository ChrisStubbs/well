namespace PH.Well.UnitTests.Services
{
    using NUnit.Framework;

    using PH.Well.Domain;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Services.EpodServices;

    [TestFixture]
    public class OrderImportMapperTests
    {
        private OrderImportMapper mapper;

        [SetUp]
        public void Setup()
        {
            this.mapper = new OrderImportMapper();
        }

        [Test]
        public void StopUpdateMapper()
        {
            var from = new StopUpdate();

            from.PlannedStopNumber = "5";
            from.ShellActionIndicator = "A";

            var to = new Stop();

            this.mapper.Map(from, to);

            Assert.That(to.PlannedStopNumber, Is.EqualTo(from.PlannedStopNumber));
            Assert.That(to.ShellActionIndicator, Is.EqualTo(from.ShellActionIndicator));
        }


        [Test]
        public void JobUpdateMapper()
        {
            var from = new JobUpdate();

            from.Sequence = 1;
            from.JobTypeCode = "A";
            from.PhAccount = "12000.001";
            from.PickListRef = "4433";
            from.InvoiceNumber = "223344";
            from.CustomerRef = "Ref";

            from.EntityAttributes.Add(new EntityAttribute { Code = "PICKED" });
            from.EntityAttributes.Add(new EntityAttribute { Code = "ORDOUTERS" });
            from.EntityAttributes.Add(new EntityAttribute { Code = "INVOUTERS" });
            from.EntityAttributes.Add(new EntityAttribute { Code = "ALLOWSOCRD" });
            from.EntityAttributes.Add(new EntityAttribute { Code = "COD" });
            from.EntityAttributes.Add(new EntityAttribute { Code = "ALLOWREORD" });
            
            var to = new Job();

            this.mapper.Map(from, to);

            Assert.That(to.Sequence, Is.EqualTo(from.Sequence));
            Assert.That(to.JobTypeCode, Is.EqualTo(from.JobTypeCode));
            Assert.That(to.PhAccount, Is.EqualTo(from.PhAccount));
            Assert.That(to.PickListRef, Is.EqualTo(from.PickListRef));
            Assert.That(to.InvoiceNumber, Is.EqualTo(from.InvoiceNumber));
            Assert.That(to.CustomerRef, Is.EqualTo(from.CustomerRef));
            Assert.That(to.PerformanceStatus, Is.EqualTo(PerformanceStatus.Notdef));
        }

       

        [Test]
        public void JobDetailUpdateMapper()
        {
            var from = new JobDetailUpdate();

            from.PhProductCode = "2001";
            from.ProdDesc = "Mars";
            from.OrderedQty = 45;
            from.UnitMeasure = "3M";
            from.PhProductType = "Lubes";
            from.PackSize = "30";
            from.SingleOrOuter = "1";
            from.SSCCBarcode = "11222121";
            from.SkuGoodsValue = 3;

            var to = new JobDetail();

            this.mapper.Map(from, to);

            Assert.That(to.PhProductCode, Is.EqualTo(from.PhProductCode));
            Assert.That(to.ProdDesc, Is.EqualTo(from.ProdDesc));
            Assert.That(to.OrderedQty, Is.EqualTo(from.OrderedQty));
            Assert.That(to.UnitMeasure, Is.EqualTo(from.UnitMeasure));
            Assert.That(to.PhProductType, Is.EqualTo(from.PhProductType));
            Assert.That(to.PackSize, Is.EqualTo(from.PackSize));
            Assert.That(to.SingleOrOuter, Is.EqualTo(from.SingleOrOuter));
            Assert.That(to.SSCCBarcode, Is.EqualTo(from.SSCCBarcode));
            Assert.That(to.SkuGoodsValue, Is.EqualTo(from.SkuGoodsValue));
        }
    }
}