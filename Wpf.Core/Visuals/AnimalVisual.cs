namespace PredAndPrey.Wpf.Core.Visuals
{
    using System;
    using System.Linq;
    using System.Windows.Media;

    using PredAndPrey.Core.Models;

    public abstract class AnimalVisual : OrganismVisual
    {
        private readonly TranslateTransform translateTransform = new TranslateTransform();

        private readonly RotateTransform rotateTransform = new RotateTransform();

        private readonly ScaleTransform scaleTransform = new ScaleTransform();

        private readonly Geometry[] geometrys;

        private Geometry frozenGeometry;

        private bool isFirstUpdate = true;

        private double centerX;

        private double centerY;

        protected AnimalVisual(Animal animal)
            : base(BrushFromAnimal(animal), new Pen(Brushes.Black, 0.5))
        {
            this.geometrys = this.InitialiseGeometrys();
        }

        public override Geometry Geometry
        {
            get
            {
                return this.frozenGeometry;
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
                this.SetupDimensions(animal);
            }

            this.translateTransform.X = animal.Position.X - this.centerX;
            this.translateTransform.Y = animal.Position.Y - this.centerY;

            this.rotateTransform.Angle = animal.Direction - 270;

            this.frozenGeometry = this.GetRandomGeometry().Clone();
            this.frozenGeometry.Freeze();
        }

        private static Brush BrushFromAnimal(Animal animal)
        {
            var red = Math.Ceiling(animal.Features["Red"]);
            var green = Math.Ceiling(animal.Features["Green"]);
            var blue = Math.Ceiling(animal.Features["Blue"]);

            red = red < 0 ? 0 : red > 255 ? 255 : red;
            green = green < 0 ? 0 : green > 255 ? 255 : green;
            blue = blue < 0 ? 0 : blue > 255 ? 255 : blue;

            return new SolidColorBrush(Color.FromRgb((byte)red, (byte)green, (byte)blue));
        }

        private Geometry GetRandomGeometry()
        {
            var rnd = new Random();

            return this.geometrys[rnd.Next(0, this.geometrys.Count())];
        }

        private void SetupDimensions(Animal animal)
        {
            var speedTransform = animal.InitialSpeed / animal.Speed;
            var sizeTransform = animal.Size / animal.InitialSize;

            this.scaleTransform.ScaleX = this.Scale * speedTransform;
            this.scaleTransform.ScaleY = this.Scale * sizeTransform;

            var bounds = this.geometrys.First().Bounds;

            this.centerX = bounds.Width / 2;
            this.centerY = bounds.Height / 2;

            this.rotateTransform.CenterX = this.centerX;
            this.rotateTransform.CenterY = this.centerY * 1.2;

            this.scaleTransform.CenterX = this.centerX;
            this.scaleTransform.CenterY = this.centerY;
        }

        private Geometry[] InitialiseGeometrys()
        {
            return this.GeometryData.Select(CreateGeometry).ToArray();
        }

        private Geometry CreateGeometry(Geometry data)
        {
            var geometry = data.Clone();

            var transformGroup = new TransformGroup();
            transformGroup.Children.Add(this.scaleTransform);
            transformGroup.Children.Add(this.rotateTransform);
            transformGroup.Children.Add(this.translateTransform);

            geometry.Transform = transformGroup;

            return geometry;
        }
    }
}