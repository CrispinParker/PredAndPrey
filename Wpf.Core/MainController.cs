namespace PredAndPrey.Wpf.Core
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;

    using PredAndPrey.Core;
    using PredAndPrey.Core.Models;
    using PredAndPrey.Wpf.Core.Visuals;

    using Environment = PredAndPrey.Core.Environment;

    public class MainController : INotifyPropertyChanged
    {
        private readonly OrganismVisualFactory visualFactory;

        private bool isPassingTime;

        public MainController()
        {
            Environment.Instance.Reset();
            
            this.Stats = new ObservableCollection<StatBase>(); 
            this.CreateCommands();
            this.visualFactory = new OrganismVisualFactory();
            this.PlantVisualHost = new VisualHost();
            this.HerbivoreVisualHost = new VisualHost();
            this.CarnivoreVisualHost = new VisualHost();

            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(50) };
            timer.Tick += this.PassTime;
            timer.Start();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand Reset { get; set; }

        public ICommand Pause { get; set; }

        public ICommand ShowSettings { get; set; }

        public bool IsPaused { get; set; }

        public double ScreenWidth
        {
            get
            {
                return SettingsHelper.Instance.ScreenWidth;
            }
        }

        public double ScreenHeight
        {
            get
            {
                return SettingsHelper.Instance.ScreenHeight;
            }
        }

        public bool ShowStatistics
        {
            get
            {
                return SettingsHelper.Instance.ShowStatistics;
            }
        }

        public ObservableCollection<StatBase> Stats { get; private set; }

        public VisualHost PlantVisualHost { get; private set; }

        public VisualHost HerbivoreVisualHost { get; private set; }

        public VisualHost CarnivoreVisualHost { get; private set; }

        private void CreateCommands()
        {
            this.Reset = new UICommand(o => Environment.Instance.Reset());
            this.Pause = new UICommand(o => this.IsPaused = this.IsPaused != true);
            this.ShowSettings = new UICommand(this.ShowSettingsWindow);
        }

        private void ShowSettingsWindow(object obj)
        {
            var parentWindow = obj as Window;

            var settingsWindow = new SettingsWindow { Owner = parentWindow };

            settingsWindow.ShowDialog();

            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(string.Empty));
            }
        }

        private void PassTime(object sender, EventArgs eventArgs)
        {
            if (this.isPassingTime || this.IsPaused)
            {
                return;
            }

            this.isPassingTime = true;

            for (int i = 0; i < SettingsHelper.Instance.RunSpeed; i++)
            {
                Environment.Instance.PassTime();
            }

            var organisms = Environment.Instance.Organisms.ToArray();

            if (!organisms.OfType<Animal>().Any())
            {
                Environment.Instance.Reset();
            }

            var plants = this.visualFactory.GetVisuals(organisms.OfType<Plant>());
            var herbivores = this.visualFactory.GetVisuals(organisms.OfType<Herbivore>());
            var carnivores = this.visualFactory.GetVisuals(organisms.OfType<Carnivore>());

            this.PlantVisualHost.RenderVisuals(plants);
            this.HerbivoreVisualHost.RenderVisuals(herbivores);
            this.CarnivoreVisualHost.RenderVisuals(carnivores);

            if (SettingsHelper.Instance.ShowStatistics)
            {
                this.UpdateStatistics();
            }

            this.visualFactory.Purge(organisms.Select(o => o.Id));

            this.isPassingTime = false;
        }

        private void UpdateStatistics()
        {
            this.Stats.Clear();

            foreach (var statistic in Environment.Instance.Statistics)
            {
                this.Stats.Add(statistic);
            }
        }
    }
}
