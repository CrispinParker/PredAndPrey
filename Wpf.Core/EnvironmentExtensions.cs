namespace PredAndPrey.Wpf.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Media;

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
            var organisms = new List<Organism>();

            organisms.AddRange(CreateInstances<HerbivoreA>(10, "#FF8AFF72"));
            organisms.AddRange(CreateInstances<HerbivoreB>(10, "#FF02FFE2"));
            organisms.AddRange(CreateInstances<HerbivoreC>(10, "#FFFFA1F7"));

            Environment.Instance.Seed(organisms);
        }

        private static void SeedCarnivors()
        {
            var organisms = new List<Organism>();

            organisms.AddRange(CreateInstances<CarnivoreA>(10, "#FFFF0B00"));
            organisms.AddRange(CreateInstances<CarnivoreB>(10, "#FFFFA402"));
            organisms.AddRange(CreateInstances<CarnivoreC>(10, "#FFFDFF02"));

            Environment.Instance.Seed(organisms);
        }

        private static IEnumerable<Animal> CreateInstances<TAnimal>(int volume, string colourAsString) where TAnimal : Animal, new()
        {
            var fromString = ColorConverter.ConvertFromString(colourAsString);

            if (fromString == null)
            {
                return Enumerable.Empty<TAnimal>();
            }

            var color = (Color)fromString;

            var organisms = new List<Animal>();

            for (int i = 0; i < volume; i++)
            {
                var herbivore = Environment.Instance.GenerateDefault<TAnimal>();

                herbivore.Features.Add("Red", color.R);
                herbivore.Features.Add("Green", color.G);
                herbivore.Features.Add("Blue", color.B);

                organisms.Add(herbivore);
            }

            return organisms.AsEnumerable();
        }
    }
}
