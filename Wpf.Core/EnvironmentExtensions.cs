namespace PredAndPrey.Wpf.Core
{
    using System;
    using System.Collections.Generic;

    using PredAndPrey.Core.Models;

    using Environment = PredAndPrey.Core.Environment;

    public static class EnvironmentExtensions
    {
        public static void Reset(this Environment environment)
        {
            Environment.Instance = null;
            SeedHerbivors();
            SeedCarnivors();
        }

        private static void SeedHerbivors()
        {
            var rnd = new Random();

            var organisms = new List<Organism>();

            var red = 255 * rnd.NextDouble();
            var green = 255 * rnd.NextDouble();
            var blue = 255d;

            for (int i = 0; i < 15; i++)
            {
                var herbivore = Environment.Instance.GenerateDefault<HerbivoreA>();

                herbivore.Features.Add("Red", red);
                herbivore.Features.Add("Green", green);
                herbivore.Features.Add("Blue", blue);

                organisms.Add(herbivore);
            }

            red = 126 * rnd.NextDouble();
            green = 255;
            blue = (126 * rnd.NextDouble()) + 126;

            for (int i = 0; i < 15; i++)
            {
                var herbivore = Environment.Instance.GenerateDefault<HerbivoreB>();

                herbivore.Features.Add("Red", red);
                herbivore.Features.Add("Green", green);
                herbivore.Features.Add("Blue", blue);

                organisms.Add(herbivore);
            }

            Environment.Instance.Seed(organisms);
        }

        private static void SeedCarnivors()
        {
            var rnd = new Random();

            var organisms = new List<Organism>();

            const double Red = 255d;
            var green = 255 * rnd.NextDouble();
            var blue = 126 * rnd.NextDouble();

            var grey = (126 * rnd.NextDouble()) + 126;

            for (int i = 0; i < 7; i++)
            {
                var carnivore = Environment.Instance.GenerateDefault<CarnivoreA>();

                carnivore.Features.Add("Red", Red);
                carnivore.Features.Add("Green", grey);
                carnivore.Features.Add("Blue", grey);

                organisms.Add(carnivore);
            }

            for (int i = 0; i < 7; i++)
            {
                var carnivore = Environment.Instance.GenerateDefault<CarnivoreB>();

                carnivore.Features.Add("Red", Red);
                carnivore.Features.Add("Green", green);
                carnivore.Features.Add("Blue", blue);

                organisms.Add(carnivore);
            }

            Environment.Instance.Seed(organisms);
        }
    }
}
