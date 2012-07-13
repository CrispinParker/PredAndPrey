namespace PredAndPrey.Core.Models
{
    using System;

    public struct Position
    {
        public Position(double x, double y)
            : this()
        {
            this.X = x;
            this.Y = y;
        }

        public double X { get; private set; }

        public double Y { get; private set; }

        public static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public double Distance(Position target)
        {
            var x = this.X - target.X;
            var y = this.Y - target.Y;

            return Math.Sqrt((x * x) + (y * y));
        }

        public double Angle(Position target)
        {
            return Math.Atan2(target.Y - this.Y, target.X - this.X) * 180 / Math.PI;
        }
    }
}