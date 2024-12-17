using System;
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
    }
}