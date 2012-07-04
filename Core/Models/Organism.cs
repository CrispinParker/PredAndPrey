namespace PredAndPrey.Core.Models
{
    public abstract class Organism
    {
        protected Organism()
        {
            this.Position = new Position();
        }

        public double Size { get; set; }

        public double Health { get; set; }

        public int Age { get; set; }

        public Position Position { get; private set; }

        public abstract void Behave(Environment environment);
    }
}
