namespace PredAndPrey.Core.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class HerbivoreA : Herbivore
    {
        private const double DefaultInitialSize = 175;

        private const double DefaultInitialSpeed = 2.5;

        private const double DefaultInitialSight = 40;

        public override double InitialSpeed
        {
            get
            {
                return DefaultInitialSpeed;
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
                return DefaultInitialSight;
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