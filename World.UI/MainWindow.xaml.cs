namespace World.UI
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Threading;

    using PredAndPrey.Core;
    using PredAndPrey.Core.Models;

    using Environment = PredAndPrey.Core.Environment;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private readonly DisplayElementFactory displayElementFacotry;

        private bool isPassingTime;

        public MainWindow()
        {
            this.displayElementFacotry = new DisplayElementFactory();
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
            var organisms = Environment.Instance.Organisms.ToArray();

            this.PART_PlantCanvas.Children.Clear();

            FrameworkElement element;

            foreach (var plant in organisms.OfType<Plant>())
            {
                element = this.displayElementFacotry.GetElement(plant);

                var canvasLeft = plant.Position.X - (element.Width / 2);
                var canvasTop = plant.Position.Y - (element.Height / 2);

                element.SetValue(Canvas.LeftProperty, canvasLeft);
                element.SetValue(Canvas.TopProperty, canvasTop);

                this.PART_PlantCanvas.Children.Add(element);
            }

            this.PART_HerbivoreCanvas.Children.Clear();
            foreach (var herbivore in organisms.OfType<Herbivore>())
            {
                var canvasLeft = herbivore.Position.X;
                var canvasTop = herbivore.Position.Y;

                element = this.displayElementFacotry.GetElement(herbivore);

                element.SetValue(Canvas.LeftProperty, canvasLeft);
                element.SetValue(Canvas.TopProperty, canvasTop);

                this.PART_HerbivoreCanvas.Children.Add(element);
            }

            this.PART_CarnivoreCanvas.Children.Clear();
            foreach (var carnivore in organisms.OfType<Carnivore>())
            {
                var canvasLeft = carnivore.Position.X;
                var canvasTop = carnivore.Position.Y;

                element = this.displayElementFacotry.GetElement(carnivore);

                element.SetValue(Canvas.LeftProperty, canvasLeft);
                element.SetValue(Canvas.TopProperty, canvasTop);

                this.PART_CarnivoreCanvas.Children.Add(element);
            }

            this.displayElementFacotry.Purge(organisms.Select(o => o.Id).ToArray());
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
            var blue = 255d;

            for (int i = 0; i < 15; i++)
            {
                var herbivore = Environment.Instance.GenerateDefault<HerbivoreA>();

                herbivore.Features.Add("Red", red);
                herbivore.Features.Add("Green", green);
                herbivore.Features.Add("Blue", blue);

                organisms.Add(herbivore);
            }

            red = 126 * rnd.NextDouble();
            green = 255;
            blue = (126 * rnd.NextDouble()) + 126;

            for (int i = 0; i < 15; i++)
            {
                var herbivore = Environment.Instance.GenerateDefault<HerbivoreB>();

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

            const double Red = 255d;
            var green = 255 * rnd.NextDouble();
            var blue = 126 * rnd.NextDouble();

            var grey = (126 * rnd.NextDouble()) + 126;

            for (int i = 0; i < 7; i++)
            {
                var carnivore = Environment.Instance.GenerateDefault<CarnivoreA>();

                carnivore.Features.Add("Red", Red);
                carnivore.Features.Add("Green", grey);
                carnivore.Features.Add("Blue", grey);

                organisms.Add(carnivore);
            }

            for (int i = 0; i < 7; i++)
            {
                var carnivore = Environment.Instance.GenerateDefault<CarnivoreB>();

                carnivore.Features.Add("Red", Red);
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
    }
}
