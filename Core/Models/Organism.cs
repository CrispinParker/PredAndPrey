namespace PredAndPrey.Core.Models
{
    public abstract class Organism
    {
        public double Size { get; set; }

        public double Health { get; set; }

        public int Age { get; set; }

        public Position Position { get; set; }

        public abstract void Behave(Environment environment);
    }
}
