namespace PredAndPrey.Wpf.Core.Visuals
{
    using System.Windows.Media;

    using PredAndPrey.Core.Models;

    public abstract class OrganismVisual
    {
        protected OrganismVisual(Brush fill, Pen pen)
        {
            this.Fill = fill;
            this.Pen = pen;
        }

        public Brush Fill { get; private set; }

        public Pen Pen { get; private set; }

        public abstract Geometry Geometry { get; }

        public abstract void UpdateGeometry(Organism orgamism);
    }
}