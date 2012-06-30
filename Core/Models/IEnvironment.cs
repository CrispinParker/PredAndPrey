namespace PredAndPrey.Core.Models
{
    using System.Collections.Generic;

    public interface IEnvironment
    {
        double Width { get; }

        double Height { get; }

        IEnumerable<Organism> Organisms { get; }

        void Seed(IEnumerable<Organism> organismsToAdd);

        IEnumerable<Organism> Look(Animal me);

        void Move(Animal animal);

        void Reproduce(Organism parent);

        void Eat(Animal pred, Organism prey);

        void PassTime();
    }
}