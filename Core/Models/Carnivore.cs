namespace PredAndPrey.Core.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class Carnivore : Animal
    {
        public const double InitialSize = 200;

        public const double InitialSpeed = 3;

        public const double InitialSight = 45;

        private Carnivore()
        {
        }

        public static Animal GenerateDefault()
        {
            var output = new Carnivore
                {
                    Size = InitialSize,
                    Health = InitialSize * HungerPercentage,
                    Speed = InitialSpeed,
                    Sight = InitialSight,
                    Generation = 1
                };

            return output;
        }

        protected override Animal CreateChild()
        {
            var child = new Carnivore();

            return child;
        }

        protected override IEnumerable<Animal> SelectMates(IEnumerable<Organism> visibleOrganisms)
        {
            return visibleOrganisms.OfType<Carnivore>();
        }

        protected override IEnumerable<Organism> SelectPrey(IEnumerable<Organism> visibleOrganisms)
        {
            return visibleOrganisms.OfType<Herbivore>();
        }

        protected override IEnumerable<Animal> SelectPredators(IEnumerable<Organism> visibleOrganisms)
        {
            return Enumerable.Empty<Animal>();
        }
    }
}