namespace PredAndPrey.Wpf.Core.Visuals
{
    using System.Windows;
    using System.Windows.Media;

    using PredAndPrey.Core.Models;

    public class PlantVisual : OrganismVisual
    {
        private static readonly Brush PlantBrush = new RadialGradientBrush(Colors.DarkGreen, Color.FromArgb(0, 0, 255, 0));

        private readonly EllipseGeometry geometry;

        private EllipseGeometry frozenGeometry;

        public PlantVisual(Organism plant)
            : base(PlantBrush, null)
        {
            this.geometry = new EllipseGeometry { Center = new Point(plant.Position.X, plant.Position.Y) };
            this.DoUpdateGeometry(plant);
        }

        public override Geometry Geometry
        {
            get
            {
                return this.frozenGeometry;
            }
        }

        public override void UpdateGeometry(Organism orgamism)
        {
            this.DoUpdateGeometry(orgamism);
        }

        private void DoUpdateGeometry(Organism orgamism)
        {
            var radius = orgamism.Health / 4;

            this.geometry.RadiusX = radius;
            this.geometry.RadiusY = radius;

            this.frozenGeometry = this.geometry.Clone();
            this.frozenGeometry.Freeze();
        }
    }
}