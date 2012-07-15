namespace PredAndPrey.Wpf.Core.Visuals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using PredAndPrey.Core.Models;

    public class OrganismVisualFactory
    {
        private readonly IDictionary<Type, Func<Organism, OrganismVisual>> factoryMap = new Dictionary<Type, Func<Organism, OrganismVisual>>();

        private readonly IDictionary<int, OrganismVisual> elementMap = new Dictionary<int, OrganismVisual>();

        public OrganismVisualFactory()
        {
            this.factoryMap.Add(typeof(HerbivoreA), CreateHerbivoreAVisual);
            this.factoryMap.Add(typeof(HerbivoreB), CreateHerbivoreBVisual);
            this.factoryMap.Add(typeof(CarnivoreA), CreateCanivoreAVisual);
            this.factoryMap.Add(typeof(CarnivoreB), CreateCanivoreBVisual);
            this.factoryMap.Add(typeof(Plant), CreatePlantElement);
        }

        public OrganismVisual[] GetVisuals(IEnumerable<Organism> organisms)
        {
            var iteratedArray = organisms.ToArray();

            var output = new List<OrganismVisual>();

            foreach (var organism in iteratedArray.OfType<Plant>().Cast<Organism>().Union(iteratedArray.OfType<Herbivore>()).Union(iteratedArray.OfType<Carnivore>()))
            {
                OrganismVisual visual;
                if (!this.elementMap.TryGetValue(organism.Id, out visual))
                {
                    visual = this.factoryMap[organism.GetType()](organism);
                    this.elementMap.Add(organism.Id, visual);
                }

                visual.UpdateGeometry(organism);

                output.Add(visual);
            }

            return output.ToArray();
        }

        public void Purge(IEnumerable<int> livingIds)
        {
            foreach (var idToRemove in this.elementMap.Keys.Where(k => livingIds.All(id => id != k)).ToArray())
            {
                this.elementMap.Remove(idToRemove);
            }
        }

        private static OrganismVisual CreateHerbivoreAVisual(Organism organism)
        {
            return new HerbivoreAVisual((Animal)organism);
        }

        private static OrganismVisual CreateHerbivoreBVisual(Organism organism)
        {
            return new HerbivoreBVisual((Animal)organism);
        }

        private static OrganismVisual CreateCanivoreAVisual(Organism organism)
        {
            return new CarnivoreAVisual((Animal)organism);
        }

        private static OrganismVisual CreateCanivoreBVisual(Organism organism)
        {
            return new CarnivoreBVisual((Animal)organism);
        }

        private static OrganismVisual CreatePlantElement(Organism organism)
        {
            return new PlantVisual(organism);
        }
    }
}
