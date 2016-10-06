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
    public class CleanPreferenceValidatorTests
    {
        // days should be int
        // day should be in range 1-100
        // at least one branch selected
        // one branch per clean preference. IE medway can not be assigned a clean preference more than once

        private Mock<ICleanPreferenceRepository> repository;
        private CleanPreferenceValidator validator;

        [SetUp]
        public void Setup()
        {
            this.repository = new Mock<ICleanPreferenceRepository>(MockBehavior.Strict);
            this.validator = new CleanPreferenceValidator(this.repository.Object);
        }

        [Test]
        public void DaysRequired()
        {
            this.repository.Setup(x => x.GetAll()).Returns(new List<CleanPreference>());

            var model = new CleanPreferenceModel { Days = null };
            model.Branches.Add(new Branch());
            
            Assert.IsFalse(this.validator.IsValid(model, false));

            Assert.That(this.validator.Errors.Count, Is.EqualTo(1));

            Assert.That(this.validator.Errors[0], Is.EqualTo("Days is required!"));
        }

        [Test]
        public void DaysInRange()
        {
            this.repository.Setup(x => x.GetAll()).Returns(new List<CleanPreference>());

            var model = new CleanPreferenceModel { Days = 0 };
            model.Branches.Add(new Branch());

            Assert.IsFalse(this.validator.IsValid(model, false));

            Assert.That(this.validator.Errors.Count, Is.EqualTo(1));

            Assert.That(this.validator.Errors[0], Is.EqualTo("Days range is 1 to 100!"));

            model.Days = 101;

            this.ClearErrors();
            
            this.validator.IsValid(model, false);

            Assert.That(this.validator.Errors.Count, Is.EqualTo(1));

            Assert.That(this.validator.Errors[0], Is.EqualTo("Days range is 1 to 100!"));

            model.Days = 1;

            this.ClearErrors();

            this.validator.IsValid(model, false);

            Assert.That(this.validator.Errors.Count, Is.EqualTo(0));

            model.Days = 100;

            this.ClearErrors();

            this.validator.IsValid(model, false);

            Assert.That(this.validator.Errors.Count, Is.EqualTo(0));

            model.Days = 50;

            this.ClearErrors();

            this.validator.IsValid(model, false);

            Assert.That(this.validator.Errors.Count, Is.EqualTo(0));
        }

        [Test]
        public void BranchRequired()
        {
            this.repository.Setup(x => x.GetAll()).Returns(new List<CleanPreference>());

            var model = new CleanPreferenceModel { Days = 24 };

            Assert.IsFalse(this.validator.IsValid(model, false));

            Assert.That(this.validator.Errors.Count, Is.EqualTo(1));

            Assert.That(this.validator.Errors[0], Is.EqualTo("Branch is required!"));
        }

        [Test]
        public void BranchAttachedToOnePreferenceOnly1Branch()
        {
            var medway = new Branch() { Name = "Medway", Id = 3 };

            var model = new CleanPreferenceModel { Days = 24 };
            model.Branches.Add(medway);

            var cleanPreference = new CleanPreference();
            cleanPreference.Branches.Add(medway);

            var cleanPreferences = new List<CleanPreference> { cleanPreference };

            this.repository.Setup(x => x.GetAll()).Returns(cleanPreferences);

            Assert.IsFalse(this.validator.IsValid(model, false));

            Assert.That(this.validator.Errors.Count, Is.EqualTo(1));

            Assert.That(this.validator.Errors[0], Is.EqualTo("Branch already has a clean preference assigned!"));
        }

        [Test]
        public void BranchAttachedToOnePreferenceOnly2Branch()
        {
            var medway = new Branch() { Name = "Medway", Id = 3 };
            var birtley = new Branch() { Name = "Birtley", Id = 1 };

            var model = new CleanPreferenceModel { Days = 24 };
            model.Branches.Add(medway);
            model.Branches.Add(birtley);

            var cleanPreference = new CleanPreference();
            cleanPreference.Branches.Add(medway);
            cleanPreference.Branches.Add(birtley);

            var cleanPreferences = new List<CleanPreference> { cleanPreference };

            this.repository.Setup(x => x.GetAll()).Returns(cleanPreferences);

            Assert.IsFalse(this.validator.IsValid(model, false));

            Assert.That(this.validator.Errors.Count, Is.EqualTo(1));

            Assert.That(this.validator.Errors[0], Is.EqualTo("Branch already has a clean preference assigned!"));
        }

        [Test]
        public void BranchAttachedToOnePreferenceOnly3BranchValidAndInvalid()
        {
            var medway = new Branch() { Name = "Medway", Id = 3 };
            var birtley = new Branch() { Name = "Birtley", Id = 1 };
            var brighton = new Branch() { Name = "Brighton", Id = 11 };

            var model = new CleanPreferenceModel { Days = 24 };
            model.Branches.Add(medway);
            model.Branches.Add(birtley);
            model.Branches.Add(brighton);

            var cleanPreference = new CleanPreference();
            cleanPreference.Branches.Add(medway);
            cleanPreference.Branches.Add(birtley);

            var cleanPreferences = new List<CleanPreference> { cleanPreference };

            this.repository.Setup(x => x.GetAll()).Returns(cleanPreferences);

            Assert.IsFalse(this.validator.IsValid(model, false));

            Assert.That(this.validator.Errors.Count, Is.EqualTo(1));

            Assert.That(this.validator.Errors[0], Is.EqualTo("Branch already has a clean preference assigned!"));
        }

        [Test]
        public void BranchAttachedToOnePreferenceOnlyMultipleCleanPreferences()
        {
            var medway = new Branch() { Name = "Medway", Id = 3 };
            var birtley = new Branch() { Name = "Birtley", Id = 1 };
            var brighton = new Branch() { Name = "Brighton", Id = 11 };

            var model = new CleanPreferenceModel { Days = 24 };
            model.Branches.Add(medway);
            model.Branches.Add(birtley);
            model.Branches.Add(brighton);

            var cleanPreference = new CleanPreference();
            cleanPreference.Branches.Add(medway);
            cleanPreference.Branches.Add(birtley);

            var cleanPreference2 = new CleanPreference();
            cleanPreference2.Branches.Add(brighton);

            var cleanPreferences = new List<CleanPreference> { cleanPreference, cleanPreference2 };

            this.repository.Setup(x => x.GetAll()).Returns(cleanPreferences);

            Assert.IsFalse(this.validator.IsValid(model, false));

            Assert.That(this.validator.Errors.Count, Is.EqualTo(1));

            Assert.That(this.validator.Errors[0], Is.EqualTo("Branch already has a clean preference assigned!"));
        }

        [Test]
        public void BranchAttachedToOnePreferenceOnlyMultipleCleanPreferencesSwitched()
        {
            var medway = new Branch() { Name = "Medway", Id = 3 };
            var birtley = new Branch() { Name = "Birtley", Id = 1 };
            var brighton = new Branch() { Name = "Brighton", Id = 11 };

            var model = new CleanPreferenceModel { Days = 24 };
            model.Branches.Add(medway);
            model.Branches.Add(birtley);
            
            var cleanPreference = new CleanPreference();
            cleanPreference.Branches.Add(medway);
            cleanPreference.Branches.Add(brighton);

            var cleanPreference2 = new CleanPreference();
            cleanPreference2.Branches.Add(brighton);
            cleanPreference.Branches.Add(birtley);

            var cleanPreferences = new List<CleanPreference> { cleanPreference, cleanPreference2 };

            this.repository.Setup(x => x.GetAll()).Returns(cleanPreferences);

            Assert.IsFalse(this.validator.IsValid(model, false));

            Assert.That(this.validator.Errors.Count, Is.EqualTo(1));

            Assert.That(this.validator.Errors[0], Is.EqualTo("Branch already has a clean preference assigned!"));
        }

        [Test]
        public void BranchAttachedToOnePreferenceOnlyValid()
        {
            var medway = new Branch() { Name = "Medway", Id = 3 };
            var birtley = new Branch() { Name = "Birtley", Id = 1 };
            var brighton = new Branch() { Name = "Brighton", Id = 11 };

            var model = new CleanPreferenceModel { Days = 24 };
            model.Branches.Add(medway);
            model.Branches.Add(birtley);

            var cleanPreference = new CleanPreference();
            cleanPreference.Branches.Add(brighton);

            var cleanPreference2 = new CleanPreference();
            cleanPreference2.Branches.Add(brighton);

            var cleanPreferences = new List<CleanPreference> { cleanPreference, cleanPreference2 };

            this.repository.Setup(x => x.GetAll()).Returns(cleanPreferences);

            Assert.IsTrue(this.validator.IsValid(model, false));

            Assert.That(this.validator.Errors.Count, Is.EqualTo(0));
        }

        private void ClearErrors()
        {
            this.validator.Errors = new List<string>();
        }
    }
}