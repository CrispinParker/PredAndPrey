namespace PredAndPrey.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using PredAndPrey.Core.Models;

    public class Statistics : IEnumerable<StatBase>
    {
        private readonly List<StatBase> stats = new List<StatBase>();

        public Statistics()
        {
            this.stats.Add(new Stat<Organism>("Total", orgs => orgs.Count()));

            this.stats.Add(new Stat<Herbivore>("Prey", orgs => orgs.Count()));
            this.stats.Add(new Stat<Herbivore>("    Avg. Age", orgs => orgs.Average(o => o.Age)));
            this.stats.Add(new Stat<Herbivore>("    Avg. Health", orgs => orgs.Average(o => o.Health)));
            this.stats.Add(new Stat<Herbivore>("    Avg. Sight", orgs => orgs.Average(o => o.Sight)));
            this.stats.Add(new Stat<Herbivore>("    Avg. Speed", orgs => orgs.Average(o => o.Speed)));
            this.stats.Add(new Stat<Herbivore>("    Avg. Size", orgs => orgs.Average(o => o.Size)));
            this.stats.Add(new Stat<Herbivore>("    Avg. Generation", orgs => orgs.Average(o => o.Generation)));

            this.stats.Add(new Stat<Carnivore>("Preditor", orgs => orgs.Count()));
            this.stats.Add(new Stat<Carnivore>("    Avg. Age", orgs => orgs.Average(o => o.Age)));
            this.stats.Add(new Stat<Carnivore>("    Avg. Health", orgs => orgs.Average(o => o.Health)));
            this.stats.Add(new Stat<Carnivore>("    Avg. Sight", orgs => orgs.Average(o => o.Sight)));
            this.stats.Add(new Stat<Carnivore>("    Avg. Speed", orgs => orgs.Average(o => o.Speed)));
            this.stats.Add(new Stat<Carnivore>("    Avg. Size", orgs => orgs.Average(o => o.Size)));
            this.stats.Add(new Stat<Carnivore>("    Avg. Generation", orgs => orgs.Average(o => o.Generation)));

            this.stats.Add(new Stat<Plant>("Plants", orgs => orgs.Count()));
        }

        public void Calculate(IEnumerable<Organism> organisms)
        {
            var iteratedArray = organisms.ToArray();

            foreach (var stat in this.stats)
            {
                stat.Calculate(iteratedArray);
            }
        }

        public IEnumerator<StatBase> GetEnumerator()
        {
            return this.stats.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public abstract class StatBase
    {
        protected StatBase(string title)
        {
            this.Title = title;
        }

        public string Title { get; protected set; }

        public double Initial { get; protected set; }

        public double Max { get; protected set; }

        public double Min { get; protected set; }

        public double Current { get; protected set; }

        public abstract void Calculate(IEnumerable<Organism> organisms);
    }

    public class Stat<T> : StatBase where T : Organism
    {
        private readonly Func<IEnumerable<T>, double> method;

        private bool hasInitialised;

        public Stat(string title, Func<IEnumerable<T>, double> method)
            : base(title)
        {
            this.method = method;
        }

        public override void Calculate(IEnumerable<Organism> organisms)
        {
            var typedArray = organisms.OfType<T>().ToArray();

            if (!typedArray.Any())
            {
                this.Current = 0;
                return;
            }

            this.Current = this.method(typedArray);

            if (!this.hasInitialised)
            {
                this.hasInitialised = true;

                this.Initial = this.Current;
                this.Min = this.Current;
            }

            if (this.Max < this.Current)
            {
                this.Max = this.Current;
            }

            if (this.Min > this.Current)
            {
                this.Min = this.Current;
            }
        }
    }
}
