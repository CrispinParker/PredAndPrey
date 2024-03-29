namespace PredAndPrey.Core.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class CarnivoreC : Carnivore
    {
        protected override Animal CreateInstance()
        {
            var child = new CarnivoreC();

            return child;
        }

        protected override IEnumerable<Animal> SelectMates(IEnumerable<Organism> organisms)
        {
            return organisms.OfType<CarnivoreC>();
        }
    }
}