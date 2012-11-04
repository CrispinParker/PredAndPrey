namespace PredAndPrey.Wpf.Core.Visuals
{
    using System.Windows.Media;

    using PredAndPrey.Core.Models;

    public class HerbivoreCVisual : AnimalVisual
    {
        private static readonly Geometry[] Geometrys = new[]
                {
                    Geometry.Parse("M32.286875,29.833005 C34.555893,8.19707 25.320776,0.49999999 16.560418,0.49999999 7.8000606,0.49999999 -0.93510978,8.6106184 0.69838711,30.333 0.69838711,30.333 10.240316,111.18593 16.547721,128.86266 17.32183,131.03213 15.83111,64.576059 16.56026,64.576059 17.314447,64.576059 16.341026,131.03369 17.126158,128.86618 23.54559,111.14409 32.286875,29.833005 32.286875,29.833005 z"),
                    Geometry.Parse("M32.286875,29.833005 C34.555893,8.19707 25.320776,0.49999999 16.560418,0.49999999 7.8000606,0.49999999 -0.93510978,8.6106184 0.69838711,30.333 0.69838711,30.333 -0.5096688,111.18593 5.7977363,128.86266 6.5718453,131.03213 15.83111,64.576059 16.56026,64.576059 17.314447,64.576059 27.091027,131.03369 27.876159,128.86618 34.295591,111.14409 32.286875,29.833005 32.286875,29.833005 z")
                };

        public HerbivoreCVisual(Animal animal)
            : base(animal)
        {
        }

        protected override double Scale
        {
            get
            {
                return 0.3d;
            }
        }

        protected override Geometry[] GeometryData
        {
            get
            {
                return Geometrys;
            }
        }
    }
}