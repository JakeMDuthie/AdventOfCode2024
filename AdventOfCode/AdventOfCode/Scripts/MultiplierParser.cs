using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class MultiplierParser
    {
        private class Multiplier
        {
            public int Index { get; private set; }
            private readonly int[] terms;
            
            public Multiplier(string input, int index)
            {
                Index = index;
                var regex = @"\d{1,3}";
                var matches = Regex.Matches(input, regex);
                
                terms = new int[matches.Count];
                
                for (var i = 0; i < matches.Count; i++)
                {
                    terms[i] = int.Parse(matches[i].Value);
                }
            }

            public int GetValue()
            {
                return terms[0] * terms[1];
            }
        }

        private readonly Dictionary<int,Multiplier> _multipliers;
        private readonly Dictionary<int, bool> _enablerIndices;

        private int _maxMultiplierIndex;
        
        public MultiplierParser(string filename)
        {
            _multipliers = new Dictionary<int,Multiplier>();
            _enablerIndices = new Dictionary<int, bool>();
            InitialiseMultipliersFromFile(filename);
        }

        private void InitialiseMultipliersFromFile(string filename)
        {
            var sr = new StreamReader(
                Path.Combine(Environment.CurrentDirectory, filename));

            var regex = @"mul\(\d{1,3}\,\d{1,3}\)";
            var doDontregex = @"do\(\)|don't\(\)";
            var data = sr.ReadToEnd();
            
            var matches = Regex.Matches(data, regex);

            foreach (Match match in matches)
            {
                _multipliers.Add(match.Index,new Multiplier(match.Value, match.Index));
                _maxMultiplierIndex = match.Index;
                Console.WriteLine(match.Value);
            }
            
            var doDonts = Regex.Matches(data, doDontregex);

            foreach (Match match in doDonts)
            {
                _enablerIndices.Add(match.Index, match.Value == "do()");
                Console.WriteLine(match.Value);
            }
        }

        public int GetValue()
        {
            var value = 0;

            foreach (var multiplier in _multipliers.Values)
            {
                value += multiplier.GetValue();
            }
            
            return value;
        }

        public int GetValueRespectingDoDonts()
        {
            var value = 0;
            var enabled = true;

            for (var index = 0; index <= _maxMultiplierIndex; index++)
            {
                if (_enablerIndices.TryGetValue(index, out var enablerIndex))
                {
                    enabled = enablerIndex;
                }

                if (!enabled)
                {
                    continue;
                }
                
                if (_multipliers.TryGetValue(index, out var multiplier))
                {
                    value += multiplier.GetValue();
                }
            }
            
            return value;
        }
    }
}