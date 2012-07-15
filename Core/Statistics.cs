namespace PredAndPrey.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using PredAndPrey.Core.Models;

    public class Statistics : IEnumerable<StatBase>
    {
        private readonly List<StatBase> stats = new List<StatBase>();

        private double counter;

        private string logFilePath;

        public Statistics()
        {
            this.stats.Add(new Stat<Organism>("World Age", orgs => Math.Ceiling(this.counter / 10)));
            this.stats.Add(new Stat<Organism>("Total Organisms", orgs => orgs.Count()));

            this.stats.Add(new Stat<Herbivore>("Prey", orgs => orgs.Count()));
            this.stats.Add(new Stat<Herbivore>("    Avg. Age", orgs => orgs.Average(o => o.Age) / 10));
            this.stats.Add(new Stat<Herbivore>("    Avg. Health", orgs => orgs.Average(o => o.Health)));
            this.stats.Add(new Stat<Herbivore>("    Avg. Sight", orgs => orgs.Average(o => o.Sight)));
            this.stats.Add(new Stat<Herbivore>("    Avg. Speed", orgs => orgs.Average(o => o.Speed)));
            this.stats.Add(new Stat<Herbivore>("    Avg. Size", orgs => orgs.Average(o => o.Size)));
            this.stats.Add(new Stat<Herbivore>("    Avg. Generation", orgs => orgs.Average(o => o.Generation)));

            this.stats.Add(new Stat<Carnivore>("Predator", orgs => orgs.Count()));
            this.stats.Add(new Stat<Carnivore>("    Avg. Age", orgs => orgs.Average(o => o.Age) / 10));
            this.stats.Add(new Stat<Carnivore>("    Avg. Health", orgs => orgs.Average(o => o.Health)));
            this.stats.Add(new Stat<Carnivore>("    Avg. Sight", orgs => orgs.Average(o => o.Sight)));
            this.stats.Add(new Stat<Carnivore>("    Avg. Speed", orgs => orgs.Average(o => o.Speed)));
            this.stats.Add(new Stat<Carnivore>("    Avg. Size", orgs => orgs.Average(o => o.Size)));
            this.stats.Add(new Stat<Carnivore>("    Avg. Generation", orgs => orgs.Average(o => o.Generation)));

            this.stats.Add(new Stat<Plant>("Plants", orgs => orgs.Count()));

            // this.CreateLogFile();
        }

        public void Calculate(IEnumerable<Organism> organisms)
        {
            var iteratedArray = organisms.ToArray();

            foreach (var stat in this.stats)
            {
                stat.Calculate(iteratedArray);
            }

            this.counter++;

            /*
            if ((decimal)this.counter % 10 == 0)
            {
                this.LogToFile();
            }
            */ 
        }

        public IEnumerator<StatBase> GetEnumerator()
        {
            return this.stats.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private void CreateLogFile()
        {
            var logFileName = "PredAndPrey." + DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss") + ".csv";

            this.logFilePath = Path.Combine(Path.GetTempPath(), logFileName);

            using (var fs = new FileStream(this.logFilePath, FileMode.CreateNew))
            using (var sr = new StreamWriter(fs))
            {
                var statsAsCsv = this.Aggregate(
                    string.Empty,
                    (current, next) =>
                    string.Concat(current, string.IsNullOrEmpty(current) ? string.Empty : ",", next.Title));

                sr.WriteLine(statsAsCsv);
            }
        }

        private void LogToFile()
        {
            using (var fs = new FileStream(this.logFilePath, FileMode.Append))
            using (var sr = new StreamWriter(fs))
            {
                var statsAsCsv = this.Aggregate(
                    string.Empty,
                    (current, next) =>
                    string.Concat(current, string.IsNullOrEmpty(current) ? string.Empty : ",", next.Current));

                sr.WriteLine(statsAsCsv);
            }
        }
    }
}
