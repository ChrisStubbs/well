namespace PH.Well.UnitTests.Api.Validators
{
    using System.Collections.Generic;

    using Moq;

    using NUnit.Framework;

    using PH.Well.Api.Models;
    using PH.Well.Api.Validators;
    using PH.Well.Domain;
    using PH.Well.Repositories.Contracts;

    [TestFixture]
    public class CreditThresholdValidatorTests
    {
        // threshold level is required
        // threshold should be int
        // threshold should be in range 1-1000000
        // at least one branch selected
        // one threshold to one branch only

        private Mock<ICreditThresholdRepository> repository;

        private CreditThresholdValidator validator;

        [SetUp]
        public void Setup()
        {
            this.repository = new Mock<ICreditThresholdRepository>(MockBehavior.Strict);

            this.validator = new CreditThresholdValidator(this.repository.Object);
        }

        [Test]
        public void ThresholdLevelRequired()
        {
            this.repository.Setup(x => x.GetAll()).Returns(new List<CreditThreshold>());

            var model = new CreditThresholdModel { ThresholdLevel = "Level", Threshold = 100 };
            model.Branches.Add(new Branch());

            Assert.IsFalse(this.validator.IsValid(model, false));

            Assert.That(this.validator.Errors.Count, Is.EqualTo(1));

            Assert.That(this.validator.Errors[0], Is.EqualTo("Threshold level is required!"));
        }

        [Test]
        public void ThresholdRequired()
        {
            this.repository.Setup(x => x.GetAll()).Returns(new List<CreditThreshold>());

            var model = new CreditThresholdModel { ThresholdLevel = "Level 1", Threshold = null };
            model.Branches.Add(new Branch());

            Assert.IsFalse(this.validator.IsValid(model, false));

            Assert.That(this.validator.Errors.Count, Is.EqualTo(1));

            Assert.That(this.validator.Errors[0], Is.EqualTo("Threshold is required!"));
        }

        [Test]
        public void ThresholdInRange()
        {
            this.repository.Setup(x => x.GetAll()).Returns(new List<CreditThreshold>());

            var model = new CreditThresholdModel { ThresholdLevel = "Level 1", Threshold = 0 };
            model.Branches.Add(new Branch());

            Assert.IsFalse(this.validator.IsValid(model, false));

            Assert.That(this.validator.Errors.Count, Is.EqualTo(1));

            Assert.That(this.validator.Errors[0], Is.EqualTo("Threshold range is 1 to 1000000"));

            this.ClearErrors();

            model.Threshold = 1000001;

            Assert.IsFalse(this.validator.IsValid(model, false));

            Assert.That(this.validator.Errors.Count, Is.EqualTo(1));

            Assert.That(this.validator.Errors[0], Is.EqualTo("Threshold range is 1 to 1000000"));

            this.ClearErrors();

            model.Threshold = 1000000;

            Assert.IsTrue(this.validator.IsValid(model, false));

            this.ClearErrors();

            model.Threshold = 1;

            Assert.IsTrue(this.validator.IsValid(model, false));
        }

        [Test]
        public void BranchRequired()
        {
            this.repository.Setup(x => x.GetAll()).Returns(new List<CreditThreshold>());

            var model = new CreditThresholdModel { ThresholdLevel = "Level 1", Threshold = 54 };

            Assert.IsFalse(this.validator.IsValid(model, false));

            Assert.That(this.validator.Errors.Count, Is.EqualTo(1));

            Assert.That(this.validator.Errors[0], Is.EqualTo("Branch is required!"));
        }

        [Test]
        public void ThresholdUniqueToBranch()
        {
            var existingThreshold = new CreditThreshold { ThresholdLevelId = 1, Value = 100 };
            var thresholds = new List<CreditThreshold> { existingThreshold };

            var model = new CreditThresholdModel { ThresholdLevel = "Level 1", Threshold = 100 };

            var medway = new Branch { Name = "Medway", Id = 1 };

            existingThreshold.Branches.Add(medway);
            model.Branches.Add(medway);

            this.repository.Setup(x => x.GetAll()).Returns(thresholds);

            Assert.IsFalse(this.validator.IsValid(model, false));

            Assert.That(this.validator.Errors.Count, Is.EqualTo(1));

            Assert.That(this.validator.Errors[0], Is.EqualTo("Branches already have a threshold assigned!"));
        }

        [Test]
        public void ThresholdUniqueToMultipleBranch()
        {
            var existingThreshold = new CreditThreshold { ThresholdLevelId = 1, Value = 100 };
            var thresholds = new List<CreditThreshold> { existingThreshold };

            var model = new CreditThresholdModel { ThresholdLevel = "Level 1", Threshold = 100 };

            var medway = new Branch { Name = "Medway", Id = 1 };
            var birtley = new Branch { Name = "Birtley", Id = 5 };

            existingThreshold.Branches.Add(medway);
            existingThreshold.Branches.Add(birtley);
            model.Branches.Add(medway);

            this.repository.Setup(x => x.GetAll()).Returns(thresholds);

            Assert.IsFalse(this.validator.IsValid(model, false));

            Assert.That(this.validator.Errors.Count, Is.EqualTo(1));

            Assert.That(this.validator.Errors[0], Is.EqualTo("Branches already have a threshold assigned!"));
        }

        [Test]
        public void ThresholdUniqueToMultipleBranches()
        {
            var existingThreshold = new CreditThreshold { ThresholdLevelId = 1, Value = 100 };
            var thresholds = new List<CreditThreshold> { existingThreshold };

            var model = new CreditThresholdModel { ThresholdLevel = "Level 1", Threshold = 100 };

            var medway = new Branch { Name = "Medway", Id = 1 };
            var birtley = new Branch { Name = "Birtley", Id = 5 };

            existingThreshold.Branches.Add(medway);
            model.Branches.Add(medway);
            model.Branches.Add(birtley);

            this.repository.Setup(x => x.GetAll()).Returns(thresholds);

            Assert.IsFalse(this.validator.IsValid(model, false));

            Assert.That(this.validator.Errors.Count, Is.EqualTo(1));

            Assert.That(this.validator.Errors[0], Is.EqualTo("Branches already have a threshold assigned!"));
        }

        [Test]
        public void ThresholdUniqueToBranchesValid()
        {
            var existingThreshold = new CreditThreshold { ThresholdLevelId = 1, Value = 100 };
            var thresholds = new List<CreditThreshold> { existingThreshold };

            var model = new CreditThresholdModel { ThresholdLevel = "Level 1", Threshold = 100 };

            var medway = new Branch { Name = "Medway", Id = 1 };
            var birtley = new Branch { Name = "Birtley", Id = 5 };

            existingThreshold.Branches.Add(medway);
            model.Branches.Add(birtley);

            this.repository.Setup(x => x.GetAll()).Returns(thresholds);

            Assert.IsTrue(this.validator.IsValid(model, false));

            Assert.That(this.validator.Errors.Count, Is.EqualTo(0));
        }

        private void ClearErrors()
        {
            this.validator.Errors = new List<string>();
        }
    }
}