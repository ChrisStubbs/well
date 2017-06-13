using NUnit.Framework;
using PH.Well.Domain;
using PH.Well.UnitTests.Factories;
using static PH.Well.Domain.Mappers.AutoMapperConfig;

namespace PH.Well.UnitTests.DTOMappers
{
    [TestFixture]
    class JobDTOTests
    {
        [Test]
        public void JobdDTO_Should_Store_Correct_Values()
        {
            var sut = JobFactoryDTO.New
                .AddEntityAttribute("ORDOUTERS", "10")
                .AddEntityAttribute("INVOUTERS", "10")
                .AddEntityAttribute("COLOUTERS", "10")
                .AddEntityAttribute("COLBOXES", "33")
                .AddEntityAttribute("RECALLPRD", "Y")
                .AddEntityAttribute("ALLOWSOCRD", "Y")
                .AddEntityAttribute("COD", "COD")
                .AddEntityAttribute("SandwchOrd", "Y")
                .AddEntityAttribute("AllowReOrd", "Y")
                .AddEntityAttribute("ACTLOGNO", "ACTLOGNO")
                .AddEntityAttributeValues("GRNNO", "GRNNO")
                .AddEntityAttribute("GRNREFREAS", "GRNREFREAS")
                .AddEntityAttributeValues("OUTERCOUNT", "36")
                .AddEntityAttributeValues("TOTOVER", "1")
                .AddEntityAttributeValues("TOTSHORT", "10")
                .AddEntityAttributeValues("DETOVER", "1")
                .AddEntityAttributeValues("DETSHORT", "1")
                .AddEntityAttributeValues("PICKED", "N")
                .AddEntityAttributeValues("INVALUE", "10")
                .With(p => p.SequenceXml = "23")
                .Build();

            Assert.That(sut.Sequence, Is.EqualTo(23));
            Assert.That(sut.OrdOuters, Is.EqualTo(10));
            Assert.That(sut.InvOuters, Is.EqualTo(10));
            Assert.That(sut.ColOuters, Is.EqualTo(10));
            Assert.That(sut.ReCallPrd, Is.True);
            Assert.That(sut.AllowSoCrd, Is.True);
            Assert.That(sut.Cod, Is.EqualTo("COD"));
            Assert.That(sut.SandwchOrd, Is.True);
            Assert.That(sut.AllowReOrd, Is.True);
            Assert.That(sut.ActionLogNumber, Is.EqualTo("ACTLOGNO"));
            Assert.That(sut.GrnNumber, Is.EqualTo("GRNNO"));
            Assert.That(sut.GrnRefusedReason, Is.EqualTo("GRNREFREAS"));
            Assert.That(sut.OuterCount, Is.EqualTo(36));
            Assert.That(sut.TotalOutersOver, Is.EqualTo(1));
            Assert.That(sut.TotalOutersShort, Is.EqualTo(10));
            Assert.That(sut.DetailOutersOver, Is.EqualTo(1));
            Assert.That(sut.DetailOutersShort, Is.EqualTo(1));
            Assert.That(sut.Picked, Is.False);
            Assert.That(sut.InvoiceValue, Is.EqualTo(10));
            Assert.That(sut.OuterDiscrepancyFound, Is.True);
            Assert.That(sut.OuterDiscrepancyFound, Is.True);
        }

        [Test]
        public void Should_Map_From_JobDTO_To_Job()
        {
            var jobDTO = JobFactoryDTO.New
                .AddEntityAttribute("ORDOUTERS", "10")
                .AddEntityAttribute("INVOUTERS", "10")
                .AddEntityAttribute("COLOUTERS", "10")
                .AddEntityAttribute("COLBOXES", "33")
                .AddEntityAttribute("RECALLPRD", "Y")
                .AddEntityAttribute("ALLOWSOCRD", "Y")
                .AddEntityAttribute("COD", "COD")
                .AddEntityAttribute("SandwchOrd", "Y")
                .AddEntityAttribute("AllowReOrd", "Y")
                .AddEntityAttribute("ACTLOGNO", "ACTLOGNO")
                .AddEntityAttributeValues("GRNNO", "GRNNO")
                .AddEntityAttribute("GRNREFREAS", "GRNREFREAS")
                .AddEntityAttributeValues("OUTERCOUNT", "36")
                .AddEntityAttributeValues("TOTOVER", "1")
                .AddEntityAttributeValues("TOTSHORT", "10")
                .AddEntityAttributeValues("DETOVER", "1")
                .AddEntityAttributeValues("DETSHORT", "1")
                .AddEntityAttributeValues("PICKED", "Y")
                .AddEntityAttributeValues("INVALUE", "10")
                .Build();

            var sut = Mapper.Map<JobDTO, Job>(jobDTO);

            Assert.That(sut.OrdOuters, Is.EqualTo(jobDTO.OrdOuters));
            Assert.That(sut.InvOuters, Is.EqualTo(jobDTO.InvOuters));
            Assert.That(sut.ColOuters, Is.EqualTo(jobDTO.ColOuters));
            Assert.That(sut.ReCallPrd, Is.EqualTo(jobDTO.ReCallPrd));
            Assert.That(sut.AllowSoCrd, Is.EqualTo(jobDTO.AllowSoCrd));
            Assert.That(sut.Cod, Is.EqualTo(jobDTO.Cod));
            Assert.That(sut.SandwchOrd, Is.EqualTo(jobDTO.SandwchOrd));
            Assert.That(sut.AllowReOrd, Is.EqualTo(jobDTO.AllowReOrd));
            Assert.That(sut.ActionLogNumber, Is.EqualTo(jobDTO.ActionLogNumber));
            Assert.That(sut.GrnNumber, Is.EqualTo(jobDTO.GrnNumber));
            Assert.That(sut.GrnRefusedReason, Is.EqualTo(jobDTO.GrnRefusedReason));
            Assert.That(sut.OuterCount, Is.EqualTo(jobDTO.OuterCount));
            Assert.That(sut.TotalOutersOver, Is.EqualTo(jobDTO.TotalOutersOver));
            Assert.That(sut.TotalOutersShort, Is.EqualTo(jobDTO.TotalOutersShort));
            Assert.That(sut.DetailOutersOver, Is.EqualTo(jobDTO.DetailOutersOver));
            Assert.That(sut.DetailOutersShort, Is.EqualTo(jobDTO.DetailOutersShort));
            Assert.That(sut.Picked, Is.EqualTo(jobDTO.Picked));
            Assert.That(sut.InvoiceValue, Is.EqualTo(jobDTO.InvoiceValue));
            Assert.That(sut.OuterDiscrepancyFound, Is.True);
            Assert.That(sut.OuterDiscrepancyFound, Is.True);
        }
    }
}
