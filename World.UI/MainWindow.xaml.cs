namespace World.UI
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using System.Windows.Threading;

    using PredAndPrey.Core;
    using PredAndPrey.Core.Models;

    using Environment = PredAndPrey.Core.Models.Environment;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private readonly Geometry preditorGeometry1;

        private readonly Geometry preditorGeometry2;

        private bool isPassingTime;

        public MainWindow()
        {
            this.preditorGeometry1 = Geometry.Parse("M4.4583524,12.75 C3.7217159,14.829125 4.1673398,17.084405 3.2744659,18.631597 1.5830007,21.562605 -1.0089202,23.597217 -1.8791008,23.625299 -2.6823161,23.65122 0.23795338,20.937608 1.5429095,18.538761 2.3828402,16.994751 1.0559103,14.809055 0.5,12.75 -1.3336364,5.9583333 1.4513949,0.5 2.625,0.5 3.7986051,0.5 6.7908351,6.1666667 4.4583524,12.75 z");
            this.preditorGeometry2 = Geometry.Parse("M4.4583524,12.75 C3.7217159,14.829125 2.2084026,16.754817 3.2744659,18.631597 4.8983605,21.490426 6.6036858,23.597217 5.7335052,23.625299 4.9302899,23.65122 2.8612837,20.647061 1.5429095,18.538761 0.33284332,16.603664 1.0559103,14.809055 0.5,12.75 -1.3336364,5.9583333 1.4513949,0.5 2.625,0.5 3.7986051,0.5 6.7908351,6.1666667 4.4583524,12.75 z");
            this.Stats = new ObservableCollection<StatBase>();

            InitializeComponent();

            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(50) };
            timer.Tick += this.PassTime;
            timer.Start();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<StatBase> Stats { get; private set; }

        public bool IsPaused { get; set; }

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

            this.DrawWorld();
            Environment.Instance.PassTime();

            this.Stats.Clear();

            foreach (var statistic in Environment.Instance.Statistics)
            {
                this.Stats.Add(statistic);
            }

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
                    element = new Ellipse
                        {
                            Width = Math.Max(1.5, 9 - herbivore.Speed), 
                            Height = herbivore.Size / 24, 
                            Fill = ColourFromOrganism(herbivore),
                            RenderTransform = new RotateTransform(herbivore.Direction - 270, 0.5, 0.1)
                        };
                }

                var carnivore = organism as Carnivore;
                if (carnivore != null)
                {
                    var pathData = rnd.NextDouble() > 0.5 ? this.preditorGeometry1 : this.preditorGeometry2;

                    element = new Path
                    {
                        Data = pathData,
                        Width = Math.Max(2, 28 - (carnivore.Speed * 4)),
                        Height = carnivore.Size / 7,
                        Fill = ColourFromOrganism(carnivore),
                        RenderTransform = new RotateTransform(carnivore.Direction - 270, 0.5, 0.1),
                        Stretch = Stretch.Fill
                    };
                }

                if (element == null)
                {
                    continue;
                }

                element.SetValue(Canvas.LeftProperty, canvasLeft);
                element.SetValue(Canvas.TopProperty, canvasTop);

                PART_Canvas.Children.Add(element);
            }
        }

        private void SeedHerbivors(object sender, RoutedEventArgs e)
        {
            var rnd = new Random();

            var organisms = new List<Organism>();

            var red = 255 * rnd.NextDouble();
            var green = 255 * rnd.NextDouble();
            var blue = 255;

            for (int i = 0; i < 10; i++)
            {
                var herbivore = Herbivore.GenerateDefault();
                herbivore.Features.Add("Red", red);
                herbivore.Features.Add("Green", green);
                herbivore.Features.Add("Blue", blue);

                organisms.Add(herbivore);
            }

            Environment.Instance.Seed(organisms);
        }

        private void SeedCarnivors(object sender, RoutedEventArgs e)
        {
            var rnd = new Random();

            var organisms = new List<Organism>();

            var red = 255;
            var green = 255 * rnd.NextDouble();
            var blue = 126 * rnd.NextDouble();

            for (int i = 0; i < 5; i++)
            {
                var carnivore = Carnivore.GenerateDefault();
                carnivore.Features.Add("Red", red);
                carnivore.Features.Add("Green", green);
                carnivore.Features.Add("Blue", blue);

                organisms.Add(carnivore);
            }

            Environment.Instance.Seed(organisms);
        }
    }
}
