using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PH.Well.Domain.Enums;
using PH.Well.Domain.Extensions;
using PH.Well.Domain.ValueObjects;
using PH.Well.UnitTests.Factories;

namespace PH.Well.UnitTests.Common
{

    [TestFixture]
    class JobExtensionsTests
    {
        [Test]
        [Description("Check if the job HasUnresolvedActions")]
        [Category("Extension")]
        [Category("Job")]
        public void JobShouldHasUnresolvedActions()
        {
            var line = LineItemFactory.New
                .AddNotDefinedAction()
                .Build();

            var j = JobFactory.New
                    .With(p => p.LineItems.Add(line))
                    .Build();

            Assert.IsTrue(j.HasUnresolvedActions());
        }

        [Test]
        [Description("Check if the job doesn't  HasUnresolvedActions is credited")]
        [Category("Extension")]
        [Category("Job")]
        public void JobShouldNotHasUnresolvedActions()
        {
            var line = LineItemFactory.New
                .AddCreditAction()
                .Build();

            var j = JobFactory.New
                    .With(p => p.LineItems.Add(line))
                    .Build();

            Assert.IsFalse(j.HasUnresolvedActions());
        }

        [Test]
        [Description("Check if the job doesn't HasUnresolvedActions because it is closed")]
        [Category("Extension")]
        [Category("Job")]
        public void ClosedJobShouldNotHasUnresolvedActions()
        {
            var line = LineItemFactory.New
                .AddNotDefinedAction()
                .Build();

            var j = JobFactory.New
                    .With(p => p.LineItems.Add(line))
                    .With(p => p.ResolutionStatus = ResolutionStatus.Closed)
                    .Build();

            Assert.IsFalse(j.HasUnresolvedActions());
        }

        [Test]
        [Description("Check if the ActivitySourceDetail HasUnresolvedActions")]
        [Category("Extension")]
        [Category("ActivitySourceDetail")]
        public void ActivitySourceDetailShouldHasUnresolvedActions()
        {
            var sut = new ActivitySourceDetail
            {
                ResolutionStatus = ResolutionStatus.Approved,
                HasNoDefinedActions = true
            };

            Assert.IsTrue(sut.HasUnresolvedActions());
        }

        [Test]
        [Description("Check if the ActivitySourceDetail doesn't HasUnresolvedActions because is credited")]
        [Category("Extension")]
        [Category("ActivitySourceDetail")]
        public void ActivitySourceDetailShouldNotHasUnresolvedActions()
        {
            var sut = new ActivitySourceDetail
            {
                ResolutionStatus = ResolutionStatus.Credited,
                HasNoDefinedActions = false
            };

            Assert.IsFalse(sut.HasUnresolvedActions());
        }

        [Test]
        [Description("Check if the job doesn't HasUnresolvedActions because it is closed")]
        [Category("Extension")]
        [Category("ActivitySourceDetail")]
        public void ClosedActivitySourceDetailShouldNotHasUnresolvedActions()
        {
            var sut = new ActivitySourceDetail
            {
                ResolutionStatus = ResolutionStatus.Closed
            };

            Assert.IsFalse(sut.HasUnresolvedActions());
        }
    }
}
