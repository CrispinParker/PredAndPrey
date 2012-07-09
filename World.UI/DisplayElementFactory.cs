namespace World.UI
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Shapes;

    using PredAndPrey.Core.Models;

    public class DisplayElementFactory
    {
        private readonly IDictionary<Type, Func<Animal, FrameworkElement>> factoryMap = new Dictionary<Type, Func<Animal, FrameworkElement>>();

        private readonly Random rnd = new Random();

        private readonly Geometry herbivoreAGeometry1;

        private readonly Geometry herbivoreAGeometry2;

        private readonly Geometry herbivoreBGeometry1;

        private readonly Geometry herbivoreBGeometry2;

        private readonly Geometry carnivoreAGeometry1;

        private readonly Geometry carnivoreAGeometry2;

        private readonly Geometry carnivoreBGeometry1;

        private readonly Geometry carnivoreBGeometry2;

        public DisplayElementFactory()
        {
            this.herbivoreAGeometry1 = Geometry.Parse("M4.4583524,12.75 C3.7217159,14.829125 4.1673398,17.084405 3.2744659,18.631597 1.5830007,21.562605 -1.0089202,23.597217 -1.8791008,23.625299 -2.6823161,23.65122 0.23795338,20.937608 1.5429095,18.538761 2.3828402,16.994751 1.0559103,14.809055 0.5,12.75 -1.3336364,5.9583333 1.4513949,0.5 2.625,0.5 3.7986051,0.5 6.7908351,6.1666667 4.4583524,12.75 z");
            this.herbivoreAGeometry2 = Geometry.Parse("M4.4583524,12.75 C3.7217159,14.829125 2.2084026,16.754817 3.2744659,18.631597 4.8983605,21.490426 6.6036858,23.597217 5.7335052,23.625299 4.9302899,23.65122 2.8612837,20.647061 1.5429095,18.538761 0.33284332,16.603664 1.0559103,14.809055 0.5,12.75 -1.3336364,5.9583333 1.4513949,0.5 2.625,0.5 3.7986051,0.5 6.7908351,6.1666667 4.4583524,12.75 z");
            this.herbivoreBGeometry1 = Geometry.Parse("M4.4583524,12.75 C3.7217159,14.829125 4.1673398,17.084405 3.2744659,18.631597 1.5830007,21.562605 -1.0089202,23.597217 -1.8791008,23.625299 -2.6823161,23.65122 0.23795338,20.937608 1.5429095,18.538761 2.3828402,16.994751 1.0559103,14.809055 0.5,12.75 -1.3336364,5.9583333 1.4513949,0.5 2.625,0.5 3.7986051,0.5 6.7908351,6.1666667 4.4583524,12.75 z");
            this.herbivoreBGeometry2 = Geometry.Parse("M4.4583524,12.75 C3.7217159,14.829125 2.2084026,16.754817 3.2744659,18.631597 4.8983605,21.490426 6.6036858,23.597217 5.7335052,23.625299 4.9302899,23.65122 2.8612837,20.647061 1.5429095,18.538761 0.33284332,16.603664 1.0559103,14.809055 0.5,12.75 -1.3336364,5.9583333 1.4513949,0.5 2.625,0.5 3.7986051,0.5 6.7908351,6.1666667 4.4583524,12.75 z");
            this.carnivoreAGeometry1 = Geometry.Parse("M49.759025,57.339219 C49.052418,61.601867 50.308386,67.343452 51.213843,71.750126 53.646007,83.586967 49.858848,88.422355 51.172872,98.333361 52.89663,111.33479 66.754149,136.74484 66.714135,136.50107 66.645532,136.08313 45.646352,119.83428 40.514083,104.56509 37.401941,95.306047 25.897158,84.33691 22.216666,73.499994 20.739689,69.151156 18.04201,61.417918 17.559345,57.072002 17.559345,57.072002 0.1483824,53.089135 0.1483824,53.089135 -5.1853544,48.755802 11.244186,39.617024 16.383023,36.757001 16.383023,36.757001 14.250292,20.445917 19.57379,9.7043026 22.187206,4.4310219 31.783929,-4.6664844 34.215274,-4.6664844 36.741815,-4.6664844 45.16806,4.6448346 47.491617,10.157922 51.99165,20.835119 49.647604,36.64437 49.684681,37.416067 49.684681,37.416067 71.92003,49.206365 64.896283,53.589097 64.805284,53.645879 55.880339,53.242501 49.759025,57.339219 z");
            this.carnivoreAGeometry2 = Geometry.Parse("M49.759025,57.339219 C49.052418,61.601867 48.060212,67.264733 47.713979,71.750126 46.896756,82.337114 32.288664,98.832366 28.173803,108.58261 22.897728,121.08438 0.50676418,136.74484 0.46675018,136.50107 0.39814718,136.08313 19.397853,111.58493 20.014901,96.56571 20.415874,86.805867 21.397333,84.33691 17.716841,73.499994 16.239864,69.151156 17.57358,57.09967 17.559345,57.072002 15.960453,53.964258 -0.1013883,53.52679 -0.1013883,53.52679 -5.6009471,49.027304 11.244186,39.617024 16.383023,36.757001 16.383023,36.757001 14.250292,20.445917 19.57379,9.7043026 22.187206,4.4310219 31.783929,-4.6664844 34.215274,-4.6664844 36.741815,-4.6664844 45.16806,4.6448346 47.491617,10.157922 51.99165,20.835119 49.647604,36.64437 49.684681,37.416067 49.684681,37.416067 72.645738,51.089467 64.896283,52.339188 64.79039,52.356265 55.880339,53.242501 49.759025,57.339219 z");
            this.carnivoreBGeometry1 = Geometry.Parse("M49.759025,57.339219 C49.052418,61.601867 50.308386,67.343452 51.213843,71.750126 53.646007,83.586967 49.858848,88.422355 51.172872,98.333361 52.89663,111.33479 66.754149,136.74484 66.714135,136.50107 66.645532,136.08313 45.646352,119.83428 40.514083,104.56509 37.401941,95.306047 25.897158,84.33691 22.216666,73.499994 20.739689,69.151156 18.04201,61.417918 17.559345,57.072002 17.559345,57.072002 0.1483824,53.089135 0.1483824,53.089135 -5.1853544,48.755802 11.244186,39.617024 16.383023,36.757001 16.383023,36.757001 14.250292,20.445917 19.57379,9.7043026 22.187206,4.4310219 31.783929,-4.6664844 34.215274,-4.6664844 36.741815,-4.6664844 45.16806,4.6448346 47.491617,10.157922 51.99165,20.835119 49.647604,36.64437 49.684681,37.416067 49.684681,37.416067 71.92003,49.206365 64.896283,53.589097 64.805284,53.645879 55.880339,53.242501 49.759025,57.339219 z");
            this.carnivoreBGeometry2 = Geometry.Parse("M49.759025,57.339219 C49.052418,61.601867 48.060212,67.264733 47.713979,71.750126 46.896756,82.337114 32.288664,98.832366 28.173803,108.58261 22.897728,121.08438 0.50676418,136.74484 0.46675018,136.50107 0.39814718,136.08313 19.397853,111.58493 20.014901,96.56571 20.415874,86.805867 21.397333,84.33691 17.716841,73.499994 16.239864,69.151156 17.57358,57.09967 17.559345,57.072002 15.960453,53.964258 -0.1013883,53.52679 -0.1013883,53.52679 -5.6009471,49.027304 11.244186,39.617024 16.383023,36.757001 16.383023,36.757001 14.250292,20.445917 19.57379,9.7043026 22.187206,4.4310219 31.783929,-4.6664844 34.215274,-4.6664844 36.741815,-4.6664844 45.16806,4.6448346 47.491617,10.157922 51.99165,20.835119 49.647604,36.64437 49.684681,37.416067 49.684681,37.416067 72.645738,51.089467 64.896283,52.339188 64.79039,52.356265 55.880339,53.242501 49.759025,57.339219 z");

            this.factoryMap.Add(typeof(HerbivoreA), this.CreateHerbivoreAElement);
            this.factoryMap.Add(typeof(HerbivoreB), this.CreateHerbivoreBElement);
            this.factoryMap.Add(typeof(CarnivoreA), this.CreateCanivoreAElement);
            this.factoryMap.Add(typeof(CarnivoreB), this.CreateCanivoreBElement);
        }

        public FrameworkElement CreateElement(Animal animal)
        {
            return this.factoryMap[animal.GetType()](animal);
        }

        public FrameworkElement CreateElement(Plant plant)
        {
            var radius = Math.Max(0, plant.Health / 4);
            return new Ellipse
            {
                Width = radius,
                Height = radius,
                Fill = new RadialGradientBrush(Colors.DarkGreen, Color.FromArgb(0, 0, 255, 0))
            };
        }

        private static Brush BrushColourFromOrganism(Animal animal)
        {
            var red = Math.Ceiling(animal.Features["Red"]);
            var green = Math.Ceiling(animal.Features["Green"]);
            var blue = Math.Ceiling(animal.Features["Blue"]);

            red = red < 0 ? 0 : red > 255 ? 255 : red;
            green = green < 0 ? 0 : green > 255 ? 255 : green;
            blue = blue < 0 ? 0 : blue > 255 ? 255 : blue;

            return new SolidColorBrush(Color.FromRgb((byte)red, (byte)green, (byte)blue));
        }

        private static FrameworkElement CreateAnimalElement(Geometry pathData, double width, double height, Animal animal)
        {
            var speedTransform = animal.InitialSpeed / animal.Speed;
            var sizeTransform = animal.Size / animal.InitialSize;

            FrameworkElement element = new Path
            {
                Data = pathData,
                Height = Math.Max(3, height * sizeTransform),
                Width = Math.Max(2, width * speedTransform),
                Fill = BrushColourFromOrganism(animal),
                Stretch = Stretch.Fill,
                Stroke = Brushes.Black,
                StrokeThickness = 0.5,
                Tag = animal.Id
            };

            element.RenderTransform = new RotateTransform(
                animal.Direction - 270, element.Width / 2, element.Height - (element.Height * 0.8));

            return element;
        }

        private FrameworkElement CreateHerbivoreAElement(Animal animal)
        {
            var pathData = this.rnd.NextDouble() > 0.5 ? this.herbivoreAGeometry1 : this.herbivoreAGeometry2;

            return CreateAnimalElement(pathData, 5, 9, animal);
        }

        private FrameworkElement CreateHerbivoreBElement(Animal animal)
        {
            var pathData = this.rnd.NextDouble() > 0.5 ? this.herbivoreBGeometry1 : this.herbivoreBGeometry2;

            return CreateAnimalElement(pathData, 10, 18, animal);
        }

        private FrameworkElement CreateCanivoreAElement(Animal animal)
        {
            var pathData = this.rnd.NextDouble() > 0.5 ? this.carnivoreAGeometry1 : this.carnivoreAGeometry2;

            return CreateAnimalElement(pathData, 8.5, 16.5, animal);
        }

        private FrameworkElement CreateCanivoreBElement(Animal animal)
        {
            var pathData = this.rnd.NextDouble() > 0.5 ? this.carnivoreAGeometry1 : this.carnivoreAGeometry2;

            return CreateAnimalElement(pathData, 17, 35, animal);
        }
    }
}
