namespace PredAndPrey.Core.Models
{
    using System;

    using Environment = PredAndPrey.Core.Environment;

    public class Plant : Organism
    {
        private readonly double speed;

        public Plant()
        {
            this.Health = 40;
            this.Size = 500;
            this.speed = new Random().NextDouble();
        }

        public virtual Plant Reproduce()
        {
            return new Plant();
        }

        public override void Behave(Environment environment)
        {
            if (this.Health <= 0)
            {
                return;
            }

            var newX = this.Position.X - this.speed;
            newX = newX <= 0 ? Environment.Instance.Width - newX : newX;

            this.Position = new Position(newX, this.Position.Y);

            if (this.Health < this.Size)
            {
                this.Health += 3.5;
            }
        }
    }
}