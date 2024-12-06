using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public struct Coordinate
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
    }
    
    public class GuardMap
    {
        private class Cell
        {
            public Coordinate Coordinate;
            public bool Blocker { get; }
            public bool Visited { get; set; }

            public Cell(int x, int y, bool blocker)
            {
                Coordinate = new Coordinate(x,y);
                Blocker = blocker;
            }
        }

        private List<Cell> _cells = new List<Cell>();
        private Coordinate _guardPositionStart;
        
        public GuardMap(string filename)
        {
            InitialiseCells(filename);
        }

        private void InitialiseCells(string filename)
        {
            var sr = new StreamReader(
                Path.Combine(Environment.CurrentDirectory, filename));
            
            var line = sr.ReadLine();

            var y = 0;
            
            while (line != null)
            {
                var cells = line.ToCharArray();
                
                for (var x = 0; x < cells.Length; x++)
                {
                    if (cells[x] == '#')
                    {
                        _cells.Add(new Cell(x, y, true));
                    }
                    else if (cells[x] == '.' || cells[x] == '^')
                    {
                        _cells.Add(new Cell(x, y, false));
                        if (cells[x] == '^')
                        {
                            _guardPositionStart = new Coordinate(x, y);
                        }
                    }
                }
                
                line = sr.ReadLine();
                y++;
            }
        }

        private bool TryGetCellAtCoordinate(Coordinate coordinate, out Cell cell)
        {
            cell = _cells.FirstOrDefault(c => c.Coordinate.Equals(coordinate));
            return cell != null;
        }

        public int GetUniqueCellsVisited()
        {
            return _cells.Count(c => c.Visited);
        }

        public void ProcessWalk()
        {
            var directions = new List<Coordinate>
            {
                new Coordinate(0, -1),
                new Coordinate(1, 0),
                new Coordinate(0, 1),
                new Coordinate(-1, 0)
            };
            var directionIndex = 0;
            var direction = directions[directionIndex];
            var guardPosition = _guardPositionStart;

            Console.WriteLine("\nBEFORE WALK:\n");
            LogCells();
            
            while (TryGetCellAtCoordinate(guardPosition, out var cell))
            {
                if (cell.Blocker)
                {
                    guardPosition.X -= direction.X;
                    guardPosition.Y -= direction.Y;
                    directionIndex++;
                    if (directionIndex >= directions.Count)
                    {
                        directionIndex = 0;
                    }
                    direction = directions[directionIndex];
                }
                else
                {
                    cell.Visited = true;
                }

                guardPosition.X += direction.X;
                guardPosition.Y += direction.Y;
            }
            
            Console.WriteLine("\nAFTER WALK:\n");
            LogCells();
        }

        private void LogCells()
        {
            var y = 0;
            var debug = string.Empty;
            foreach (var cell in _cells)
            {
                if (cell.Coordinate.Y != y)
                {
                    debug += "\n";
                    y = cell.Coordinate.Y;
                }

                if (cell.Visited)
                {
                    debug += "X";
                }
                else if (cell.Blocker)
                {
                    debug += "#";
                }
                else
                {
                    debug += ".";
                }
            }
            
            Console.WriteLine(debug);
        }
    }
}