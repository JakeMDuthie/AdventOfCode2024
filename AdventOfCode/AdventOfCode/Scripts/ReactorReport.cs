using System;
using System.Linq;

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

        public bool IsSafeWithinThreshold()
        {
            if (IsSafe())
            {
                return true;
            }
            
            return IsSafeWithOneRemoved();
        }

        private bool IsSafeWithOneRemoved()
        {
            for (int i = 0; i < _levels.Length; i++)
            {
                var list = _levels.ToList();
                list.RemoveAt(i);
                
                var tempList = list.ToArray();
                if (AreLevelsSafe(tempList))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsSafe()
        {
            return AreLevelsSafe(_levels);
        }

        private bool AreLevelsSafe(int[] levels)
        {
            if (!AllAscending(levels) && !AllDescending(levels))
            {
                return false;
            }

            return GapsWithinTolerance(levels);
        }

        private bool GapsWithinTolerance(int[] levels)
        {
            for (var i = 0; i < levels.Length-1; i++)
            {
                if (Math.Abs(levels[i] - levels[i + 1]) > k_safetyTolerance)
                {
                    return false;
                }
            }

            return true;
        }

        private bool AllDescending(int[] levels)
        {
            for (var i = 0; i < levels.Length-1; i++)
            {
                if (levels[i] <= levels[i + 1])
                {
                    return false;
                }
            }
            
            return true;
        }
        
        private bool AllAscending(int[] levels)
        {
            for (var i = 0; i < levels.Length-1; i++)
            {
                if (levels[i] >= levels[i + 1])
                {
                    return false;
                }
            }
            
            return true;
        }
    }
}