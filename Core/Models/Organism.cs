namespace PredAndPrey.Core.Models
{
    public abstract class Organism
    {
        public int Health { get; set; }

        public int Size { get; set; }

        public int Age { get; set; }

        public Position Position { get; set; }

        public abstract void Behave(IEnvironment environment);

        public abstract Organism Reproduce();
    }
}
