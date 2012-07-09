namespace PredAndPrey.Core.Models
{
    public abstract class Organism
    {
        protected Organism()
        {
            this.Id = Environment.Instance.GetNextId();
        }

        public int Id { get; private set; }

        public double Size { get; set; }

        public double Health { get; set; }

        public int Age { get; set; }

        public Position Position { get; set; }

        public abstract void Behave(Environment environment);
    }
}
