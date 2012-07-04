namespace PredAndPrey.Core.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class Herbivore : Animal
    {
        private Herbivore()
        {
        }

        public static Herbivore GenerateDefault()
        {
            var output = new Herbivore
                {
                    Size = 200,
                    Health = 200 * 0.75,
                    Speed = 2.5d,
                    Sight = 40,
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

        protected override IEnumerable<Animal> SelectPreditors(IEnumerable<Organism> visibleOrganisms)
        {
            return visibleOrganisms.OfType<Carnivore>();
        }
    }
}