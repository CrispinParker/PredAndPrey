namespace PredAndPrey.Tests
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using NUnit.Framework;

    using PredAndPrey.Core.Models;

    [TestFixture]
    public class WorldTests
    {
        [Test]
        public void When_time_passes_plants_should_grow_and_reproduce()
        {
            // Arrange
            Environment.Instance.Seed(new[] { new Plant() });

            // Act
            for (int i = 0; i < 100; i++)
            {
                Environment.Instance.PassTime();
            }

            // Assert
            Assert.Greater(Environment.Instance.Organisms.OfType<Plant>().Count(), 1);
        }

        [Test]
        public void When_time_passes_herbivours_should_eat_plants()
        {
            // Arrange
            var seeds = new List<Organism>();

            var plant = new Plant();
            seeds.Add(plant);

            var herbivore = new Herbivore
                {
                    RangeOfAwareness = 200, 
                    ReproductiveHealth = 30, 
                    Speed = 10
                };

            seeds.Add(herbivore);

            Environment.Instance.Seed(seeds);

            herbivore.Position.X = plant.Position.X + 10;
            herbivore.Position.Y = plant.Position.Y + 10;

            // Act
            for (int i = 0; i < 5; i++)
            {
                Environment.Instance.PassTime();
                Debug.WriteLine("Health: " + herbivore.Health);
                Debug.WriteLine("Position: " + herbivore.Position.X + ", " + herbivore.Position.Y);
            }

            // Assert
            Assert.IsTrue(Environment.Instance.Organisms.OfType<Herbivore>().Any());
            Assert.IsFalse(Environment.Instance.Organisms.OfType<Plant>().Any());
        }
    }
}
