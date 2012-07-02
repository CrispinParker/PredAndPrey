namespace PredAndPrey.Core.Models
{
    using System;

    public class Position
    {
        public double X { get; set; }

        public double Y { get; set; }

        public static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public double Distance(Position target)
        {
            var lengthX = this.X - target.X;
            var lengthY = this.Y - target.Y;

            var sqrX = lengthX * lengthX;
            var sqrY = lengthY * lengthY;

            var sqrDistance = sqrX + sqrY;

            return Math.Sqrt(sqrDistance);
        }

        public double Angle(Position target)
        {
            return Math.Atan2(target.Y - this.Y, target.X - this.X) * 180 / Math.PI;
        }
    }
}