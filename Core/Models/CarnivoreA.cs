namespace PredAndPrey.Core.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class CarnivoreA : Carnivore
    {
        private const double DefaultInitialSize = 200;

        private const double DefaultInitialSpeed = 5;

        private const double DefaultInitialSight = 50;

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
            var child = new CarnivoreA();

            return child;
        }

        protected override IEnumerable<Animal> SelectMates(IEnumerable<Organism> visibleOrganisms)
        {
            return visibleOrganisms.OfType<CarnivoreA>();
        }
    }
}