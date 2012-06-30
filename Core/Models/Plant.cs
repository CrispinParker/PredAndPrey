namespace PredAndPrey.Core.Models
{
    public class Plant : Organism
    {
        public const int ReproductiveSize = 10;

        public Plant()
        {
            this.Health = 100;
            this.Size = 1;
        }

        public override Organism Reproduce()
        {
            return new Plant();
        }

        public override void Behave(IEnvironment environment)
        {
            this.Size++;

            if (this.Size > ReproductiveSize)
            {
                environment.Reproduce(this);
            }
        }
    }
}