using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public static class LongExtensions
    {
        // a theoretical quicker solution that works for Day 7 pt 2's small data, but not for the actual data
        public static long Concat(this long a, long b)
        {
            if (b == 1)
            {
                // for some reason, if b == 1- this works but the normal method doesn't?
                return a * 10 + b;
            }
            
            long pow = 1;

            while (pow < b)
            {
                pow = ((pow << 2) + pow) << 1;
                a = ((a << 2) + a) << 1;
            }

            return a + b;
        }
    }
    
    public class OperatorCalibrator
    {
        private class Equation
        {
            public long Target { get; }

            private readonly List<long> _operators;
            
            private bool _alreadyKnownToSatisfy;

            public Equation(long target, List<long> operators)
            {
                Target = target;
                _operators = operators;
            }

            public override string ToString()
            {
                var operatorString = string.Empty;
                foreach (var @operator in _operators)
                {
                    operatorString += " " + @operator;
                }
                return $"{Target}:" + operatorString;
            }

            public bool Satisfies(bool withConcatenation)
            {
                if (_alreadyKnownToSatisfy)
                {
                    return true;
                }
                
                var startNums = new List<long> {_operators[0]};
                var results = new List<long>();

                for (var i = 1; i < _operators.Count; i++)
                {
                    var nextNum = _operators[i];

                    foreach (var startNum in startNums)
                    {
                        results.Add(startNum + nextNum);
                        results.Add(startNum * nextNum);

                        if (withConcatenation)
                        {
                            results.Add(startNum.Concat(nextNum));
                        }
                    }

                    if (i == _operators.Count - 1)
                    {
                        break;
                    }
                    
                    startNums.Clear();
                    startNums.AddRange(results);
                    results.Clear();
                }

                var satisfies = results.Any(r => r == Target);
                _alreadyKnownToSatisfy = satisfies;
                return satisfies;
            }
        }
        
        private readonly List<Equation> _equations = new List<Equation>();
        
        public OperatorCalibrator(string filename)
        {
            var sr = new StreamReader(
                Path.Combine(Environment.CurrentDirectory, filename));
            
            var line = sr.ReadLine();
            
            while (line != null)
            {
                AddEquationToListFromString(line);
                line = sr.ReadLine();
            }
        }

        private void AddEquationToListFromString(string input)
        {
            var parts = input.Split(':');
            var target = long.Parse(parts[0]);
            var operatorStrings = parts[1].Split(' ');
            var operators = new List<long>();
            
            foreach (var operatorString in operatorStrings)
            {
                if (!long.TryParse(operatorString, out var op))
                {
                    continue;
                }
                
                operators.Add(op);
            }
            
            _equations.Add(new Equation(target, operators));
        }

        public long GetTotalCalibrationResult(bool withConcatenation)
        {
            long returnValue = 0;

            for (var index = 0; index < _equations.Count; index++)
            {
                var equation = _equations[index];
                var satisfies = equation.Satisfies(withConcatenation);
                if (satisfies)
                {
                    returnValue += equation.Target;
                }
                Console.WriteLine($"{index+1} / {_equations.Count} = {satisfies}");
            }

            return returnValue;
        }
    }
}