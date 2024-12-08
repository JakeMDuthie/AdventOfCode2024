using System;

namespace AdventOfCode
{
    public class Coordinate
    {
        public int X;
        public int Y;

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(Coordinate other)
        {
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return X * 131 + Y;
        }

        public Coordinate GetReflection(Coordinate other)
        {
            return new Coordinate(this.X + (this.X - other.X), this.Y + (this.Y - other.Y));
        }

        public Coordinate GetAbsDistance(Coordinate other)
        {
            return new Coordinate(Math.Abs(other.X - this.X), Math.Abs(other.Y - this.Y));
        }
    }
}