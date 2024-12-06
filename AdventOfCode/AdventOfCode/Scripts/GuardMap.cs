using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
    }
    
    public class GuardMap
    {
        private class Cell
        {
            public Coordinate Coordinate;
            public bool Blocker { get; set; }
            public bool Visited { get; set; }
            public Coordinate DirectionEnteredIn { get; set; }

            public Cell(int x, int y, bool blocker)
            {
                Coordinate = new Coordinate(x,y);
                Blocker = blocker;
            }

            public void Reset()
            {
                Visited = false;
                DirectionEnteredIn = new Coordinate(0, 0);
            }
        }

        private readonly Dictionary<int,Cell> _cells = new Dictionary<int,Cell>();
        
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
                        var cell = new Cell(x, y, true);
                        _cells.Add(cell.Coordinate.GetHashCode(), cell);
                    }
                    else if (cells[x] == '.' || cells[x] == '^')
                    {
                        var cell = new Cell(x, y, false);
                        _cells.Add(cell.Coordinate.GetHashCode(), cell);
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
            return _cells.TryGetValue(coordinate.GetHashCode(), out cell);
        }

        public int GetUniqueCellsVisited()
        {
            return _cells.Values.Count(c => c.Visited);
        }

        private void ResetCells()
        {
            foreach (var cell in _cells.Values)
            {
                cell.Reset();
            }
        }

        public bool ProcessWalk()
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
            var guardPosition = new Coordinate(_guardPositionStart.X, _guardPositionStart.Y);
            var loopDetected = false;
            ResetCells();

            //Console.WriteLine("\nBEFORE WALK:\n");
            //LogCells();
            
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
                    if (cell.Visited &&
                        cell.DirectionEnteredIn.Equals(direction))
                    {
                        loopDetected = true;
                        break;
                    }
                    
                    cell.Visited = true;
                    cell.DirectionEnteredIn = direction;
                }

                guardPosition.X += direction.X;
                guardPosition.Y += direction.Y;
            }
            
            //Console.WriteLine("\nAFTER WALK:\n");
            //LogCells();
            return loopDetected;
        }

        private void LogCells()
        {
            var y = 0;
            var debug = string.Empty;
            foreach (var cell in _cells.Values)
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

        public int GetCellsThatCanBecomeBlockersToIntroduceLoops()
        {
            var flippableCells = _cells.Values.Where(c => c.Visited && !c.Coordinate.Equals(_guardPositionStart)).ToList();
            var possibleCells = 0;
            var index = 0;
            
            foreach (var flippableCell in flippableCells)
            {
                flippableCell.Blocker = true;

                if (ProcessWalk())
                {
                    possibleCells++;
                }
                
                flippableCell.Blocker = false;
                index++;
                Console.WriteLine($"{index} / {flippableCells.Count} : Found So Far: {possibleCells}");
            }
            
            return possibleCells;
        }
    }
}