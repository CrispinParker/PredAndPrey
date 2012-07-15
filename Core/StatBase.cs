namespace PredAndPrey.Core
{
    using System.Collections.Generic;

    using PredAndPrey.Core.Models;

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
}