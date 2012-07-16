namespace PredAndPrey.Wpf.Core.Visuals
{
    using System;
    using System.Linq;
    using System.Windows.Media;

    using PredAndPrey.Core.Models;

    public abstract class AnimalVisual : OrganismVisual
    {
        private static readonly Pen StaticPen = new Pen(Brushes.Black, 0.5);

        private readonly TransformGroup transformGroup = new TransformGroup();

        private readonly TranslateTransform translateTransform = new TranslateTransform();

        private readonly RotateTransform rotateTransform = new RotateTransform();

        private readonly ScaleTransform scaleTransform = new ScaleTransform();

        private bool isFirstUpdate = true;

        private double centerX;

        private double centerY;

        static AnimalVisual()
        {
            StaticPen.Freeze();
        }

        protected AnimalVisual(Animal animal)
            : base(BrushFromAnimal(animal), StaticPen)
        {
            this.initialiseTransformGroup();
        }

        public override Geometry Geometry
        {
            get
            {
                var output = this.GetRandomGeometry().Clone();
                output.Transform = this.transformGroup.Clone();
                output.Freeze();

                return output;
            }
        }

        protected abstract double Scale { get; }

        protected abstract Geometry[] GeometryData { get; }

        public override void UpdateGeometry(Organism orgamism)
        {
            var animal = (Animal)orgamism;

            if (this.isFirstUpdate)
            {
                this.isFirstUpdate = false;
                this.SetupScale(animal);
                this.SetupCentrePoint();
            }

            this.translateTransform.X = animal.Position.X - this.centerX;
            this.translateTransform.Y = animal.Position.Y - this.centerY;

            this.rotateTransform.Angle = animal.Direction - 270;
        }

        private static Brush BrushFromAnimal(Animal animal)
        {
            var red = Math.Ceiling(animal.Features["Red"]);
            var green = Math.Ceiling(animal.Features["Green"]);
            var blue = Math.Ceiling(animal.Features["Blue"]);

            red = red < 0 ? 0 : red > 255 ? 255 : red;
            green = green < 0 ? 0 : green > 255 ? 255 : green;
            blue = blue < 0 ? 0 : blue > 255 ? 255 : blue;

            var brush = new SolidColorBrush(Color.FromRgb((byte)red, (byte)green, (byte)blue));

            brush.Freeze();

            return brush;
        }

        private Geometry GetRandomGeometry()
        {
            var rnd = new Random();

            return this.GeometryData[rnd.Next(0, this.GeometryData.Count())];
        }

        private void SetupCentrePoint()
        {
            var geometry = this.GetRandomGeometry().Clone();
            geometry.Transform = this.scaleTransform;

            var bounds = geometry.Bounds;

            this.centerX = bounds.Width / 2;
            this.centerY = bounds.Height / 2;

            this.rotateTransform.CenterX = this.centerX;
            this.rotateTransform.CenterY = this.centerY * 1.2;

            this.scaleTransform.CenterX = this.centerX;
            this.scaleTransform.CenterY = this.centerY;
        }

        private void SetupScale(Animal animal)
        {
            var speedTransform = animal.InitialSpeed / animal.Speed;
            var sizeTransform = animal.Size / animal.InitialSize;

            this.scaleTransform.ScaleX = this.Scale * speedTransform;
            this.scaleTransform.ScaleY = this.Scale * sizeTransform;
        }

        private void initialiseTransformGroup()
        {
            this.transformGroup.Children.Add(this.scaleTransform);
            this.transformGroup.Children.Add(this.rotateTransform);
            this.transformGroup.Children.Add(this.translateTransform);
        }
    }
}