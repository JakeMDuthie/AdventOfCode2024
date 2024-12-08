using System;

namespace AdventOfCode
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Day 8: Resonant Collinearity");
            var antennaMap = new AntennaMap(@"..\..\Data\antennaMapSmall.txt");
            antennaMap.GenerateAntinodes();
            Console.WriteLine($"SOLUTION pt 1 = {antennaMap.GetUniqueAntinodes()}");
        }

        private static void Day7()
        {
            Console.WriteLine("Day 7: Bridge Repair");
            var operatorCalibrator = new OperatorCalibrator(@"..\..\Data\bridgeRepair.txt");
            Console.WriteLine($"SOLUTION pt 1 = {operatorCalibrator.GetTotalCalibrationResult(false)}");
            Console.WriteLine($"SOLUTION pt 2 = {operatorCalibrator.GetTotalCalibrationResult(true)}");
        }

        private static void Day6()
        {
            Console.WriteLine("Day 6: Guard Gallivant");
            var guardMap = new GuardMap(@"..\..\Data\guardMap.txt");
            guardMap.ProcessWalk();
            Console.WriteLine($"SOLUTION pt 1 = {guardMap.GetUniqueCellsVisited()}");
            Console.WriteLine($"SOLUTION pt 2 = {guardMap.GetCellsThatCanBecomeBlockersToIntroduceLoops()}");
        }

        private static void Day5()
        {
            Console.WriteLine("Day 5: Print Queue");
            var pageOrderer = new PageOrderer(@"..\..\Data\printQueue.txt");
            Console.WriteLine($"SOLUTION pt 1 = {pageOrderer.GetTotalValueOfMiddlePagesForPrintCommandsThatSatisfyRules()}");
            Console.WriteLine($"SOLUTION pt 2 = {pageOrderer.GetTotalValueOfMiddlePagesForPrintCommandsThatNeedToBeReorderedAfterReorderingThem()}");
        }

        private static void Day4()
        {
            Console.WriteLine("Day 4: Ceres Search");
            var wordSearcher = new WordSearcher(@"..\..\Data\wordSearch.txt");
            Console.WriteLine($"SOLUTION pt 1 = {wordSearcher.GetOccurrencesOfWord("XMAS")}");
            Console.WriteLine($"SOLUTION pt 2 = {wordSearcher.XmasOccurrences()}");
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