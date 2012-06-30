namespace PredAndPrey.Tests
{
    using NUnit.Framework;

    using PredAndPrey.Core.Models;

    using Rhino.Mocks;

    [TestFixture]
    public class PlantBehaviourTests : OrganismBehaviourTestBase
    {
        [Test]
        public void When_asked_to_behave_should_grow()
        {
            // Arrange
            var plant = new Plant();
            var initialSize = plant.Size;

            // Act
            plant.Behave(this.Environment);

            // Assert
            Assert.Greater(plant.Size, initialSize);
        }

        [Test]
        public void When_size_exceeds_reproduction_size_should_reproduce()
        {
            // Arrange
            var plant = new Plant { Size = Plant.ReproductiveSize + 1 };

            // Act
            plant.Behave(this.Environment);

            // Assert
            this.Environment.AssertWasCalled(e => e.Reproduce(plant), options => options.Repeat.AtLeastOnce());
        }
    }
}
