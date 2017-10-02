namespace PH.Well.UnitTests.Api.Mapper
{
    using System;

    using NUnit.Framework;

    using PH.Well.Api.Mapper;
    using PH.Well.Api.Models;
    using PH.Well.Domain;

    [TestFixture]
    public class SeasonalDateMapperTests
    {
        public class TheMapMethod : SeasonalDateMapperTests
        {
            [Test]
            public void ShouldMapCorrectly()
            {
                var model = new SeasonalDateModel
                {
                    Description = "Easter",
                    FromDate = DateTime.Now,
                    ToDate = DateTime.Now
                };

                model.Branches.Add(new Branch { Id = 1, Name = "Birtley" });

                var seasonalDate = new SeasonalDateMapper().Map(model);

                Assert.That(seasonalDate.Description, Is.EqualTo(model.Description));
                Assert.That(seasonalDate.From, Is.EqualTo(model.FromDate));
                Assert.That(seasonalDate.To, Is.EqualTo(model.ToDate));
                Assert.That(seasonalDate.Branches.Count, Is.EqualTo(1));
                Assert.That(seasonalDate.Branches[0].Id, Is.EqualTo(1));
                Assert.That(seasonalDate.Branches[0].Name, Is.EqualTo("Birtley"));
            }
        }
    }
}