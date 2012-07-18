namespace PredAndPrey.Core.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class HerbivoreA : Herbivore
    {
        protected override Animal CreateInstance()
        {
            var child = new HerbivoreA();

            return child;
        }

        protected override IEnumerable<Animal> SelectMates(IEnumerable<Organism> organisms)
        {
            return organisms.OfType<HerbivoreA>();
        }
    }
}