namespace PredAndPrey.Core.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public abstract class Herbivore : Animal
    {
        protected override IEnumerable<Organism> SelectPrey(IEnumerable<Organism> visibleOrganisms)
        {
            return visibleOrganisms.OfType<Plant>();
        }

        protected override IEnumerable<Animal> SelectPredators(IEnumerable<Organism> visibleOrganisms)
        {
            var enumerable = visibleOrganisms.ToArray();

            return enumerable.OfType<Carnivore>();
        }
    }
}