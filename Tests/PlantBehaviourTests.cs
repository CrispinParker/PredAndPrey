namespace PredAndPrey.Tests
{
    using NUnit.Framework;

    using PredAndPrey.Core.Models;

    [TestFixture]
    public class PlantBehaviourTests : OrganismBehaviourTestBase
    {
        [Test]
        public void When_asked_to_behave_should_grow()
        {
            // Arrange
            var plant = new Plant();
            var initialHealth = plant.Health;

            // Act
            plant.Behave(this.Environment);

            // Assert
            Assert.Greater(plant.Health, initialHealth);
        }
    }
}
