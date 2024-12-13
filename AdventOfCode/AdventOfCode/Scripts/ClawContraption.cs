using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode
{
    public class ClawMachine
    {
        private LongCoordinate _prizeCoordinate;
        private readonly LongCoordinate _buttonA;
        private readonly LongCoordinate _buttonB;

        public ClawMachine(LongCoordinate buttonA, LongCoordinate buttonB, LongCoordinate prizeCoordinate)
        {
            _prizeCoordinate = prizeCoordinate;
            _buttonA = buttonA;
            _buttonB = buttonB;
        }

        public void AmplifyPrizeCoordinate(long coeff)
        {
            _prizeCoordinate *= coeff;
        }

        public long GetMinimumTokensToGetPrize()
        {
            /*if (!IsPossible())
            {
                return 0;
            }*/

            var a = -1;
            var smallestCombo = long.MaxValue;

            while (_prizeCoordinate.X > _buttonA.X * a &&
                   _prizeCoordinate.Y > _buttonA.Y * a)
            {
                a++;
                var b = -1;
                while (_prizeCoordinate.X > _buttonB.X * b &&
                       _prizeCoordinate.Y > _buttonB.Y * b)
                {
                    b++;
                    if (((_buttonA * a) + (_buttonB * b)).Equals(_prizeCoordinate))
                    {
                        var combo = (a * 3) + b;
                        if (combo < smallestCombo)
                        {
                            smallestCombo = combo;
                        }
                    }
                }
            }

            return smallestCombo < long.MaxValue ? smallestCombo : 0;
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
            LongCoordinate buttonA = null;
            LongCoordinate buttonB = null;

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

        private LongCoordinate StringToCoordinate(string line)
        {
            line = line.Replace(" ", "");
            line = line.Replace("+", "");
            line = line.Replace("=", "");
            line = line.Replace("X", "");
            line = line.Replace("Y", "");
            
            var parts = line.Split(',');
            
            return new LongCoordinate(long.Parse(parts[0]), long.Parse(parts[1]));
        }

        public long GetMinimumTokensForAllPossiblePrizes()
        {
            long result = 0;

            Console.WriteLine("GetMinimumTokensForAllPossiblePrizes:");
            var index = 0;
            foreach (var clawMachine in _clawMachines)
            {
                result += clawMachine.GetMinimumTokensToGetPrize();
                Console.WriteLine($"{++index}/{_clawMachines.Count}");
            }
            
            return result;
        }

        public void AmplifyAllPrizeCoords(long coeff)
        {
            foreach (var clawMachine in _clawMachines)
            {
                clawMachine.AmplifyPrizeCoordinate(coeff);
            }
        }
    }
}