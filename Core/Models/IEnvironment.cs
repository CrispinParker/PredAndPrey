namespace PredAndPrey.Core.Models
{
    using System.Collections.Generic;

    public interface IEnvironment
    {
        double Width { get; }

        double Height { get; }

        IEnumerable<Organism> Organisms { get; }

        void Seed(IEnumerable<Organism> organismsToAdd);

        IEnumerable<Organism> Look(Animal animal);

        void Move(Animal animal, double distance);

        void Reproduce<T>(T parentA, T parentB) where T : Animal;

        void Eat(Animal pred, Organism prey);

        void PassTime();

        void Deficate(Organism parent, Organism spore);
    }
}