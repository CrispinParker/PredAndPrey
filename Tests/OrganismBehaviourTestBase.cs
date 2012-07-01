namespace PredAndPrey.Tests
{
    using NUnit.Framework;

    using PredAndPrey.Core.Models;

    using Rhino.Mocks;

    public abstract class OrganismBehaviourTestBase
    {
        protected IEnvironment Environment { get; private set; }

        [SetUp]
        public virtual void SetUp()
        {
            this.Environment = MockRepository.GenerateMock<IEnvironment>();
        }
    }
}