namespace PredAndPrey.Core.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class Herbivore : Animal
    {
        public const double InitialSize = 200;

        public const double InitialSpeed = 2.5;

        public const double InitialSight = 40;
        
        private Herbivore()
        {
        }

        public static Herbivore GenerateDefault()
        {
            var output = new Herbivore
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
            var child = new Herbivore();

            return child;
        }

        protected override IEnumerable<Animal> SelectMates(IEnumerable<Organism> visibleOrganisms)
        {
            return visibleOrganisms.OfType<Herbivore>();
        }

        protected override IEnumerable<Organism> SelectPrey(IEnumerable<Organism> visibleOrganisms)
        {
            return visibleOrganisms.OfType<Plant>();
        }

        protected override IEnumerable<Animal> SelectPredators(IEnumerable<Organism> visibleOrganisms)
        {
            return visibleOrganisms.OfType<Carnivore>();
        }
    }
}