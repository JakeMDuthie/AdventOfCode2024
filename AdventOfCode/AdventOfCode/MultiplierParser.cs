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
            private readonly int[] terms;
            
            public Multiplier(string input)
            {
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

        private readonly List<Multiplier> _multipliers;
        
        public MultiplierParser(string filename)
        {
            _multipliers = new List<Multiplier>();
            InitialiseMultipliersFromFile(filename);
        }

        private void InitialiseMultipliersFromFile(string filename)
        {
            var sr = new StreamReader(
                Path.Combine(Environment.CurrentDirectory, filename));

            var regex = @"mul\(\d{1,3}\,\d{1,3}\)";
            var data = sr.ReadToEnd();
            
            var matches = Regex.Matches(data, regex);

            foreach (Match match in matches)
            {
                _multipliers.Add(new Multiplier(match.Value));
                Console.WriteLine(match.Value);
            }
        }

        public int GetValue()
        {
            var value = 0;

            foreach (var multiplier in _multipliers)
            {
                value += multiplier.GetValue();
            }
            
            return value;
        }
    }
}