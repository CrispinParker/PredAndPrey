namespace PredAndPrey.Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class Environment : IEnvironment
    {
        private static IEnvironment instance;

        private readonly List<Organism> organisms = new List<Organism>();

        private readonly object syncLock = new object();

        private Environment()
        {
            this.Width = 800;
            this.Height = 600;
        }

        public static IEnvironment Instance
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
            var rnd = new Random();

            foreach (var organism in organismsToAdd)
            {
                var position = new Position
                    {
                        X = rnd.NextDouble() * this.Width,
                        Y = rnd.NextDouble() * this.Height
                    };

                organism.Position = position;

                var organism1 = organism;
                this.InvokeLocked(() => this.organisms.Add(organism1));
            }
        }

        public IEnumerable<Organism> Look(Animal me)
        {
            return this.InvokeLocked(() => this.organisms.Where(o => me.Position.Distance(o.Position) <= me.RangeOfAwareness).ToArray());
        }

        public void Move(Animal animal)
        {
            var radian = DegreeToRadian(animal.Direction);

            var newX = animal.Position.X += animal.Speed * Math.Cos(radian);
            var newY = animal.Position.Y += animal.Speed * Math.Sin(radian);

            animal.Position.X = newX < 0 ? 0 : newX > this.Width ? this.Width : newX;
            animal.Position.Y = newY < 0 ? 0 : newY > this.Height ? this.Height : newY;

            animal.Health -= (int)Math.Ceiling(animal.Speed);
        }

        public void Reproduce(Organism parent)
        {
            var child = parent.Reproduce();
            child.Position = new Position { X = parent.Position.X, Y = parent.Position.Y };

            parent.Size -= child.Size;

            this.InvokeLocked(() => this.organisms.Add(child));
        }

        public void Eat(Animal pred, Organism prey)
        {
            var consumed = Math.Min(prey.Health, pred.Size);

            prey.Health -= consumed;
            pred.Health += consumed;
        }

        public void PassTime()
        {
            var tasks = new List<Task>();

            foreach (var organism in this.Organisms.ToArray())
            {
                organism.Age++;
                organism.Health--;

                if (organism.Health > 0)
                {
                    var organism1 = organism;
                    tasks.Add(Task.Factory.StartNew(() => organism1.Behave(this)));
                }
            }

            Task.WaitAll(tasks.ToArray());

            foreach (var organism in this.organisms.Where(o => o.Health < 1).ToArray())
            {
                this.organisms.Remove(organism);
            }
        }

        private static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
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
