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
        public void When_asked_to_asexual_reproduce_should_create_child()
        {
            // Arrange
            var organism = new Plant();

            // Act
            Environment.Instance.Reproduce(organism);

            // Assert
            var child = Environment.Instance.Organisms.OfType<Plant>().FirstOrDefault();
            Assert.IsNotNull(child);
        }

        [Test]
        public void When_asked_to_asexual_reproduce_should_reduce_parent_health()
        {
            // Arrange
            const int InitialHealth = 10;

            var organism = new Plant { Health = InitialHealth };

            // Act
            Environment.Instance.Reproduce(organism);

            // Assert
            var child = Environment.Instance.Organisms.OfType<Plant>().First();

            Assert.AreEqual(InitialHealth - child.Health, organism.Health);
        }

        [Test]
        public void When_asexual_reproducing_should_create_child_adjacent_to_parent()
        {
            // Arrange
            var parent = new Plant();
            Environment.Instance.Seed(new[] { parent });

            // Act
            Environment.Instance.Reproduce(parent);

            // Assert
            var child = Environment.Instance.Organisms.First(o => o != parent);
            var distance = parent.Position.Distance(child.Position);

            Assert.LessOrEqual(distance, parent.Health);
        }

        [Test]
        public void When_seeding_should_add_specified_organisms()
        {
            // Arrange
            var toAdd = new[]
                {
                    new Herbivore(),
                    new Herbivore(),
                    new Herbivore()
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
                organisms.Add(new Herbivore());
            }

            Environment.Instance.Seed(organisms);

            var observer = new Carnivore { RangeOfAwareness = 20, Position = { X = 400, Y = 300 } };

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

            var mover = new Herbivore
                {
                    Position = { X = initialPosition.X, Y = initialPosition.Y },
                    Speed = 5,
                    Direction = 180
                };

            // Act
            Environment.Instance.Move(mover, mover.Speed);
            var firstPosition = new Position { X = mover.Position.X, Y = mover.Position.Y };
            mover.Direction = 0;
            Environment.Instance.Move(mover, mover.Speed);
            var secondPosition = new Position { X = mover.Position.X, Y = mover.Position.Y };

            // Assert
            Assert.AreEqual(firstPosition.Distance(initialPosition), mover.Speed);
            Assert.AreEqual(firstPosition.Distance(secondPosition), mover.Speed);
        }

        [Test]
        public void When_eating_should_increase_health_of_preditor()
        {
            // Arrange
            const int InitialHealth = 20;

            var preditor = new Herbivore { Health = InitialHealth };

            var prey = new Herbivore();

            // Act
            Environment.Instance.Eat(preditor, prey);

            // Assert
            Assert.Greater(preditor.Health, InitialHealth);
        }

        [Test]
        public void When_eating_should_deduct_health_from_prey()
        {
            // Arrange
            const int InitialHealth = 20;

            var preditor = new Carnivore();

            var prey = new Herbivore { Health = InitialHealth };

            // Act
            Environment.Instance.Eat(preditor, prey);

            // Assert
            Assert.Less(prey.Health, InitialHealth);
        }
    }
}
