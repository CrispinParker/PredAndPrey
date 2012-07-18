namespace PredAndPrey.Core.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class CarnivoreB : Carnivore
    {
        protected override Animal CreateInstance()
        {
            var child = new CarnivoreB();

            return child;
        }

        protected override IEnumerable<Animal> SelectMates(IEnumerable<Organism> organisms)
        {
            return organisms.OfType<CarnivoreB>();
        }
    }
}