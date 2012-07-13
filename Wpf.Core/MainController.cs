namespace PredAndPrey.Wpf.Core
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Threading;

    using PredAndPrey.Core;
    using PredAndPrey.Core.Models;
    using PredAndPrey.Wpf.Core.Properties;

    using Environment = PredAndPrey.Core.Environment;

    public class MainController
    {
        private readonly DisplayElementFactory displayElementFacotry;

        private bool isPassingTime;

        public MainController()
        {
            Environment.Instance.Reset();

            this.CreateCommands();
            this.CreateCollections();
            this.displayElementFacotry = new DisplayElementFactory();

            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(50) };
            timer.Tick += this.PassTime;
            timer.Start();
        }

        public ICommand Reset { get; set; }

        public ICommand Pause { get; set; }

        public ICommand ShowSettings { get; set; }

        public bool IsPaused { get; set; }

        public ObservableCollection<StatBase> Stats { get; private set; }

        public ObservableCollection<FrameworkElement> Plants { get; private set; }

        public ObservableCollection<FrameworkElement> Herbivores { get; private set; }

        public ObservableCollection<FrameworkElement> Carnivores { get; private set; }

        private static void PurgeDecessed(int[] livingIds, ICollection<FrameworkElement> targetCollection)
        {
            foreach (var child in targetCollection.ToArray())
            {
                var elementId = child.Tag as int?;

                if (!elementId.HasValue || !livingIds.Contains(elementId.Value))
                {
                    targetCollection.Remove(child);
                }
            }
        }

        private void CreateCollections()
        {
            this.Stats = new ObservableCollection<StatBase>();
            this.Plants = new ObservableCollection<FrameworkElement>();
            this.Herbivores = new ObservableCollection<FrameworkElement>();
            this.Carnivores = new ObservableCollection<FrameworkElement>();
        }

        private void CreateCommands()
        {
            this.Reset = new UICommand(o => Environment.Instance.Reset());
            this.Pause = new UICommand(o => this.IsPaused = this.IsPaused != true);
            this.ShowSettings = new UICommand(o => new SettingsWindow { Owner = o as Window }.Show());
        }

        private void PassTime(object sender, EventArgs eventArgs)
        {
            if (this.isPassingTime || this.IsPaused)
            {
                return;
            }

            this.isPassingTime = true;

            for (int i = 0; i < Settings.Default.RunSpeed; i++)
            {
                Environment.Instance.PassTime();
            }

            if (!Environment.Instance.Organisms.OfType<Animal>().Any())
            {
                Environment.Instance.Reset();                
            }

            this.UpdateUIElements();

            if (Settings.Default.ShowStats)
            {
                this.UpdateStatistics();
            }

            this.isPassingTime = false;
        }

        private void UpdateUIElements()
        {
            var organisms = Environment.Instance.Organisms.ToArray();

            this.DrawOrganism(organisms.OfType<Plant>(), this.Plants, true);
            this.DrawOrganism(organisms.OfType<Herbivore>(), this.Herbivores);
            this.DrawOrganism(organisms.OfType<Carnivore>(), this.Carnivores);

            var livingIds = organisms.Select(o => o.Id).ToArray();

            this.displayElementFacotry.Purge(livingIds);
            PurgeDecessed(livingIds, this.Plants);
            PurgeDecessed(livingIds, this.Herbivores);
            PurgeDecessed(livingIds, this.Carnivores);
        }

        private void DrawOrganism(IEnumerable<Organism> organisms, ICollection<FrameworkElement> targetCollection, bool centreOnElement = false)
        {
            foreach (var organism in organisms)
            {
                var left = organism.Position.X;
                var top = organism.Position.Y;

                var element = this.displayElementFacotry.GetElement(organism);

                if (centreOnElement)
                {
                    left -= element.Width / 2;
                    top -= element.Height / 2;
                }

                element.SetValue(Canvas.LeftProperty, left);
                element.SetValue(Canvas.TopProperty, top);

                if (!targetCollection.Contains(element))
                {
                    targetCollection.Add(element);
                }
            }
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
