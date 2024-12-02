using System;

namespace AdventOfCode
{
    public class ReactorReport
    {
        private const int k_safetyTolerance = 3;
        private readonly int[] _levels;

        public ReactorReport(string line)
        {
            var levels = line.Split(' ');
            _levels = new int[levels.Length];
            for (var index = 0; index < levels.Length; index++)
            {
                _levels[index] = int.Parse(levels[index]);
            }
        }

        public bool IsSafe()
        {
            if (!AllAscending() && !AllDescending())
            {
                return false;
            }
            
            return GapsWithinTolerance();
        }

        private bool GapsWithinTolerance()
        {
            for (var i = 0; i < _levels.Length-1; i++)
            {
                if (Math.Abs(_levels[i] - _levels[i + 1]) > k_safetyTolerance)
                {
                    return false;
                }
            }
            
            return true;
        }

        private bool AllDescending()
        {
            for (var i = 0; i < _levels.Length-1; i++)
            {
                if (_levels[i] <= _levels[i + 1])
                {
                    return false;
                }
            }
            
            return true;
        }
        
        private bool AllAscending()
        {
            for (var i = 0; i < _levels.Length-1; i++)
            {
                if (_levels[i] >= _levels[i + 1])
                {
                    return false;
                }
            }
            
            return true;
        }
    }
}