using System;
using System.Collections.Generic;

namespace AdventOfCode
{
    public class LargeWarehouseNavigator : WarehouseNavigator
    {

        private class BigBoxCell : IWarehouseCell
        {
            public bool LeftHalf { get; }
            public Coordinate Coordinate { get; set; }
            public BigBoxCell OtherHalf { get; set; }
            public override string ToString() => LeftHalf? "[":"]";

            public BigBoxCell(Coordinate coordinate, bool leftHalf)
            {
                LeftHalf = leftHalf;
                Coordinate = coordinate;
            }

            public int GetCoordinateValue()
            {
                return !LeftHalf ? 0 : (Coordinate.Y * 100) + Coordinate.X;
            }
        }
        
        public LargeWarehouseNavigator(string filename) : base(filename)
        {
            Width += Width;
        }

        public override void ProcessRobotMoves()
        {
            var toMove = new HashSet<IWarehouseCell>();
            var front = new List<Coordinate>();
            var index = 0;
            foreach (var directionIndex in DirectionIndices)
            {
                index++;
                var direction = CoordinateUtils.CardinalDirections[directionIndex];
                toMove.Clear();
                toMove.Add(WarehouseRobot);
                front.Clear();
                front.Add(WarehouseRobot.Coordinate);

                var blockerFound = false;
                var pushingBoxes = false;

                do
                {
                    var nextFront = new List<Coordinate>();
                    pushingBoxes = false;
                    foreach (var frontCoord in front)
                    {
                        var coordToCheck = frontCoord + direction;
                        if (Map.TryGetCellAtCoordinate(coordToCheck, out var cell))
                        {
                            toMove.Add(cell);
                        }

                        if (cell is BlockerCell)
                        {
                            blockerFound = true;
                            break;
                        }

                        if (cell is BigBoxCell bigBoxCell)
                        {
                            pushingBoxes = true;

                            if (direction.X == 0)
                            {
                                nextFront.Add(bigBoxCell.OtherHalf.Coordinate);
                                nextFront.Add(coordToCheck);
                            }
                            else if (direction.X < 0)
                            {
                                if (bigBoxCell.LeftHalf)
                                {
                                    nextFront.Add(bigBoxCell.Coordinate);
                                }
                                else
                                {
                                    nextFront.Add(bigBoxCell.OtherHalf.Coordinate);
                                }
                            }
                            else if (direction.X > 0)
                            {
                                if (!bigBoxCell.LeftHalf)
                                {
                                    nextFront.Add(bigBoxCell.Coordinate);
                                }
                                else
                                {
                                    nextFront.Add(bigBoxCell.OtherHalf.Coordinate);
                                }
                            }

                            toMove.Add(bigBoxCell.OtherHalf);
                        }
                        else
                        {
                            nextFront.Add(coordToCheck);
                        }
                    }
                    front.Clear();
                    front.AddRange(nextFront);
                    
                } while (!blockerFound && pushingBoxes);

                if (blockerFound)
                {
                    PrintMap(index, directionIndex);
                    continue;
                }
                
                MoveCells(toMove, direction);
                
                PrintMap(index, directionIndex);
            }
        }

        public override int GetBoxCoordinateSum()
        {
            var result = 0;

            foreach (var mapValue in Map.Values)
            {
                if (!(mapValue is BigBoxCell boxCell))
                {
                    continue;
                }
                
                result += boxCell.GetCoordinateValue();
            }
            
            return result;
        }

        protected override void AddLineToMap(string line, int yPos)
        {
            var cells = line.ToCharArray(); 
            for (var xPos = 0; xPos < cells.Length; xPos++)
            {
                var symbol = cells[xPos];

                if (char.IsControl(symbol) || symbol == '.')
                {
                    continue;
                }
                    
                // left half
                var x = xPos * 2;
                var coordinate = new Coordinate(x, yPos);
                var leftCell = GetCellForCharacter(symbol, coordinate);
                Map.AddCell(coordinate, leftCell);

                if (leftCell is Robot robot)
                {
                    WarehouseRobot = robot;
                    continue;
                }
                
                
                // right half
                coordinate = new Coordinate(x+1, yPos);
                var rightCell = GetCellForCharacter(symbol, coordinate);
                
                Map.AddCell(coordinate, rightCell);

                if (rightCell is BigBoxCell bigBoxCellRight &&
                    leftCell is BigBoxCell bigBoxCellLeft)
                {
                    bigBoxCellLeft.OtherHalf = bigBoxCellRight;
                    bigBoxCellRight.OtherHalf = bigBoxCellLeft;
                }
            }
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
                    return new BigBoxCell(coordinate, coordinate.X % 2 == 0);
                default:
                    throw new ArgumentException();
            }
        }
    }
}