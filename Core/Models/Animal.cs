namespace PredAndPrey.Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Environment = PredAndPrey.Core.Environment;

    public abstract class Animal : Organism
    {
        public const double HungerPercentage = 0.75;

        private const double HealthBehaviourCost = 0.1;

        private const double ChanceOfMutation = 0.1;

        private const int ContactDistance = 3;

        private const double MutationAmount = 0.03;

        private const double ReproductiveHealthPercentage = 0.5;

        private readonly Random rnd;

        protected Animal()
        {
            this.Features = new Dictionary<string, double>();
            this.rnd = new Random(this.Id);
        }

        public int Generation { get; set; }

        public abstract double InitialSpeed { get; }

        public abstract double InitialSize { get; }

        public abstract double InitialSight { get; }

        public double Speed { get; set; }

        public double Direction { get; set; }

        public double Sight { get; set; }

        public Dictionary<string, double> Features { get; private set; }

        public bool IsReproductive
        {
            get
            {
                return this.Health > (this.Size * ReproductiveHealthPercentage);
            }
        }

        public bool IsHungry
        {
            get
            {
                return this.Health <= (this.Size * HungerPercentage);
            }
        }

        public Animal Reproduce(Animal mate)
        {
            var maxHealth = this.GetInheritedValue(this.Size, mate.Size);
            var rangeOfAwareness = this.GetInheritedValue(this.Sight, mate.Sight);
            var speed = this.GetInheritedValue(this.Speed, mate.Speed);

            var child = this.CreateInstance();

            child.Size = maxHealth;
            child.Health = maxHealth * ReproductiveHealthPercentage;
            child.Sight = rangeOfAwareness;
            child.Speed = speed;
            child.Generation = Math.Max(this.Generation, mate.Generation) + 1;

            foreach (var featureKey in this.Features.Keys)
            {
                var myFeature = this.Features[featureKey];
                var mateFeature = mate.Features[featureKey];

                var featureValue = this.GetInheritedValue(myFeature, mateFeature);

                child.Features.Add(featureKey, featureValue);
            }

            return child;
        }

        public override void Behave(Environment environment)
        {
            this.Health -= HealthBehaviourCost;

            if (this.Health <= 0)
            {
                return;
            }

            var organisms = environment.Organisms.ToArray();

            Tuple<double, Organism> closestPrey;
            Tuple<double, Animal> closestThreat;
            Tuple<double, Animal> closestMate;

            if (this.TryFindClosest(this.SelectPredators(organisms), out closestThreat))
            {
                this.Run(environment, closestThreat);
            }
            else if (this.IsHungry && this.TryFindClosest(this.SelectPrey(organisms), out closestPrey))
            {
                this.SeekFood(environment, closestPrey);
            }
            else if (this.IsReproductive && this.TryFindClosest(this.SelectMates(organisms), out closestMate))
            {
                this.SeekMate(environment, closestMate);
            }
            else
            {
                this.Wander(environment);
            }
        }

        protected abstract Animal CreateInstance();

        protected abstract IEnumerable<Animal> SelectMates(IEnumerable<Organism> visibleOrganisms);

        protected abstract IEnumerable<Organism> SelectPrey(IEnumerable<Organism> visibleOrganisms);

        protected abstract IEnumerable<Animal> SelectPredators(IEnumerable<Organism> visibleOrganisms);

        private double GetInheritedValue(double parentAValue, double parentBValue)
        {
            var output = this.rnd.NextDouble() > 0.5 ? parentAValue : parentBValue;

            if (this.rnd.NextDouble() <= ChanceOfMutation)
            {
                output = this.Deviate(output);
            }

            return output;
        }

        private void Run(Environment environment, Tuple<double, Animal> preditor)
        {
            this.Direction = preditor.Item2.Position.Angle(this.Position);
            this.RandomiseDirection();
            environment.Move(this, Math.Min(preditor.Item1, this.Speed));
        }

        private void Wander(Environment environment)
        {
            this.RandomiseDirection();
            environment.Move(this, this.rnd.NextDouble() * this.Speed);
        }

        private void SeekFood(Environment environment, Tuple<double, Organism> closestPrey)
        {
            if (closestPrey.Item1 < ContactDistance)
            {
                environment.Eat(this, closestPrey.Item2);
            }
            else
            {
                this.Direction = this.Position.Angle(closestPrey.Item2.Position);
                this.RandomiseDirection();
                environment.Move(this, Math.Min(closestPrey.Item1, this.Speed));
            }
        }

        private void SeekMate(Environment environment, Tuple<double, Animal> closestMate)
        {
            if (closestMate.Item1 < ContactDistance)
            {
                environment.Reproduce(this, closestMate.Item2);
            }
            else
            {
                this.Direction = this.Position.Angle(closestMate.Item2.Position);
                this.RandomiseDirection();
                environment.Move(this, Math.Min(closestMate.Item1, this.Speed));
            }
        }

        private void Shoal(Environment environment, Tuple<double, Animal> closestMate)
        {
            var idealDistance = this.Sight / 2;

            if (closestMate.Item1 > idealDistance)
            {
                this.Direction = this.Position.Angle(closestMate.Item2.Position);
                this.RandomiseDirection();
                environment.Move(this, Math.Min(idealDistance, this.Speed));
            }
            else
            {
                this.Wander(environment);
            }
        }

        private bool TryFindClosest<T>(IEnumerable<T> organisms, out Tuple<double, T> closest)
            where T : Organism
        {
            closest = organisms
                .Select(m => Tuple.Create(this.Position.Distance(m.Position), m))
                .Where(o => o.Item2 != this && o.Item1 < this.Sight)
                .OrderBy(tup => tup.Item1)
                .FirstOrDefault();

            return closest != null;
        }

        private void RandomiseDirection()
        {
            this.Direction += this.rnd.Next(30) - 15;
        }

        private double Deviate(double source)
        {
            var deviation = (source * MutationAmount) * ((this.rnd.NextDouble() * 2) - 1);

            return source + deviation;
        }
    }
}