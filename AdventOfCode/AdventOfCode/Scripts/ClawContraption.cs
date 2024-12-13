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
            _prizeCoordinate += new LongCoordinate(coeff, coeff);
        }

        public long GetMinimumTokensToGetPrize()
        {
            var bVal = B_Value();
            if (bVal > 0)
            {
                var aVal = A_Value(bVal);
                if (aVal > 0)
                {
                    return (aVal*3) + bVal;
                }
            }
            
            return 0;
        }

        private long B_Value()
        {
            if (((_buttonA.X * _prizeCoordinate.Y) - (_buttonA.Y * _prizeCoordinate.X)) %
                ((_buttonA.X * _buttonB.Y) - (_buttonA.Y * _buttonB.X)) != 0)
            {
                return -1;
            }
            
            return ((_buttonA.X*_prizeCoordinate.Y)-(_buttonA.Y*_prizeCoordinate.X))
                   /
                   ((_buttonA.X*_buttonB.Y)-(_buttonA.Y*_buttonB.X));
        }

        private long A_Value(long b)
        {
            if (((_prizeCoordinate.X) - (_buttonB.X * b)) %
                (_buttonA.X) != 0)
            {
                return -1;
            }

            return ((_prizeCoordinate.X) - (_buttonB.X * b)) /
                   (_buttonA.X);
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