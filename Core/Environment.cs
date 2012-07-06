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

        private const int HealthPerBite = 10;

        private const int MaxNumOfPlants = 100;

        private const double ChanceOfSeedingAPlant = 0.3;

        private const int HealthAfterReproduction = 10;

        private const double MaxSpeed = 10;

        private const double MinimumeSize = 40;

        private static Environment instance;

        private readonly List<Organism> organisms = new List<Organism>();

        private readonly object syncLock = new object();

        private readonly Random rnd;

        private Environment()
        {
            this.Width = 960;
            this.Height = 720;
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

        public void Seed(IEnumerable<Organism> organismsToAdd)
        {
            foreach (var organism in organismsToAdd)
            {
                var x = this.rnd.NextDouble() * this.Width;
                var y = this.rnd.NextDouble() * this.Height;

                organism.Position = new Position(x, y);

                var organism1 = organism;

                this.InvokeLocked(() => this.organisms.Add(organism1));
            }
        }

        public void Move(Animal animal, double distance)
        {
            var radian = Position.DegreeToRadian(animal.Direction);
            
            var x = animal.Position.X + (distance * Math.Cos(radian));
            var y = animal.Position.Y + (distance * Math.Sin(radian));

            x = x < 0 ? 0 : x > this.Width ? this.Width : x;
            y = y < 0 ? 0 : y > this.Height ? this.Height : y;

            this.InvokeLocked(
                () =>
                    {
                        animal.Position = new Position(x, y);
                    });
        }

        public void Reproduce<T>(T parentA, T parentB)
            where T : Animal
        {
            var enumeratedList = this.Organisms.ToArray();

            if (enumeratedList.Count() > MaxElements)
            {
                return;
            }

            var speciesCount = enumeratedList.OfType<T>().Count();

            var child = parentA.Reproduce(parentB);

            child.Speed = child.Speed > MaxSpeed ? MaxSpeed : child.Speed;
            child.Size = child.Size < MinimumeSize ? MinimumeSize : child.Size;

            child.Position = new Position(parentA.Position.X, parentA.Position.Y);
            
            this.InvokeLocked(() =>
                {
                    this.organisms.Add(child);
                    if (speciesCount <= 10)
                    {
                        return;
                    }

                    parentA.Health = HealthAfterReproduction;
                    parentB.Health = HealthAfterReproduction;
                });
        }

        public void Eat(Animal pred, Organism prey)
        {
            this.InvokeLocked(
                () =>
                    {
                        var consumed = Math.Min(prey.Health, HealthPerBite);
                        prey.Health -= consumed;                        
                        pred.Health += Math.Min(pred.Size - pred.Health, consumed);
                    });
        }

        public void PassTime()
        {
            var tasks = new List<Task>();

            var numOfPlants = this.Organisms.OfType<Plant>().Count();

            if (numOfPlants < MaxNumOfPlants && this.rnd.NextDouble() < ChanceOfSeedingAPlant)
            {
                this.Seed(new[] { new Plant() });
            }

            foreach (var organism in this.Organisms.ToArray())
            {
                organism.Age++;                

                if (organism.Health <= 0)
                {
                    this.organisms.Remove(organism);
                }
                else
                {
                    var organism1 = organism;
                    Task.Factory.StartNew(() => organism1.Behave(this)).ContinueWith(
                        t =>
                            {
                                if (t.Exception != null)
                                {
                                    throw t.Exception;
                                }
                            });
                }
            }

            Task.WaitAll(tasks.ToArray());

            this.Statistics.Calculate(this.Organisms);
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
