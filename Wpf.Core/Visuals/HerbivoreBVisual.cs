namespace PredAndPrey.Wpf.Core.Visuals
{
    using System.Windows.Media;

    using PredAndPrey.Core.Models;

    public class HerbivoreBVisual : AnimalVisual
    {
        private static readonly Geometry[] Geometrys = new[]
                {
                    Geometry.Parse("M54.565006,44.497998 C96.958691,100.583 56.450366,29.833334 56.450366,29.833334 59.239801,8.1971567 47.886538,0.50000053 37.116925,0.50000053 26.347312,0.50000053 15.608663,8.6107097 17.616815,30.333334 17.616815,30.333334 -27.518609,107.833 19.52068,45.458001 19.52068,45.458001 28.759452,132.5 37.116925,132.5 45.624884,132.5 54.565006,44.497998 54.565006,44.497998 z"),
                    Geometry.Parse("M54.565006,44.497998 C100.53367,-19.666999 56.450366,29.833334 56.450366,29.833334 59.239801,8.1971567 47.886538,0.50000053 37.116925,0.50000053 26.347312,0.50000053 15.608663,8.6107097 17.616815,30.333334 17.616815,30.333334 -26.092141,-18.166999 19.52068,45.458001 19.52068,45.458001 28.759452,132.25 37.116925,132.25 45.624884,132.25 54.565006,44.497998 54.565006,44.497998 z")
                };

        public HerbivoreBVisual(Animal animal)
            : base(animal)
        {
        }

        protected override double Scale
        {
            get
            {
                return 0.3d;
            }
        }

        protected override Geometry[] GeometryData
        {
            get
            {
                return Geometrys;
            }
        }
    }
}