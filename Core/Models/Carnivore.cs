namespace PredAndPrey.Core.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class Carnivore : Animal
    {
        private Carnivore()
        {
        }

        public static Animal GenerateDefault()
        {
            var output = new Carnivore
                {
                    Size = 200,
                    Health = 200 * 0.75,
                    Speed = 3.5d,
                    Sight = 60,
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

        protected override IEnumerable<Animal> SelectPreditors(IEnumerable<Organism> visibleOrganisms)
        {
            return Enumerable.Empty<Animal>();
        }
    }
}