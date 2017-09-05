namespace PH.Well.UnitTests.Common
{
    using System;
    using System.Data.SqlClient;
    using System.Runtime.Serialization;
    using Factories;
    using Moq;
    using NUnit.Framework;
    using Well.Common;
    using Well.Common.Contracts;
    using Well.Services;
    using Well.Services.EpodServices;

    [TestFixture]
    public class DeadlockRetryHelperTests
    {
        public interface IAction
        {
            void Execute();
        }

        private Mock<ILogger> logger;
        private Mock<IDeadlockRetryConfig> config;
        private Mock<IAction> myAction;

        private IDeadlockRetryHelper deadlockRetryHelper;

        [SetUp]
        public void SetUp()
        {
            logger = new Mock<ILogger>();
            config = new Mock<IDeadlockRetryConfig>();
            myAction = new Mock<IAction>();

            deadlockRetryHelper = new DeadlockRetryHelper(logger.Object, config.Object);
        }

        [Test]
        public void ShouldCallMethodOnceifMaxNoOfRetriesZero()
        {
            config.Setup(x => x.MaxNoOfDeadlockRetires).Returns(0);
            myAction.Setup(x => x.Execute());

            deadlockRetryHelper.Retry(() => myAction.Object.Execute());

            myAction.Verify(x => x.Execute(), Times.Once);
        }

        [Test]
        public void ShouldRetryIfThrowsSqlException1205()
        {
            config.Setup(x => x.MaxNoOfDeadlockRetires).Returns(3);
            myAction.Setup(x => x.Execute()).Throws(SqlExceptionCreator.CreateSqlException(DeadlockRetryHelper.SqlDeadlockErrorNumber));

            try
            {
                deadlockRetryHelper.Retry(() => myAction.Object.Execute());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            myAction.Verify(x => x.Execute(), Times.Exactly(4));
        }

        [Test]
        public void ShouldThrowErrorAfterLastRetry()
        {
            config.Setup(x => x.MaxNoOfDeadlockRetires).Returns(2);

            myAction.Setup(x => x.Execute()).Callback(() => 
                {
                    throw SqlExceptionCreator.CreateSqlException(DeadlockRetryHelper.SqlDeadlockErrorNumber);
                });

            Assert.Throws<SqlException>( ()=> deadlockRetryHelper.Retry(() => myAction.Object.Execute()));
            myAction.Verify(x => x.Execute(), Times.Exactly(3));
        }

        [Test]
        public void ShouldRetryIfErrorThenReturnWhenNoError()
        {
            config.Setup(x => x.MaxNoOfDeadlockRetires).Returns(3);
            var calls = 0;

            myAction.Setup(x => x.Execute()).Callback(() =>
                {
                    calls++;
                    if (calls == 1)
                    {
                        throw SqlExceptionCreator.CreateSqlException(DeadlockRetryHelper.SqlDeadlockErrorNumber);
                    }
                }
            );

            try
            {
                deadlockRetryHelper.Retry(() => myAction.Object.Execute());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            myAction.Verify(x => x.Execute(), Times.Exactly(2));
        }
    }
}