using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode
{
    public class MazeNavigator
    {
        private interface IMazeCell : ICell
        {
            Coordinate Coordinate { get; set; }
        }

        private class EmptyCell : IMazeCell
        {
            public Coordinate Coordinate { get; set; }

            public EmptyCell(Coordinate coordinate)
            {
                Coordinate = coordinate;
            }
        }

        private class WallCell : IMazeCell
        {
            public Coordinate Coordinate { get; set; }

            public WallCell(Coordinate coordinate)
            {
                Coordinate = coordinate;
            }
        }
        
        private readonly CellMap<IMazeCell> _mazeMap = new CellMap<IMazeCell>();
        
        private Coordinate _startPoint;
        private Coordinate _endPoint;
        private List<EmptyCell> _nodes = new List<EmptyCell>();
        
        public MazeNavigator(string filename)
        {
            var sr = new StreamReader(
                Path.Combine(Environment.CurrentDirectory, filename));
            
            var line = sr.ReadLine();

            var y = 0;
            
            while (line != null)
            {
                AddLineToMap(line, y);
                y++;
                
                line = sr.ReadLine();
            }
        }

        private void AddLineToMap(string line, int yPos)
        {
            var cells = line.ToCharArray(); 
            for (var xPos = 0; xPos < cells.Length; xPos++)
            {
                var symbol = cells[xPos];

                if (char.IsControl(symbol))
                {
                    continue;
                }
                    
                var coordinate = new Coordinate(xPos, yPos);

                if (symbol == '#')
                {
                    _mazeMap.AddCell(coordinate, new WallCell(coordinate));
                    continue;
                }

                if (symbol == 'S')
                {
                    _startPoint = coordinate;
                }

                if (symbol == 'E')
                {
                    _endPoint = coordinate;
                }
                
                _mazeMap.AddCell(coordinate, new EmptyCell(coordinate));
            }
        }

        public int GetSmallestNavigationScore()
        {
            return -1;
        }

        public void BuildNodes()
        {
            _nodes.Clear();

            foreach (var mazeCell in _mazeMap.Values)
            {
                if (!(mazeCell is EmptyCell emptyCell))
                {
                    continue;
                }

                var neighbourCells = new List<EmptyCell>();
                foreach (var direction in CoordinateUtils.CardinalDirections)
                {
                    var coordToCheck = emptyCell.Coordinate + direction;
                    if (_mazeMap.TryGetCellAtCoordinate(coordToCheck, out var cell) &&
                        cell is EmptyCell emptyNeighbour)
                    {
                        neighbourCells.Add(emptyNeighbour);
                    }
                }

                if (neighbourCells.Count < 2)
                {
                    continue;
                }

                if (neighbourCells.Count > 2)
                {
                    _nodes.Add(emptyCell);
                    continue;
                }

                var firstCoordinate = neighbourCells[0].Coordinate;
                var secondCoordinate = neighbourCells[1].Coordinate;
                if (firstCoordinate.X == secondCoordinate.X ||
                    firstCoordinate.Y == secondCoordinate.Y)
                {
                    continue;
                }
                
                _nodes.Add(emptyCell);
            }
            
            Console.WriteLine($"Map has {_nodes.Count} nodes");
        }
    }
}