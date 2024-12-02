using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public class IdAnalyser
    {
        private readonly string _filename;
        private List<int> _firstList;
        private List<int> _secondList;

        public IdAnalyser(string filename)
        {
            _filename = filename;
        }

        public int GetTotalDifferenceBetweenLists()
        {
            GetAndSortLists();
            var totalDiff = 0;

            for (int i = 0; i < _firstList.Count; i++)
            {
                var diff = Math.Abs(_firstList[i] - _secondList[i]);
                Console.WriteLine($"DIFF = {diff}");
                totalDiff += diff;
            }
            
            Console.WriteLine($"Total difference = {totalDiff}");
            return totalDiff;
        }

        public int GetSimilarityScoreTotal()
        {
            var totalSimilarity = 0;
            var dictionary = new Dictionary<int, int>();
            foreach (var id in _firstList)
            {
                // we already know this value
                if (dictionary.ContainsKey(id))
                {
                    totalSimilarity += dictionary[id] * id;
                }
                else
                {
                    var appearances = _secondList.Count(x => x == id);  
                    dictionary.Add(id, appearances);
                    totalSimilarity += (appearances * id);
                }
            }
            
            return totalSimilarity;
        }

        private void GetAndSortLists()
        {
            var sr = new StreamReader(
                Path.Combine(Environment.CurrentDirectory, _filename));
            
            var line = sr.ReadLine();

            _firstList = new List<int>();
            _secondList = new List<int>();
            
            while (line != null)
            {
                var numbers = line.Split(' ');
                _firstList.Add(int.Parse(numbers[0]));
                _secondList.Add(int.Parse(numbers[numbers.Length - 1]));
                
                line = sr.ReadLine();
            }
            
            _firstList.Sort();
            _secondList.Sort();
        }
    }
}