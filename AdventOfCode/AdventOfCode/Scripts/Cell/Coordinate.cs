using System.Collections.Generic;

namespace AdventOfCode
{
    public static class CoordinateUtils
    {
        public static readonly List<Coordinate> CardinalDirections = new List<Coordinate>
        {
            new Coordinate(0, -1), // up
            new Coordinate(1, 0), // right
            new Coordinate(0, 1), // down
            new Coordinate(-1, 0) // left
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
    
    public class LongCoordinate
    {
        public readonly long X;
        public readonly long Y;

        public LongCoordinate(long x, long y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(LongCoordinate other)
        {
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return (int)X * 173 + (int)Y;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        public LongCoordinate GetReflection(LongCoordinate other)
        {
            return new LongCoordinate(this.X + (this.X - other.X), this.Y + (this.Y - other.Y));
        }
        
        public static LongCoordinate operator +(LongCoordinate c1, LongCoordinate c2)
        {
            return new LongCoordinate(c1.X + c2.X, c1.Y + c2.Y);
        }
        
        public static LongCoordinate operator -(LongCoordinate c1, LongCoordinate c2)
        {
            return new LongCoordinate(c1.X - c2.X, c1.Y - c2.Y);
        }
        
        public static LongCoordinate operator *(LongCoordinate c1, long coeff)
        {
            return new LongCoordinate(c1.X * coeff, c1.Y * coeff);
        }
    }
}