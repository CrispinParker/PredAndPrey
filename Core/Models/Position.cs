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
            var lengthX = this.X - target.X;
            var lengthY = this.Y - target.Y;

            var sqrX = Math.Pow(lengthX, 2);
            var sqrY = Math.Pow(lengthY, 2);

            var ceiling = Math.Ceiling(sqrX + sqrY);

            return Math.Sqrt(ceiling);
        }

        public double Angle(Position target)
        {
            return Math.Atan2(target.Y - this.Y, target.X - this.X) * 180 / Math.PI;
        }
    }
}