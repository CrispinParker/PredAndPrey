namespace PredAndPrey.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using PredAndPrey.Core.Models;

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