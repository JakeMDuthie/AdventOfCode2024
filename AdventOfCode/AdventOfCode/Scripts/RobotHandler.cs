using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public enum Quadrant
    {
        None,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }
    
    public class Robot
    {
        private readonly Coordinate _startingPosition;
        private readonly Coordinate _velocity;
        
        public Coordinate CurrentPosition { get; private set; }

        public Robot(Coordinate startingPosition, Coordinate velocity)
        {
            _velocity = velocity;
            _startingPosition = startingPosition;
            CurrentPosition = startingPosition;
        }

        public void GetEndPositionAfterSeconds(int seconds, int roomHeight, int roomWidth)
        {
            for (var i = 0; i < seconds; i++)
            {
                CurrentPosition += _velocity;

                if (CurrentPosition.X >= roomWidth)
                {
                    CurrentPosition.X -= roomWidth;
                }
                else if (CurrentPosition.X < 0)
                {
                    CurrentPosition.X += roomWidth;
                }

                if (CurrentPosition.Y >= roomHeight)
                {
                    CurrentPosition.Y -= roomHeight;
                }
                else if (CurrentPosition.Y < 0)
                {
                    CurrentPosition.Y += roomHeight;
                }
            }
        }

        public Quadrant GetQuadrant(Coordinate midPoint)
        {
            var quadrantPosition = CurrentPosition - midPoint;

            if (quadrantPosition.X == 0 || quadrantPosition.Y == 0)
            {
                return Quadrant.None;
            }

            if (quadrantPosition.X < 0 && quadrantPosition.Y < 0)
            {
                return Quadrant.TopLeft;
            }

            if (quadrantPosition.X > 0 && quadrantPosition.Y < 0)
            {
                return Quadrant.TopRight;
            }

            if (quadrantPosition.X < 0 && quadrantPosition.Y > 0)
            {
                return Quadrant.BottomLeft;
            }
            
            return Quadrant.BottomRight;
        }
    }
    
    public class RobotHandler
    {
        private readonly int _height;
        private readonly int _width;
        private readonly List<Robot> _robots = new List<Robot>();
        
        public RobotHandler(string filename, int height, int width)
        {
            _height = height;
            _width = width;
            var sr = new StreamReader(
                Path.Combine(Environment.CurrentDirectory, filename));
            
            var line = sr.ReadLine();
            
            while (line != null)
            {
                var parts = line.Split(' ');
                var positionCoordinate = TurnStringIntoCoordinate(parts[0]);
                var velocity = TurnStringIntoCoordinate(parts[1]);
                
                _robots.Add(new Robot(positionCoordinate, velocity));
                
                line = sr.ReadLine();
            }
        }

        private Coordinate TurnStringIntoCoordinate(string input)
        {
            var str = input.Replace("=", "");
            str = str.Replace("v", "");
            str = str.Replace("p", "");
            
            var parts = str.Split(',');

            return new Coordinate(int.Parse(parts[0]), int.Parse(parts[1]));
        }

        private void PrintRobotMap(int i)
        {
            Console.WriteLine($"AFTER {i} SECONDS");
            var str = string.Empty;
            var coord = new Coordinate(0, 0);
            var hasChristmasTree = false;
            var treeStringIndex = 0;
            
            var treeStrings = new List<string>()
            {
                "..#..",
                ".###.",
                "#####"
            };
            
            var line = string.Empty;
            
            for (var y = 0; y < _height; y++)
            {
                coord.Y = y;
                for (var x = 0; x < _width; x++)
                {
                    coord.X = x;

                    line += IsRobotPresentAtCoordinate(coord) ? "#" : ".";
                }

                str += line + "\n";

                if (!hasChristmasTree)
                {
                    if (line.Contains(treeStrings[treeStringIndex]))
                    {
                        treeStringIndex++;

                        if (treeStringIndex == treeStrings.Count)
                        {
                            hasChristmasTree = true;
                        }
                    }
                    else
                    {
                        treeStringIndex = 0;
                    }
                }
                
                line = string.Empty;
            }

            if (hasChristmasTree)
            {
                Console.Write(str);
            }
        }

        private bool IsRobotPresentAtCoordinate(Coordinate coord)
        {
            return _robots.Any(robot => robot.CurrentPosition.Equals(coord));
        }

        public int GetSafetyScore(int seconds)
        {
            Console.WriteLine($"ProcessingRobots: {_robots.Count}");
            for (var i = 1; i <= seconds; i++)
            {
                foreach (var robot in _robots)
                {
                    robot.GetEndPositionAfterSeconds(1, _height, _width);
                }

                
                PrintRobotMap(i);
            }
            
            var quadrantRobots = new Dictionary<Quadrant, int>();
            var midPoint = new Coordinate(_width/2, _height/2);
            
            foreach (var robot in _robots)
            {
                var quadrant = robot.GetQuadrant(midPoint);

                if (quadrant == Quadrant.None)
                {
                    continue;
                }

                if (quadrantRobots.ContainsKey(quadrant))
                {
                    quadrantRobots[quadrant]++;
                }
                else
                {
                    quadrantRobots.Add(quadrant, 1);
                }
            }

            var result = 1;
            
            foreach (var keyValuePair in quadrantRobots)
            {
                Console.WriteLine($"{keyValuePair.Key}: {keyValuePair.Value}");
                result *= keyValuePair.Value;
            }

            return result;
        }
    }
}