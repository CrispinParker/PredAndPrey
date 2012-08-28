namespace PredAndPrey.Wpf.Core.Visuals
{
    using System;
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
                var output = this.GetRandomGeometry();
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

            this.translateTransform.X = animal.Position.X;
            this.translateTransform.Y = animal.Position.Y;

            this.rotateTransform.Angle = animal.Direction + 90;
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
            var index = new Random().Next(0, this.GeometryData.Length);

            return this.GeometryData[index].Clone();
        }

        private void SetupCentrePoint()
        {
            var geometry = this.GetRandomGeometry().Clone();
            geometry.Transform = this.scaleTransform;

            var bounds = geometry.Bounds;
             
            this.rotateTransform.CenterX = bounds.Width / 2;
            this.rotateTransform.CenterY = bounds.Height / 4;
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