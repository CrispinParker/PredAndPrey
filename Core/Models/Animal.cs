namespace PredAndPrey.Core.Models
{
    public abstract class Animal : Organism
    {
        public double Speed { get; set; }

        public double Direction { get; set; }

        public double RangeOfAwareness { get; set; }
    }
}