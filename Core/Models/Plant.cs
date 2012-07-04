namespace PredAndPrey.Core.Models
{
    public class Plant : Organism
    {
        public Plant()
        {
            this.Health = 40;
            this.Size = 700;
        }

        public virtual Plant Reproduce()
        {
            return new Plant();
        }

        public override void Behave(Environment environment)
        {
            if (this.Health <= 0 || this.Health > this.Size)
            {
                return;
            }

            this.Health += 5;
        }
    }
}