using System.Collections.Generic;

namespace AdventOfCode
{
    public static class CoordinateUtils
    {
        public static readonly List<Coordinate> CardinalDirections = new List<Coordinate>
        {
            new Coordinate(0, -1),
            new Coordinate(1, 0),
            new Coordinate(0, 1),
            new Coordinate(-1, 0)
        };
        
        public static readonly List<Coordinate> Directions = new List<Coordinate>
        {
            new Coordinate(0, -1), // top
            new Coordinate(1, -1), // top right
            new Coordinate(1, 0), // right
            new Coordinate(1, 1), // bottom right
            new Coordinate(0, 1), // bottom
            new Coordinate(-1, 1), // bottom left
            new Coordinate(-1, 0), // left
            new Coordinate(-1, -1) // top left
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

        public override string ToString()
        {
            return $"({X}, {Y})";
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
        
        public static Coordinate operator *(Coordinate c1, int coeff)
        {
            return new Coordinate(c1.X * coeff, c1.Y * coeff);
        }
    }
}