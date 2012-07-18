namespace PredAndPrey.Core.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class HerbivoreC : Herbivore
    {
        protected override Animal CreateInstance()
        {
            var child = new HerbivoreC();

            return child;
        }

        protected override IEnumerable<Animal> SelectMates(IEnumerable<Organism> organisms)
        {
            return organisms.OfType<HerbivoreC>();
        }
    }
}