using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode
{
    public class WarehouseNavigator
    {
        protected interface IWarehouseCell : ICell
        {
            Coordinate Coordinate { get; set; }
            string ToString();
        }

        protected class BlockerCell: IWarehouseCell
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

        protected class Robot : IWarehouseCell
        {
            public Coordinate Coordinate { get; set; }
            public override string ToString() => "@";

            public Robot(Coordinate coordinate)
            {
                Coordinate = coordinate;
            }
        }
        
        protected readonly CellMap<IWarehouseCell> Map = new CellMap<IWarehouseCell>();
        protected readonly List<int> DirectionIndices = new List<int>();

        protected int Width;
        private readonly int _height;
        
        protected Robot WarehouseRobot;
        
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
                    Width = line.Length;
                    y++;
                    _height = y;
                }
                else if (line.Contains("<") || line.Contains(">") || line.Contains("^") || line.Contains("v"))
                {
                    AddDirectionIndices(line);
                }
                
                line = sr.ReadLine();
            }
        }

        public virtual void ProcessRobotMoves()
        {
            var toMove = new HashSet<IWarehouseCell>();
            var index = 0;
            foreach (var directionIndex in DirectionIndices)
            {
                index++;
                var direction = CoordinateUtils.CardinalDirections[directionIndex];
                toMove.Clear();
                toMove.Add(WarehouseRobot);
                IWarehouseCell cell = WarehouseRobot;

                do
                {
                    var coordToCheck = cell.Coordinate + direction;
                    if (Map.TryGetCellAtCoordinate(coordToCheck, out cell))
                    {
                        toMove.Add(cell);
                    }
                } while (cell != null && cell is BoxCell);

                if (cell != null)
                {
                    PrintMap(index, directionIndex);
                    continue;
                }
                
                MoveCells(toMove, direction);

                PrintMap(index, directionIndex);
            }
        }

        private string GetDirectionCharacterForIndex(int index)
        {
            switch (index)
            {
                case 0:
                    return "^";
                case 1:
                    return ">";
                case 2:
                    return "v";
                default:
                    return "<";
            }
        }

        protected void MoveCells(HashSet<IWarehouseCell> toMove, Coordinate direction)
        {
            Map.RemoveCell(WarehouseRobot.Coordinate);
                
            foreach (var warehouseCell in toMove)
            {
                Map.RemoveCell(warehouseCell.Coordinate);
            }
                
            foreach (var cellMoving in toMove)
            {
                cellMoving.Coordinate += direction;
            }
                
            foreach (var warehouseCell in toMove)
            {
                Map.ReplaceCell(warehouseCell.Coordinate, warehouseCell);
            }
        }

        public virtual int GetBoxCoordinateSum()
        {
            var result = 0;

            foreach (var mapValue in Map.Values)
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
                        DirectionIndices.Add(0);
                        break;
                    case '>':
                        DirectionIndices.Add(1);
                        break;
                    case 'v':
                        DirectionIndices.Add(2);
                        break;
                    case '<':
                        DirectionIndices.Add(3);
                        break;
                }
            }
        }

        protected virtual void AddLineToMap(string line, int yPos)
        {
            var cells = line.ToCharArray(); 
            for (var xPos = 0; xPos < cells.Length; xPos++)
            {
                var symbol = cells[xPos];

                if (char.IsControl(symbol) || symbol == '.')
                {
                    continue;
                }
                    
                var coordinate = new Coordinate(xPos, yPos);
                var cell = GetCellForCharacter(symbol, coordinate);

                if (cell is Robot robot)
                {
                    WarehouseRobot = robot;
                }
                
                Map.AddCell(coordinate, cell);
            }
        }
        
        protected void PrintMap(int index, int directionIndex)
        {
            Console.WriteLine($"\nMOVE {index} {GetDirectionCharacterForIndex(directionIndex)}:");
            var str = string.Empty;
            var lastY = 0;
            var coord = new Coordinate(0, 0);

            for (var yPos = 0; yPos < _height; yPos++)
            {
                coord.Y = yPos;
                for (var xPos = 0; xPos < Width; xPos++)
                {
                    coord.X = xPos;
                    if (Map.TryGetCellAtCoordinate(coord, out var cell))
                    {
                        str += cell.ToString();
                    }
                    else
                    {
                        str += ".";
                    }
                }
                str += "\n";
            }
            
            Console.Write(str);
        }

        private IWarehouseCell GetCellForCharacter(char character, Coordinate coordinate)
        {
            switch (character)
            {
                case '#':
                    return new BlockerCell(coordinate);
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