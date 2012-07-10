namespace PredAndPrey.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using PredAndPrey.Core.Models;

    public class Environment
    {
        private const int MaxElements = 1000;

        private const int HealthPerBiteModifier = 5;

        private const int MaxNumOfPlants = 100;

        private const double ChanceOfSeedingAPlant = 0.3;

        private const int HealthAfterReproduction = 10;

        private const double MaxSpeed = 10;

        private const double MinimumSize = 40;

        private static Environment instance;

        private static int idSeed;

        private readonly List<Organism> organisms = new List<Organism>();

        private readonly object syncLock = new object();

        private readonly Random rnd;

        private Environment()
        {
            this.Width = 1000;
            this.Height = 750;
            this.rnd = new Random();
            this.Statistics = new Statistics();
        }

        public static Environment Instance
        {
            get
            {
                return instance = instance ?? new Environment();
            }

            set
            {
                instance = value;
            }
        }

        public Statistics Statistics { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public IEnumerable<Organism> Organisms
        {
            get
            {
                return this.InvokeLocked(() => this.organisms.ToArray());
            }
        }

        public int GetNextId()
        {
            return this.InvokeLocked(() => ++idSeed);
        }

        public void Seed(IEnumerable<Organism> organismsToAdd)
        {
            var enumerable = organismsToAdd.ToArray();

            foreach (var organism in enumerable)
            {
                var x = this.rnd.NextDouble() * this.Width;
                var y = this.rnd.NextDouble() * this.Height;

                organism.Position = new Position(x, y);
            }

            this.InvokeLocked(() => this.organisms.AddRange(enumerable));
        }

        public void PassTime()
        {
            this.SeedPlants();

            this.InvokeLocked(() => this.organisms.RemoveAll(o => o.Health <= 0));

            var localArray = this.Organisms.ToArray();

            foreach (var organism in localArray)
            {
                this.DoBehaviour(organism);
                organism.Age++;
            }

            this.Statistics.Calculate(localArray);
        }

        public Animal GenerateDefault<TAnimal>() where TAnimal : Animal, new()
        {
            var output = new TAnimal();

            output.Size = output.InitialSize;
            output.Health = output.InitialSize * Animal.HungerPercentage;
            output.Speed = output.InitialSpeed;
            output.Sight = output.InitialSight;
            output.Generation = 1;

            return output;
        }

        internal void Move(Animal animal, double distance)
        {
            var radian = Position.DegreeToRadian(animal.Direction);
            
            var x = animal.Position.X + (distance * Math.Cos(radian));
            var y = animal.Position.Y + (distance * Math.Sin(radian));

            x = x < 0 ? 0 : x > this.Width ? this.Width : x;
            y = y < 0 ? 0 : y > this.Height ? this.Height : y;

            animal.Position = new Position(x, y);
        }

        internal void Reproduce<T>(T parentA, T parentB)
            where T : Animal
        {
            var enumeratedList = this.Organisms.ToArray();

            if (enumeratedList.Count() > MaxElements)
            {
                return;
            }

            var speciesCount = enumeratedList.Count(o => o.GetType() == parentA.GetType());

            var child = parentA.Reproduce(parentB);

            child.Speed = child.Speed > MaxSpeed ? MaxSpeed : child.Speed;
            child.Size = child.Size < MinimumSize ? MinimumSize : child.Size;

            child.Position = new Position(parentA.Position.X, parentA.Position.Y);

            if (speciesCount < 10)
            {
                parentA.Health = 500;
                parentB.Health = 500;
                child.Health = 500;
            }
            else
            {
                parentA.Health = HealthAfterReproduction;
                parentB.Health = HealthAfterReproduction;
            }

            this.InvokeLocked(() => this.organisms.Add(child));
        }

        internal void Eat(Animal pred, Organism prey)
        {
            var consumed = Math.Min(prey.Health, pred.Size / HealthPerBiteModifier);
            prey.Health -= consumed;
            pred.Health += consumed;
        }

        private void DoBehaviour(Organism organism)
        {
            Task.Factory.StartNew(() => organism.Behave(this))
                .ContinueWith(t =>
                    {
                        if (t.Exception != null)
                        {
                            this.LogException(t.Exception);
                        }
                    });
        }

        private void LogException(AggregateException exception)
        {
            // Todo - Log exceptions.
        }

        private void SeedPlants()
        {
            var numOfPlants = this.Organisms.OfType<Plant>().Count();

            if (numOfPlants < MaxNumOfPlants && this.rnd.NextDouble() < ChanceOfSeedingAPlant)
            {
                this.Seed(new[] { new Plant() });
            }
        }

        private T InvokeLocked<T>(Func<T> method)
        {
            T output;
            lock (this.syncLock)
            {
                output = method();
            }

            return output;
        }

        private void InvokeLocked(Action method)
        {
            this.InvokeLocked(
                () =>
                    {
                        method();
                        return true;
                    });
        }
    }
}
