namespace PredAndPrey.Core.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class HerbivoreB : Herbivore
    {
        protected override Animal CreateInstance()
        {
            var child = new HerbivoreB();

            return child;
        }

        protected override IEnumerable<Animal> SelectMates(IEnumerable<Organism> organisms)
        {
            return organisms.OfType<HerbivoreB>();
        }
    }
}