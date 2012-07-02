namespace PredAndPrey.Core.Models
{
    public class Plant : Organism
    {
        public Plant()
        {
            this.Health = 40;
            this.MaxHealth = 600;
        }

        public virtual Plant Reproduce()
        {
            return new Plant();
        }

        public override void Behave(IEnvironment environment)
        {
            if (this.Health > this.MaxHealth)
            {
                return;
            }

            this.Health += 4;
        }
    }
}