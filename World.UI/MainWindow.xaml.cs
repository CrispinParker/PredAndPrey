namespace World.UI
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using System.Windows.Threading;

    using PredAndPrey.Core;
    using PredAndPrey.Core.Models;

    using Environment = PredAndPrey.Core.Environment;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private readonly Geometry preditoreGeometry1;

        private readonly Geometry preditoryGeometry2;

        private readonly Geometry preyGeometry1;

        private readonly Geometry preyGeometry2;

        private bool isPassingTime;

        private int trackedAnimal;

        public MainWindow()
        {
            this.preditoreGeometry1 = Geometry.Parse("M4.4583524,12.75 C3.7217159,14.829125 4.1673398,17.084405 3.2744659,18.631597 1.5830007,21.562605 -1.0089202,23.597217 -1.8791008,23.625299 -2.6823161,23.65122 0.23795338,20.937608 1.5429095,18.538761 2.3828402,16.994751 1.0559103,14.809055 0.5,12.75 -1.3336364,5.9583333 1.4513949,0.5 2.625,0.5 3.7986051,0.5 6.7908351,6.1666667 4.4583524,12.75 z");
            this.preditoryGeometry2 = Geometry.Parse("M4.4583524,12.75 C3.7217159,14.829125 2.2084026,16.754817 3.2744659,18.631597 4.8983605,21.490426 6.6036858,23.597217 5.7335052,23.625299 4.9302899,23.65122 2.8612837,20.647061 1.5429095,18.538761 0.33284332,16.603664 1.0559103,14.809055 0.5,12.75 -1.3336364,5.9583333 1.4513949,0.5 2.625,0.5 3.7986051,0.5 6.7908351,6.1666667 4.4583524,12.75 z");
            this.preyGeometry1 = Geometry.Parse("M54.565006,44.497998 C100.12446,2.3330005 56.450366,29.833334 56.450366,29.833334 59.239801,8.1971567 47.886538,0.50000053 37.116925,0.50000053 26.347312,0.50000053 15.608663,8.6107097 17.616815,30.333334 17.616815,30.333334 -26.092497,4.6663338 19.52068,45.458001 19.52068,45.458001 28.759452,79.500001 37.116925,79.500001 45.624884,79.500001 54.565006,44.497998 54.565006,44.497998 z");
            this.preyGeometry2 = Geometry.Parse("M54.565006,44.497998 C98.086102,81.333 56.450366,29.833334 56.450366,29.833334 59.239801,8.1971567 47.886538,0.50000053 37.116925,0.50000053 26.347312,0.50000053 15.608663,8.6107097 17.616815,30.333334 17.616815,30.333334 -26.083952,83.666333 19.52068,45.458001 19.52068,45.458001 28.759452,79.500001 37.116925,79.500001 45.624884,79.500001 54.565006,44.497998 54.565006,44.497998 z");

            this.Stats = new ObservableCollection<StatBase>();
            this.GameSpeed = 1;

            InitializeComponent();

            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(50) };
            timer.Tick += this.PassTime;
            timer.Start();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<StatBase> Stats { get; private set; }

        public bool IsPaused { get; set; }

        public int GameSpeed { get; set; }

        private static Brush ColourFromOrganism(Animal animal)
        {
            var red = Math.Ceiling(animal.Features["Red"]);
            var green = Math.Ceiling(animal.Features["Green"]);
            var blue = Math.Ceiling(animal.Features["Blue"]);

            red = red < 0 ? 0 : red > 255 ? 255 : red;
            green = green < 0 ? 0 : green > 255 ? 255 : green;
            blue = blue < 0 ? 0 : blue > 255 ? 255 : blue;

            return new SolidColorBrush(Color.FromRgb((byte)red, (byte)green, (byte)blue));
        }

        private void PassTime(object sender, EventArgs eventArgs)
        {
            if (this.isPassingTime || this.IsPaused)
            {
                return;
            }

            this.isPassingTime = true;

            for (int i = 0; i < this.GameSpeed; i++)
            {
                Environment.Instance.PassTime();
            }

            this.DrawWorld();

            this.UpdateStatistics();

            this.isPassingTime = false;
        }

        private void DrawWorld()
        {
            PART_Canvas.Children.Clear();
            var rnd = new Random();
            var organisms = Environment.Instance.Organisms.ToArray();

            foreach (var organism in organisms.OfType<Plant>().Cast<Organism>().Union(organisms.OfType<Herbivore>()).Union(organisms.OfType<Carnivore>()))
            {
                FrameworkElement element = null;

                double canvasLeft;
                double canvasTop;

                var plant = organism as Plant;
                if (plant != null)
                {
                    var radius = Math.Max(0, plant.Health / 4);
                    element = new Ellipse
                        {
                            Width = radius, 
                            Height = radius, 
                            Fill = new RadialGradientBrush(Colors.DarkGreen, Color.FromArgb(0, 0, 255, 0))
                        };

                    canvasLeft = plant.Position.X - (element.Width / 2);
                    canvasTop = plant.Position.Y - (element.Height / 2);
                }
                else
                {
                    canvasLeft = organism.Position.X;
                    canvasTop = organism.Position.Y;
                }

                var herbivore = organism as Herbivore;
                if (herbivore != null)
                {
                    element = this.CreatePreyElement(rnd, herbivore);
                }

                var carnivore = organism as Carnivore;
                if (carnivore != null)
                {
                    element = this.CreatePredatorElement(rnd, carnivore);
                }

                if (element == null)
                {
                    continue;
                }

                element.SetValue(Canvas.LeftProperty, canvasLeft);
                element.SetValue(Canvas.TopProperty, canvasTop);

                if (this.trackedAnimal != 0 && this.trackedAnimal == Convert.ToInt32(element.Tag))
                {
                    var scaleTransform = this.PART_Canvas.RenderTransform as ScaleTransform;

                    if (scaleTransform == null)
                    {
                        this.PART_Canvas.RenderTransform = new ScaleTransform(5, 5, organism.Position.X, organism.Position.Y);
                    }
                    else
                    {
                        scaleTransform.CenterX = organism.Position.X;
                        scaleTransform.CenterY = organism.Position.Y;
                    }
                }

                PART_Canvas.Children.Add(element);
            }
        }

        private FrameworkElement CreatePreyElement(Random rnd, Herbivore herbivore)
        {
            var speedTransform = Herbivore.InitialSpeed / herbivore.Speed;
            var sizeTransform = herbivore.Size / Herbivore.InitialSize;

            var pathData = rnd.NextDouble() > 0.5 ? this.preyGeometry1 : this.preyGeometry2;

            FrameworkElement element = new Path
                {
                    Data = pathData,
                    Height = Math.Max(3, 17 * sizeTransform),
                    Width = Math.Max(2, 12 * speedTransform),
                    Fill = ColourFromOrganism(herbivore),
                    Stretch = Stretch.Fill,
                    Tag = herbivore.Id
                };

            element.RenderTransform = new RotateTransform(
                herbivore.Direction - 270, element.Width / 2, element.Height - (element.Height * 0.8));
            return element;
        }

        private FrameworkElement CreatePredatorElement(Random rnd, Carnivore carnivore)
        {
            var speedTransform = Carnivore.InitialSpeed / carnivore.Speed;
            var sizeTransform = carnivore.Size / Carnivore.InitialSize;

            var pathData = rnd.NextDouble() > 0.5 ? this.preditoreGeometry1 : this.preditoryGeometry2;

            FrameworkElement element = new Path
                {
                    Data = pathData,
                    Height = Math.Max(3, 23 * sizeTransform),
                    Width = Math.Max(2, 12 * speedTransform),
                    Fill = ColourFromOrganism(carnivore),
                    Stretch = Stretch.Fill,
                    Tag = carnivore.Id
                };

            element.RenderTransform = new RotateTransform(
                carnivore.Direction - 270, element.Width / 2, element.Height - (element.Height * 0.8));

            return element;
        }

        private void UpdateStatistics()
        {
            this.Stats.Clear();

            foreach (var statistic in Environment.Instance.Statistics)
            {
                this.Stats.Add(statistic);
            }
        }

        private void SeedHerbivors(object sender, RoutedEventArgs e)
        {
            var rnd = new Random();

            var organisms = new List<Organism>();

            var red = 255 * rnd.NextDouble();
            var green = 255 * rnd.NextDouble();

            for (int i = 0; i < 10; i++)
            {
                var herbivore = Herbivore.GenerateDefault();
                herbivore.Features.Add("Red", red);
                herbivore.Features.Add("Green", green);
                herbivore.Features.Add("Blue", 255);

                organisms.Add(herbivore);
            }

            Environment.Instance.Seed(organisms);
        }

        private void SeedCarnivors(object sender, RoutedEventArgs e)
        {
            var rnd = new Random();

            var organisms = new List<Organism>();

            var green = 255 * rnd.NextDouble();
            var blue = 126 * rnd.NextDouble();

            for (int i = 0; i < 5; i++)
            {
                var carnivore = Carnivore.GenerateDefault();
                carnivore.Features.Add("Red", 255);
                carnivore.Features.Add("Green", green);
                carnivore.Features.Add("Blue", blue);

                organisms.Add(carnivore);
            }

            Environment.Instance.Seed(organisms);
        }

        private void Reset(object sender, RoutedEventArgs e)
        {
            Environment.Instance = null;
        }

        private void HandleCanvasMouseDown(object sender, MouseButtonEventArgs e)
        {
            var pt = e.GetPosition((UIElement)sender);

            var hitTestResult = VisualTreeHelper.HitTest(this.PART_Canvas, pt);

            var path = hitTestResult.VisualHit as Path;

            if (path != null && (path.Tag is int))
            {
                this.trackedAnimal = Convert.ToInt32(path.Tag);
            }
            else
            {
                this.PART_Canvas.RenderTransform = null;
                this.trackedAnimal = 0;
            }
        }
    }
}
