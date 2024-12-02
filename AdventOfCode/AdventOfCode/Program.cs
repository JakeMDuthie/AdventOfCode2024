using System;

namespace AdventOfCode
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var reactorAnalyser = new ReactorAnalyser(@"..\..\Data\redNosedReactor.txt");
            
            Console.WriteLine("Day 2: Red-Nosed Reports");
            Console.WriteLine($"SOLUTION pt 1 = {reactorAnalyser.GetSafeReports()}");
            Console.WriteLine($"SOLUTION pt 2 = {reactorAnalyser.GetSafeReportsWithinTolerance()}");
        }

        private static void Day1()
        {
            Console.WriteLine("Day 1: Historian Hysteria");
            var idAnalyser = new IdAnalyser(@"..\..\Data\historianHysteria.txt");
            Console.WriteLine($"SOLUTION pt 1 = {idAnalyser.GetTotalDifferenceBetweenLists()}");
            Console.WriteLine($"SOLUTION pt 2 = {idAnalyser.GetSimilarityScoreTotal()}");
        }
    }
}