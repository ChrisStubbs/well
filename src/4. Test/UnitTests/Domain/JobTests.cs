namespace PH.Well.UnitTests.Domain
{
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
                var j = new Job
                {
                };

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
        }
    }
}