namespace PH.Well.UnitTests.Api.Validators
{
    using System;

    using NUnit.Framework;

    using PH.Well.Api.Models;
    using PH.Well.Api.Validators;
    using PH.Well.Domain;

    [TestFixture]
    public class SeasonalDateValidatorTests
    {
        // description required
        // description length check
        // valid dates
        // from not after to
        // branch required

        private SeasonalDateValidator validator;

        [SetUp]
        public void Setup()
        {
            this.validator = new SeasonalDateValidator();
        }

        [Test]
        public void DescriptionRequired()
        {
            var model = new SeasonalDateModel { Description = null, FromDate = "01/01/2000", ToDate = "01/01/2001" };
            model.Branches.Add(new Branch());

            Assert.IsFalse(this.validator.IsValid(model));

            Assert.That(this.validator.Errors.Count, Is.EqualTo(1));

            Assert.That(this.validator.Errors[0], Is.EqualTo("Description is required!"));
        }

        [Test]
        public void DescriptionRange()
        {
            var model = new SeasonalDateModel { Description = new string('a', 256), FromDate = "01/01/2000", ToDate = "01/01/2001" };
            model.Branches.Add(new Branch());

            Assert.IsFalse(this.validator.IsValid(model));

            Assert.That(this.validator.Errors.Count, Is.EqualTo(1));

            Assert.That(this.validator.Errors[0], Is.EqualTo("Description is over the max capacity of 255 characters!"));
        }

        [Test]
        public void FromDateRequired()
        {
            var model = new SeasonalDateModel { Description = "foo", FromDate = "", ToDate = "01/01/2001" };
            model.Branches.Add(new Branch());

            Assert.IsFalse(this.validator.IsValid(model));

            Assert.That(this.validator.Errors.Count, Is.EqualTo(1));

            Assert.That(this.validator.Errors[0], Is.EqualTo("From date is required!"));
        }

        [Test]
        public void FromDateInvalid()
        {
            var model = new SeasonalDateModel { Description = "foo", FromDate = "foo", ToDate = "01/01/2001" };
            model.Branches.Add(new Branch());

            Assert.IsFalse(this.validator.IsValid(model));

            Assert.That(this.validator.Errors.Count, Is.EqualTo(1));

            Assert.That(this.validator.Errors[0], Is.EqualTo("From date is not a valid date!"));
        }

        [Test]
        public void ToDateRequired()
        {
            var model = new SeasonalDateModel { Description = "foo", FromDate = "01/01/2001", ToDate = "" };
            model.Branches.Add(new Branch());

            Assert.IsFalse(this.validator.IsValid(model));

            Assert.That(this.validator.Errors.Count, Is.EqualTo(1));

            Assert.That(this.validator.Errors[0], Is.EqualTo("To date is required!"));
        }

        [Test]
        public void ToDateInvalid()
        {
            var model = new SeasonalDateModel { Description = "foo", FromDate = "01/01/2001", ToDate = "foo" };
            model.Branches.Add(new Branch());

            Assert.IsFalse(this.validator.IsValid(model));

            Assert.That(this.validator.Errors.Count, Is.EqualTo(1));

            Assert.That(this.validator.Errors[0], Is.EqualTo("To date is not a valid date!"));
        }

        [Test]
        public void FromDatePastToDate()
        {
            var model = new SeasonalDateModel { Description = "foo", FromDate = "01/01/2001", ToDate = "01/01/2000" };
            model.Branches.Add(new Branch());

            Assert.IsFalse(this.validator.IsValid(model));

            Assert.That(this.validator.Errors.Count, Is.EqualTo(1));

            Assert.That(this.validator.Errors[0], Is.EqualTo("From date can not be greater than to date!"));
        }

        [Test]
        public void BranchRequired()
        {
            var model = new SeasonalDateModel { Description = "foo", FromDate = "01/01/2001", ToDate = "01/01/2002" };

            Assert.IsFalse(this.validator.IsValid(model));

            Assert.That(this.validator.Errors.Count, Is.EqualTo(1));

            Assert.That(this.validator.Errors[0], Is.EqualTo("Select a branch!"));
        }
    }
}