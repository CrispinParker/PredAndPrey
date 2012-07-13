namespace PredAndPrey.Test
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using NUnit.Framework;

    using PredAndPrey.Core;
    using PredAndPrey.Core.Models;

    [TestFixture]
    public class OrganismVisibilityPerformanceFixture
    {
        [Test]
        public void TestPerformance()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            var results = new List<long>();

            // Act
            for (int i = 0; i < 100; i++)
            {
                Environment.Instance = null;

                CreateAnimals<HerbivoreA>(1000);
                
                stopwatch.Start();

                Environment.Instance.PassTime();

                stopwatch.Stop();

                results.Add(stopwatch.ElapsedMilliseconds);

                stopwatch.Reset();
            }

            // Assert
            Debug.WriteLine(results.Average());
        }

        private static void CreateAnimals<TAnimal>(int numOfOrganisms) where TAnimal : Animal, new()
        {
            var environment = Environment.Instance;

            var output = new List<Animal>();

            for (int i = 0; i < numOfOrganisms; i++)
            {
                var animal = environment.GenerateDefault<TAnimal>();

                environment.Seed(new[] { animal });

                animal.Position = new Position(i, i);

                output.Add(animal);
            }
        }
    }
}
