using NUnit.Framework;
using PH.Well.Domain;
using PH.Well.UnitTests.Factories;
using static PH.Well.Domain.Mappers.AutoMapperConfig;

namespace PH.Well.UnitTests.DTOMappers
{
    [TestFixture]
    class StopDTOTests
    {
        [Test]
        public void StopDTOTests_Should_Store_Correct_Values()
        {
            var sut = StopFactoryDTO.New
                .AddEntityAttribute("ALLOWOVERS", "Y")
                .AddEntityAttribute("CUSTUNATT", "N")
                .AddEntityAttribute("PHUNATT", null)
                .AddEntityAttribute("ACTPAYCASH", "10.6")
                .AddEntityAttribute("ACTPAYCHEQ", "1.6")
                .AddEntityAttribute("ACTPAYCARD", "2.6")
                .AddEntityAttribute("ACCBAL", "19.6")
                .With(p => p.TextField5 = "1 8")
                .Build();

            Assert.That(sut.AllowOvers, Is.True);
            Assert.That(sut.CustUnatt, Is.False);
            Assert.That(sut.PHUnatt, Is.False);
            Assert.That(sut.ActualPaymentCash, Is.EqualTo(10.6M));
            Assert.That(sut.ActualPaymentCheque, Is.EqualTo(1.6M));
            Assert.That(sut.ActualPaymentCard, Is.EqualTo(2.6M));
            Assert.That(sut.AccountBalance, Is.EqualTo(19.6M));
            Assert.That(sut.DropId, Is.EqualTo("8"));

            sut.TextField5 = "";
            Assert.That(sut.DropId, Is.EqualTo("8"));
        }

        [Test]
        public void Should_Map_From_StopDTOTests_To_Stop()
        {
            var stopDTO = StopFactoryDTO.New
                .AddEntityAttribute("ALLOWOVERS", "Y")
                .AddEntityAttribute("CUSTUNATT", "N")
                .AddEntityAttribute("PHUNATT", null)
                .AddEntityAttribute("ACTPAYCASH", "10.6")
                .AddEntityAttribute("ACTPAYCHEQ", "1.6")
                .AddEntityAttribute("ACTPAYCARD", "2.6")
                .AddEntityAttribute("ACCBAL", "19.6")
                .Build();

            var sut = Mapper.Map<StopDTO, Stop>(stopDTO);

            Assert.That(sut.AllowOvers, Is.EqualTo(stopDTO.AllowOvers));
            Assert.That(sut.CustUnatt, Is.EqualTo(stopDTO.CustUnatt));
            Assert.That(sut.PHUnatt, Is.EqualTo(stopDTO.PHUnatt));
            Assert.That(sut.ActualPaymentCash, Is.EqualTo(stopDTO.ActualPaymentCash));
            Assert.That(sut.ActualPaymentCheque, Is.EqualTo(stopDTO.ActualPaymentCheque));
            Assert.That(sut.ActualPaymentCard, Is.EqualTo(stopDTO.ActualPaymentCard));
            Assert.That(sut.AccountBalance, Is.EqualTo(stopDTO.AccountBalance));
        }
    }
}
