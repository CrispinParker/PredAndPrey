namespace PredAndPrey.Core.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class CarnivoreB : Carnivore
    {
        private const double DefaultInitialSize = 200;

        private const double DefaultInitialSpeed = 3.5;

        private const double DefaultInitialSight = 55;

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
            var child = new CarnivoreB();

            return child;
        }

        protected override IEnumerable<Animal> SelectMates(IEnumerable<Organism> visibleOrganisms)
        {
            return visibleOrganisms.OfType<CarnivoreB>();
        }
    }
}