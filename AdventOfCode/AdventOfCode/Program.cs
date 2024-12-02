using System;

namespace AdventOfCode
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var idAnalyser = new IdAnalyser("..\\..\\Data\\historianHysteria.txt");
            Console.WriteLine($"SOLUTION = {idAnalyser.GetTotalDifferenceBetweenLists()}");
            Console.WriteLine($"SOLUTION pt 2 = {idAnalyser.GetSimilarityScoreTotal()}");
        }
    }
}