namespace PredAndPrey.Tests
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using NUnit.Framework;

    using PredAndPrey.Core.Models;

    [TestFixture]
    public class EnvironmentTests
    {
        [SetUp]
        public void SetUp()
        {
            Environment.Instance = null;
        }

        [Test]
        public void When_asked_to_reproduce_should_create_child()
        {
            // Arrange
            var organism = new TestOrganism();

            // Act
            Environment.Instance.Reproduce(organism);

            // Assert
            Assert.IsTrue(Environment.Instance.Organisms.OfType<TestOrganism>().Any());
            Assert.IsTrue(organism.HasReproduced);
        }

        [Test]
        public void When_asked_to_reproduce_should_reduce_parent_size()
        {
            // Arrange
            const int InitialSize = 10;

            var organism = new TestOrganism { Size = InitialSize };

            // Act
            Environment.Instance.Reproduce(organism);

            // Assert
            var child = Environment.Instance.Organisms.OfType<TestOrganism>().First();

            Assert.AreEqual(InitialSize - child.Size, organism.Size);
        }

        [Test]
        public void When_reproducing_should_create_child_adjacent_to_parent()
        {
            // Arrange
            var parent = new TestOrganism();
            Environment.Instance.Seed(new[] { parent });

            // Act
            Environment.Instance.Reproduce(parent);

            // Assert
            var child = Environment.Instance.Organisms.First(o => o != parent);
            var distance = parent.Position.Distance(child.Position);

            Assert.LessOrEqual(distance, parent.Size);
        }

        [Test]
        public void When_seeding_should_add_specified_organisms()
        {
            // Arrange
            var toAdd = new[]
                {
                    new TestOrganism(),
                    new TestOrganism(),
                    new TestOrganism()
                };

            // Act
            Environment.Instance.Seed(toAdd);

            // Assert
            Assert.AreEqual(toAdd.Count(), Environment.Instance.Organisms.Count());
            Assert.IsTrue(Environment.Instance.Organisms.All(toAdd.Contains));
        }

        [Test]
        public void When_looking_should_return_organisms_in_range_only()
        {
            // Arrange
            var organisms = new List<Organism>();

            for (int i = 0; i < 1000; i++)
            {
                organisms.Add(new TestOrganism());
            }

            Environment.Instance.Seed(organisms);

            var observer = new Preditor { RangeOfAwareness = 20, Position = new Position { X = 400, Y = 300 } };

            // Act
            var visibleOrganisms = Environment.Instance.Look(observer);

            // Assert
            foreach (var visibleOrganism in visibleOrganisms)
            {
                var distance = observer.Position.Distance(visibleOrganism.Position);

                Debug.WriteLine(distance);

                Assert.LessOrEqual(distance, observer.RangeOfAwareness);
            }
        }

        [Test]
        public void When_moving_should_reposition_animal_to_expected_location()
        {
            // Arrange
            var initialPosition = new Position { X = 400, Y = 300 };

            var mover = new TestOrganism
                {
                    Position = initialPosition,
                    Speed = 5,
                    Direction = 180
                };

            // Act
            Environment.Instance.Move(mover);
            var firstPosition = new Position { X = mover.Position.X, Y = mover.Position.Y };
            mover.Direction = 0;
            Environment.Instance.Move(mover);
            var secondPosition = new Position { X = mover.Position.X, Y = mover.Position.Y };

            // Assert
            Assert.AreEqual(firstPosition.Distance(initialPosition), mover.Speed);
            Assert.AreEqual(firstPosition.Distance(secondPosition), mover.Speed);
        }

        [Test]
        public void When_eating_should_deduct_and_increase_health_acording_to_size()
        {
            // Arrange
            var preditor = new TestOrganism { Size = 10, Health = 10 };

            var prey = new TestOrganism { Health = 20 };

            // Act
            Environment.Instance.Eat(preditor, prey);

            // Assert
            Assert.AreEqual(20, preditor.Health);
            Assert.AreEqual(10, prey.Health);
        }

        private class TestOrganism : Animal
        {
            public bool HasReproduced { get; private set; }

            public override Organism Reproduce()
            {
                this.HasReproduced = true;

                return new TestOrganism { Size = this.Size / 2 };
            }

            public override void Behave(IEnvironment environment)
            {
            }
        }
    }
}
