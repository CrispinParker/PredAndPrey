namespace PredAndPrey.Core.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public abstract class Carnivore : Animal
    {
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