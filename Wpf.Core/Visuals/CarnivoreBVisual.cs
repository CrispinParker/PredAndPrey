namespace PredAndPrey.Wpf.Core.Visuals
{
    using System.Windows.Media;

    using PredAndPrey.Core.Models;

    public class CarnivoreBVisual : AnimalVisual
    {
        private static readonly Geometry[] Geometrys = new[]
            {
                    Geometry.Parse("M42.25,56.458 L36.625,52.458 36.25,81.958 32,80.208 26.375,118.083 20.875,80.208 16.375,82.333 13.75,52.333 8.25,56.083 C8.25,56.083 8.125,34.75 8.125,34.75 8.125,34.75 -0.75,34.75 -0.75,34.75 -0.75,21.219024 11.469024,0.50000018 25,0.50000018 38.530976,0.50000018 51,21.219024 51,34.75 51,34.75 42.125,34.75 42.125,34.75 z"),
                    Geometry.Parse("M42.25,56.458 L36.625,52.458 36.25,81.958 32,80.208 26.375,118.083 20.875,80.208 16.375,82.333 13.75,52.333 8.25,56.083 C8.25,56.083 8.125,46.75 8.125,46.75 8.125,46.75 -0.75,55.75 -0.75,55.75 -0.75,42.219024 11.469024,0.50000018 25,0.50000018 38.530976,0.50000018 51,42.219024 51,55.75 51,55.75 42.125,46.75 42.125,46.75 z")
            };

        public CarnivoreBVisual(Animal animal)
            : base(animal)
        {
        }

        protected override double Scale
        {
            get
            {
                return 0.35d;
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