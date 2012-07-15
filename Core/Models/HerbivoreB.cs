namespace PredAndPrey.Core.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class HerbivoreB : Herbivore
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
            var child = new HerbivoreB();

            return child;
        }

        protected override IEnumerable<Animal> SelectMates(IEnumerable<Organism> visibleOrganisms)
        {
            return visibleOrganisms.OfType<HerbivoreB>();
        }
    }
}