using System.Collections.Generic;

namespace AdventOfCode
{
    public static class CoordinateUtils
    {
        public static readonly List<Coordinate> Directions = new List<Coordinate>
        {
            new Coordinate(0, -1),
            new Coordinate(1, 0),
            new Coordinate(0, 1),
            new Coordinate(-1, 0)
        };
    }
    
    public class Coordinate
    {
        public readonly int X;
        public readonly int Y;

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
            return X * 173 + Y;
        }

        public Coordinate GetReflection(Coordinate other)
        {
            return new Coordinate(this.X + (this.X - other.X), this.Y + (this.Y - other.Y));
        }
        
        public static Coordinate operator +(Coordinate c1, Coordinate c2)
        {
            return new Coordinate(c1.X + c2.X, c1.Y + c2.Y);
        }
        
        public static Coordinate operator -(Coordinate c1, Coordinate c2)
        {
            return new Coordinate(c1.X - c2.X, c1.Y - c2.Y);
        }
    }
}