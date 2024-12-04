using System;

namespace AdventOfCode
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Day 4: Ceres Search");
            var wordSearcher = new WordSearcher(@"..\..\Data\wordSearch.txt");
            Console.WriteLine($"SOLUTION pt 1 = {wordSearcher.GetOccurrencesOfWord("XMAS")}");
        }

        private static void Day3()
        {
            Console.WriteLine("Day 3: Mull It Over");
            var multiplierParser = new MultiplierParser(@"..\..\Data\mullItOver.txt");
            Console.WriteLine($"SOLUTION pt 1 = {multiplierParser.GetValue()}");
            Console.WriteLine($"SOLUTION pt 2 = {multiplierParser.GetValueRespectingDoDonts()}");
        }

        private static void Day2()
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