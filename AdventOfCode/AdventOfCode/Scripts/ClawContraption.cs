using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode
{
    public class ClawMachine
    {
        private readonly Coordinate _prizeCoordinate;
        private readonly Coordinate _buttonA;
        private readonly Coordinate _buttonB;

        public ClawMachine(Coordinate buttonA, Coordinate buttonB, Coordinate prizeCoordinate)
        {
            _prizeCoordinate = prizeCoordinate;
            _buttonA = buttonA;
            _buttonB = buttonB;
        }

        public int GetMinimumTokensToGetPrize()
        {
            if (!IsPossible())
            {
                return 0;
            }

            var possibleCombos = new List<int>();
            var a = -1;
            var b = -1;

            while (_prizeCoordinate.X > _buttonA.X * a &&
                   _prizeCoordinate.Y > _buttonA.Y * a)
            {
                a++;
                b = -1;
                while (_prizeCoordinate.X > _buttonB.X * b &&
                       _prizeCoordinate.Y > _buttonB.Y * b)
                {
                    b++;
                    if (((_buttonA * a) + (_buttonB * b)).Equals(_prizeCoordinate))
                    {
                        possibleCombos.Add((a*3) + b);
                    }
                }
            }
            
            possibleCombos.Sort();
            
            return possibleCombos.Count > 0 ? possibleCombos[0] : 0;
        }

        private bool IsPossible()
        {
            // if either axis requires more than 100 button presses for both buttons, then this isn't possible
            if (_prizeCoordinate.X / (_buttonA.X + _buttonB.X) > 100 && 
                _prizeCoordinate.Y / (_buttonA.Y + _buttonB.Y) > 100)
            {
                return false;
            }
            
            return true;
        }
    }
    
    public class ClawContraption
    {
        private readonly List<ClawMachine> _clawMachines = new List<ClawMachine>();
        
        public ClawContraption(string filename)
        {
            var sr = new StreamReader(
                Path.Combine(Environment.CurrentDirectory, filename));
            
            var line = sr.ReadLine();
            Coordinate buttonA = null;
            Coordinate buttonB = null;

            while (line != null)
            {
                if (line.Contains("Button A:"))
                {
                    line = line.Replace("Button A:", "");
                    buttonA = StringToCoordinate(line);
                }
                else if (line.Contains("Button B:"))
                {
                    line = line.Replace("Button B:", "");
                    buttonB = StringToCoordinate(line);
                }
                else if (line.Contains("Prize:"))
                {
                    line = line.Replace("Prize:", "");
                    var prizeCoord = StringToCoordinate(line);
                    _clawMachines.Add(new ClawMachine(buttonA, buttonB, prizeCoord));
                }
                
                line = sr.ReadLine();
            }
            
            Console.WriteLine($"Claw Machine: {_clawMachines.Count}");
        }

        private Coordinate StringToCoordinate(string line)
        {
            line = line.Replace(" ", "");
            line = line.Replace("+", "");
            line = line.Replace("=", "");
            line = line.Replace("X", "");
            line = line.Replace("Y", "");
            
            var parts = line.Split(',');
            
            return new Coordinate(int.Parse(parts[0]), int.Parse(parts[1]));
        }

        public int GetMinimumTokensForAllPossiblePrizes()
        {
            var result = 0;

            foreach (var clawMachine in _clawMachines)
            {
                result += clawMachine.GetMinimumTokensToGetPrize();
            }
            
            return result;
        }
    }
}