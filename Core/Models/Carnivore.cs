namespace PredAndPrey.Core.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public abstract class Carnivore : Animal
    {
        public override double InitialSpeed
        {
            get
            {
                return SettingsHelper.Instance.CarnivoreInitialSpeed;
            }
        }

        public override double InitialSight
        {
            get
            {
                return SettingsHelper.Instance.CarnivoreInitialSight;
            }
        }

        protected override IEnumerable<Organism> SelectPrey(IEnumerable<Organism> organisms)
        {
            return organisms.OfType<Herbivore>();
        }

        protected override IEnumerable<Animal> SelectPredators(IEnumerable<Organism> organisms)
        {
            return Enumerable.Empty<Animal>();
        }
    }
}