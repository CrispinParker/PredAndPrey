namespace PredAndPrey.Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class Animal : Organism
    {
        private const double HealthBehaviourCost = 0.04;

        private const double ChanceOfMutation = 0.05;

        private const int ContactDistance = 2;

        private const double MutationAmount = 0.5d;

        private readonly Random rnd = new Random((int)DateTime.Now.Ticks);

        protected Animal()
        {
            this.Features = new Dictionary<string, double>();
        }

        public int Generation { get; set; }

        public double Speed { get; set; }

        public double Direction { get; set; }

        public double RangeOfAwareness { get; set; }

        public Dictionary<string, double> Features { get; private set; }

        public bool IsReproductive
        {
            get
            {
                return this.Health >= (this.MaxHealth * 0.5);
            }
        }

        public bool IsHungry
        {
            get
            {
                return this.Health <= (this.MaxHealth * 0.75);
            }
        }

        public Animal Reproduce(Animal mate)
        {
            var maxHealth = this.GetInheritedValue(this.MaxHealth, mate.MaxHealth);
            var rangeOfAwareness = this.GetInheritedValue(this.RangeOfAwareness, mate.RangeOfAwareness);
            var speed = this.GetInheritedValue(this.Speed, mate.Speed);

            var child = this.CreateChild();

            child.MaxHealth = maxHealth;
            child.Health = maxHealth * 0.75;
            child.RangeOfAwareness = rangeOfAwareness;
            child.Speed = speed;
            child.Generation = Math.Max(this.Generation, mate.Generation) + 1;

            var featureKeys = this.Features.Keys;
            foreach (var featureKey in featureKeys)
            {
                var myFeature = this.Features[featureKey];
                var mateFeature = mate.Features[featureKey];

                var featureValue = this.GetInheritedValue(myFeature, mateFeature);

                child.Features.Add(featureKey, featureValue);
            }

            return child;
        }

        public double GetInheritedValue(double parentAValue, double parentBValue)
        {
            var output = this.rnd.NextDouble() > 0.5 ? parentAValue : parentBValue;

            if (this.rnd.NextDouble() <= ChanceOfMutation)
            {
                output = this.Deviate(output);
            }

            return output;
        }

        public override void Behave(IEnvironment environment)
        {
            this.Health -= this.Speed * HealthBehaviourCost;

            if (this.Health <= 0)
            {
                return;
            }

            var visibleOrganisms = environment.Organisms.ToArray();

            var closestMate = this.GetClosest(this.SelectMates(visibleOrganisms).Where(p => p != this && p.IsReproductive));
            var closestPrey = this.GetClosest(this.SelectPrey(visibleOrganisms).ToArray());
            var closestThreat = this.GetClosest(this.SelectPreditors(visibleOrganisms));

            if (closestThreat != null && closestThreat.Item1 < this.RangeOfAwareness)
            {
                this.Run(environment, closestThreat);
            }
            else if (this.IsHungry && closestPrey != null && closestPrey.Item1 < this.RangeOfAwareness)
            {
                this.SeekFood(environment, closestPrey);
            }
            else if (this.IsReproductive && closestMate != null && closestMate.Item1 < this.RangeOfAwareness)
            {
                this.SeekMate(environment, closestMate);
            }
            else
            {
                this.Wander(environment);
            }
        }

        protected void Run(IEnvironment environment, Tuple<double, Animal> preditor)
        {
            this.Direction = preditor.Item2.Position.Angle(this.Position);
            this.RandomiseDirection();
            environment.Move(this, Math.Min(preditor.Item1, this.Speed));
        }

        protected void Wander(IEnvironment environment)
        {
            this.RandomiseDirection();
            environment.Move(this, rnd.NextDouble() * this.Speed);
        }

        protected void SeekFood(IEnvironment environment, Tuple<double, Organism> closestPrey)
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

        protected void SeekMate(IEnvironment environment, Tuple<double, Animal> closestMate)
        {
            if (closestMate.Item1 < ContactDistance)
            {
                environment.Reproduce(this, closestMate.Item2);
                this.Wander(environment);
            }
            else
            {
                this.Direction = this.Position.Angle(closestMate.Item2.Position);
                this.RandomiseDirection();
                environment.Move(this, Math.Min(closestMate.Item1, this.Speed));
            }
        }

        protected abstract Animal CreateChild();

        protected abstract IEnumerable<Animal> SelectMates(IEnumerable<Organism> visibleOrganisms);

        protected abstract IEnumerable<Organism> SelectPrey(IEnumerable<Organism> visibleOrganisms);

        protected abstract IEnumerable<Animal> SelectPreditors(IEnumerable<Organism> visibleOrganisms);

        private Tuple<double, T> GetClosest<T>(IEnumerable<T> organisms)
            where T : Organism
        {
            return organisms
                .Select(m => Tuple.Create(this.Position.Distance(m.Position), m))
                .OrderBy(tup => tup.Item1)
                .FirstOrDefault();
        }

        private void RandomiseDirection()
        {
            this.Direction += this.rnd.Next(40) - 20;
        }

        private double Deviate(double source)
        {
            var deviation = (source * MutationAmount) * (this.rnd.NextDouble() - 0.5);

            return source + deviation;
        }
    }
}