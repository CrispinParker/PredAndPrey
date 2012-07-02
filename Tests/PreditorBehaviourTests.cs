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
        public void When_is_healthly_should_seek_mate()
        {
            // Arrange
            var preditor = new Carnivore { Health = 1000, RangeOfAwareness = 10 };
            var mate = new Carnivore { Health = 1000, Position = { X = 4, Y = 4 } };

            this.Environment.Expect(e => e.Look(null)).IgnoreArguments().Return(new[] { mate });

            // Act
            preditor.Behave(this.Environment);

            // Assert
            Assert.Greater(preditor.Direction, 0);
        }

        [Test]
        public void When_hungry_and_prey_visible_should_move_towards_nearest_prey()
        {
            // Arrange
            var prey = new List<Herbivore>
                {
                    new Herbivore { Position = { X = 0, Y = 0 } },
                    new Herbivore { Position = { X = 30, Y = 30 } }
                };

            var preditor = new Carnivore
                {
                    Speed = 5,
                    Position = { X = 20, Y = 20 }
                };

            this.Environment.Expect(e => e.Look(preditor)).Return(prey).Repeat.Once();

            // Act
            preditor.Behave(this.Environment);

            // Assert
            this.Environment.VerifyAllExpectations();
            Assert.AreNotEqual(0, preditor.Direction);
        }
    }
}
