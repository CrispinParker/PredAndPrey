namespace PredAndPrey.Core.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class HerbivoreA : Herbivore
    {
        private const double DefaultInitialSize = 200;

        public override double InitialSpeed
        {
            get
            {
                return SettingsHelper.Instance.HerbivoreInitialSpeed;
            }
        }

        public override double InitialSize
        {
            get
            {
                return DefaultInitialSize;
            }
        }

        public override double InitialSight
        {
            get
            {
                return SettingsHelper.Instance.HerbivoreInitialSight;
            }
        }

        protected override Animal CreateInstance()
        {
            var child = new HerbivoreA();

            return child;
        }

        protected override IEnumerable<Animal> SelectMates(IEnumerable<Organism> visibleOrganisms)
        {
            return visibleOrganisms.OfType<HerbivoreA>();
        }
    }
}