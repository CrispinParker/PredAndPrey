namespace PredAndPrey.Tests
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using PredAndPrey.Core.Models;

    using Rhino.Mocks;

    [TestFixture]
    public class PreditorBehaviourTests : OrganismBehaviourTestBase
    {
        [Test]
        public void When_size_exceeds_reproduction_size_should_reproduce()
        {
            // Arrange
            var preditor = new Preditor { Size = Preditor.ReproductiveSize + 1 };

            // Act
            preditor.Behave(this.Environment);

            // Assert
            this.Environment.AssertWasCalled(e => e.Reproduce(preditor), options => options.Repeat.AtLeastOnce());
        }

        [Test]
        public void When_prey_visible_should_move_towards_nearest_prey()
        {
            // Arrange
            var prey = new List<Prey>
                {
                    new Prey { Position = new Position { X = 0, Y = 0 } },
                    new Prey { Position = new Position { X = 30, Y = 30 } }
                };

            var preditor = new Preditor
                {
                    Speed = 5,
                    Position = new Position { X = 20, Y = 20 }
                };

            this.Environment.Expect(e => e.Look(preditor)).Return(prey).Repeat.Once();

            // Act
            preditor.Behave(this.Environment);

            // Assert
            this.Environment.VerifyAllExpectations();
            Assert.AreEqual(45, preditor.Direction);
        }
    }
}
