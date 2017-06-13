namespace PH.Well.UnitTests.Domain
{
    using Factories;
    using NUnit.Framework;
    using Well.Domain;

    [TestFixture]
    public class JobTests
    {
        public class HasDamagesMethod : JobTests
        {
            [Test]
            public void HasDamagesFalseAsNoDamages()
            {
                var j = new Job();

                Assert.False(j.HasDamages);
            }

            [Test]
            public void HasDamagesFalseAsNoDamageQuantity()
            {
                var j = new Job
                {
                };
                var jd = new JobDetail();
                jd.JobDetailDamages.Add( new JobDetailDamage());

                var jd1 = new JobDetail();
                var jd2 = new JobDetail();
                j.JobDetails.AddRange(new[] { jd, jd1, jd2 });

                Assert.False(j.HasDamages);
            }

            [Test]
            public void HasDamagesTrueAsDamageQuantity()
            {
                var j = new Job
                {
                };
                var jd = new JobDetail();
                jd.JobDetailDamages.Add(new JobDetailDamage());

                var jd1 = new JobDetail();
                var jd2 = new JobDetail();
                jd2.JobDetailDamages.Add(new JobDetailDamage {Qty = 5});
                j.JobDetails.AddRange(new[] { jd, jd1, jd2 });

                Assert.True(j.HasDamages);
            }

            [Test]
            [TestCase(Well.Domain.Enums.JobDetailStatus.Res, ExpectedResult = true)]
            [TestCase(Well.Domain.Enums.JobDetailStatus.AwtInvNum, ExpectedResult = false)]
            public bool Jod_CanResolve(Well.Domain.Enums.JobDetailStatus status)
            {
                var detail = JobDetailFactory.New
                    .With(p => p.ShortsStatus = status)
                    .With(p => p.JobDetailDamages.Add(new JobDetailDamage { DamageStatus = status }))
                    .Build();

                var sut = JobFactory.New
                    .With(p => p.JobDetails.Add(detail))
                    .With(p => p.JobDetails.Add(detail))
                    .Build();

                return sut.CanResolve;
            }

            [Test]
            [TestCase(1, ExpectedResult = true)]
            [TestCase(0, ExpectedResult = false)]
            public bool Job_HasShorts(int shortQty)
            {
                var detail = JobDetailFactory.New
                    .With(p => p.ShortQty = shortQty)
                    .Build();

                var sut = JobFactory.New
                    .With(p => p.JobDetails.Add(detail))
                    .With(p => p.JobDetails.Add(detail))
                    .Build();

                return sut.HasShorts;

            }

            [Test]
            [TestCase(1, ExpectedResult = true)]
            [TestCase(0, ExpectedResult = false)]
            public bool Job_HasDamages(int qty)
            {
                var detail = JobDetailFactory.New
                    .With(p => p.JobDetailDamages.Add(new JobDetailDamage { Qty = qty }))
                    .Build();

                var sut = JobFactory.New
                    .With(p => p.JobDetails.Add(detail))
                    .Build();

                return sut.HasDamages;

            }
        }
    }
}