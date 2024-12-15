using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode
{
    public class WarehouseNavigator
    {
        private interface IWarehouseCell : ICell
        {
            Coordinate Coordinate { get; set; }
            string ToString();
        }

        private class BlockerCell: IWarehouseCell
        {
            public Coordinate Coordinate { get; set; }
            public override string ToString() => "#";
            
            public BlockerCell(Coordinate coordinate)
            {
                Coordinate = coordinate;
            }
        }

        private class BoxCell : IWarehouseCell
        {
            public Coordinate Coordinate { get; set; }
            public override string ToString() => "O";

            public BoxCell(Coordinate coordinate)
            {
                Coordinate = coordinate;
            }

            public int GetCoordinateValue()
            {
                return (Coordinate.Y*100) + Coordinate.X;
            }
        }

        private class EmptyCell : IWarehouseCell
        {
            public Coordinate Coordinate { get; set; }
            public override string ToString() => ".";

            public EmptyCell(Coordinate coordinate)
            {
                Coordinate = coordinate;
            }
        }

        private class Robot : IWarehouseCell
        {
            public Coordinate Coordinate { get; set; }
            public override string ToString() => "@";

            public Robot(Coordinate coordinate)
            {
                Coordinate = coordinate;
            }
        }
        
        private readonly CellMap<IWarehouseCell> _map = new CellMap<IWarehouseCell>();
        private readonly List<int> _directionIndices = new List<int>();
        
        private Robot _robot;
        
        public WarehouseNavigator(string filename)
        {
            var sr = new StreamReader(
                Path.Combine(Environment.CurrentDirectory, filename));
            
            var line = sr.ReadLine();

            var y = 0;
            
            while (line != null)
            {
                if (line.Contains("#"))
                {
                    AddLineToMap(line, y);
                    y++;
                }
                else if (line.Contains("<") || line.Contains(">") || line.Contains("^") || line.Contains("v"))
                {
                    AddDirectionIndices(line);
                }
                
                line = sr.ReadLine();
            }
        }

        public void ProcessRobotMoves()
        {
            var toMove = new List<IWarehouseCell>();
            foreach (var directionIndex in _directionIndices)
            {
                var direction = CoordinateUtils.CardinalDirections[directionIndex];
                toMove.Clear();
                toMove.Add(_robot);
                IWarehouseCell cell = _robot;

                do
                {
                    var coordToCheck = cell.Coordinate + direction;
                    _map.TryGetCellAtCoordinate(coordToCheck, out cell);
                    toMove.Add(cell);
                } while (cell is BoxCell);

                if (!(cell is EmptyCell emptyCell))
                {
                    continue;
                }
                
                emptyCell.Coordinate.X = _robot.Coordinate.X;
                emptyCell.Coordinate.Y = _robot.Coordinate.Y;
                
                for (var i = 0; i < toMove.Count-1; i++)
                {
                    var cellMoving = toMove[i];
                    cellMoving.Coordinate += direction;
                }
                
                foreach (var warehouseCell in toMove)
                {
                    _map.ReplaceCell(warehouseCell.Coordinate, warehouseCell);
                }
                
                //PrintMap();
            }
        }

        public int GetBoxCoordinateSum()
        {
            var result = 0;

            foreach (var mapValue in _map.Values)
            {
                if (!(mapValue is BoxCell boxCell))
                {
                    continue;
                }
                
                result += boxCell.GetCoordinateValue();
            }
            
            return result;
        }

        private void AddDirectionIndices(string line)
        {
            var cells = line.ToCharArray();
            foreach (var symbol in cells)
            {
                if (char.IsControl(symbol))
                {
                    continue;
                }

                switch (symbol)
                {
                    case '^':
                        _directionIndices.Add(0);
                        break;
                    case '>':
                        _directionIndices.Add(1);
                        break;
                    case 'v':
                        _directionIndices.Add(2);
                        break;
                    case '<':
                        _directionIndices.Add(3);
                        break;
                }
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
                var cell = GetCellForCharacter(symbol, coordinate);

                if (cell is Robot robot)
                {
                    _robot = robot;
                }
                
                _map.AddCell(coordinate, cell);
            }
        }
        
        private void PrintMap()
        {
            Console.WriteLine("\nMOVE:");
            var str = string.Empty;
            var lastY = 0;
            var coord = new Coordinate(0, 0);
            
            foreach (var warehouseCell in _map.Values)
            {
                coord.X = warehouseCell.Coordinate.X;
                coord.Y = warehouseCell.Coordinate.Y;

                if (coord.Y != lastY)
                {
                    str += "\n";
                    lastY = coord.Y;
                }
                
                str += warehouseCell.ToString();
            }
            
            Console.Write(str);
        }

        private IWarehouseCell GetCellForCharacter(char character, Coordinate coordinate)
        {
            switch (character)
            {
                case '#':
                    return new BlockerCell(coordinate);
                case '.':
                    return new EmptyCell(coordinate);
                case '@':
                    return new Robot(coordinate);
                case 'O':
                    return new BoxCell(coordinate);
                default:
                    throw new ArgumentException();
            }
        }
    }
}