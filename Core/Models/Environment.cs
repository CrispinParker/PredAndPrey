namespace PredAndPrey.Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class Environment
    {
        private const int MaxElements = 1000;

        private const int HealthPerBite = 10;

        private const int MaxNumOfPlants = 100;

        private const double ChanceOfSeedingAPlant = 0.3;

        private const int HealthAfterReproduction = 10;

        private const double MaxSpeed = 10;

        private static Environment instance;

        private readonly List<Organism> organisms = new List<Organism>();

        private readonly object syncLock = new object();

        private readonly Random rnd;

        private Environment()
        {
            this.Width = 800;
            this.Height = 600;
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

        public double Width { get; private set; }

        public double Height { get; private set; }

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
                organism.Position.X = this.rnd.NextDouble() * this.Width;
                organism.Position.Y = this.rnd.NextDouble() * this.Height;

                var organism1 = organism;

                this.InvokeLocked(() => this.organisms.Add(organism1));
            }
        }

        public void Move(Animal animal, double distance)
        {
            var radian = Position.DegreeToRadian(animal.Direction);

            var newX = animal.Position.X += distance * Math.Cos(radian);
            var newY = animal.Position.Y += distance * Math.Sin(radian);

            newX = newX < 0 ? 0 : newX > this.Width ? this.Width : newX;
            newY = newY < 0 ? 0 : newY > this.Height ? this.Height : newY;

            this.InvokeLocked(
                () =>
                    {
                        animal.Position.X = newX;
                        animal.Position.Y = newY;
                    });
        }

        public void Deficate(Organism parent, Organism spore)
        {
            if (this.Organisms.Count() > MaxElements)
            {
                return;
            }

            spore.Position.X = parent.Position.X;
            spore.Position.Y = parent.Position.Y;

            this.InvokeLocked(() =>
            {
                this.organisms.Add(spore);
                parent.Health -= 40;
            });
        }

        public void Reproduce<T>(T parentA, T parentB)
            where T : Animal
        {
            if (this.Organisms.Count() > MaxElements)
            {
                return;
            }

            var child = parentA.Reproduce(parentB);

            child.Speed = child.Speed > MaxSpeed ? MaxSpeed : child.Speed;

            child.Position.X = parentA.Position.X;
            child.Position.Y = parentA.Position.Y;
            
            this.InvokeLocked(() =>
                {
                    this.organisms.Add(child);
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
