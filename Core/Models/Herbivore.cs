namespace PredAndPrey.Core.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public abstract class Herbivore : Animal
    {
        public override double InitialSpeed
        {
            get
            {
                return SettingsHelper.Instance.HerbivoreInitialSpeed;
            }
        }

        public override double InitialSight
        {
            get
            {
                return SettingsHelper.Instance.HerbivoreInitialSight;
            }
        }

        protected override IEnumerable<Organism> SelectPrey(IEnumerable<Organism> organisms)
        {
            return organisms.OfType<Plant>();
        }

        protected override IEnumerable<Animal> SelectPredators(IEnumerable<Organism> organisms)
        {
            var enumerable = organisms.ToArray();

            return enumerable.OfType<Carnivore>();
        }
    }
}