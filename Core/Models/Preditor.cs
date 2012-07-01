namespace PredAndPrey.Core.Models
{
    using System;
    using System.Linq;

    public class Preditor : Animal
    {
        public const int ReproductiveSize = 20;

        public override Organism Reproduce()
        {
            return new Preditor { Health = this.Health, Speed = this.Speed };
        }

        public override void Behave(IEnvironment environment)
        {
            if (this.Size > ReproductiveSize)
            {
                environment.Reproduce(this);
            }

            var visiblePrey = environment.Look(this).OfType<Prey>().ToArray();

            if (visiblePrey.Any())
            {
                var cloestPrey = visiblePrey.OrderBy(p => p.Position.Distance(this.Position)).First();

                this.Direction = Math.Atan2(cloestPrey.Position.Y - this.Position.Y, cloestPrey.Position.X - this.Position.X) * 180 / Math.PI;

                environment.Move(this);
            }
        }
    }
}