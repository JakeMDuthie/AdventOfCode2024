using System;

namespace AdventOfCode
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Day 15: Warehouse Woes");
            var warehouseNavigator = new WarehouseNavigator(@"..\..\Data\warehouseWoesSmall.txt");
            warehouseNavigator.ProcessRobotMoves();
            Console.WriteLine($"SOLUTION pt 1 = {warehouseNavigator.GetBoxCoordinateSum()}");
            var largeWarehouseNavigator = new LargeWarehouseNavigator(@"..\..\Data\warehouseWoesMedium.txt");
            largeWarehouseNavigator.ProcessRobotMoves();
            Console.WriteLine($"\nSOLUTION pt 2 = {largeWarehouseNavigator.GetBoxCoordinateSum()}");
        }

        private static void Day14()
        {
            Console.WriteLine("Day 14: Restroom Redoubt");
            var robotHandler = new RobotHandler(@"..\..\Data\restroomRedoubt.txt", 103, 101);
            Console.WriteLine($"SOLUTION pt 1 = {robotHandler.GetSafetyScore(10403)}");
        }

        private static void Day13()
        {
            Console.WriteLine("Day 13: Claw Contraption");
            var clawContraption = new ClawContraption(@"..\..\Data\clawContraption.txt");
            Console.WriteLine($"SOLUTION pt 1 = {clawContraption.GetMinimumTokensForAllPossiblePrizes()}");
            clawContraption.AmplifyAllPrizeCoords(10000000000000);
            Console.WriteLine($"SOLUTION pt 2 = {clawContraption.GetMinimumTokensForAllPossiblePrizes()}");
        }

        private static void Day12()
        {
            Console.WriteLine("Day 12: Garden Groups");
            var gardenGroups = new GardenGroups(@"..\..\Data\gardenGroups.txt");
            gardenGroups.CalculateRegionsAndExposedEdges();
            Console.WriteLine($"SOLUTION pt 1 = {gardenGroups.GetFenceCost()}");
            Console.WriteLine($"SOLUTION pt 2 = {gardenGroups.GetFenceCostBasedOnSides()}");
        }

        private static void Day11()
        {
            Console.WriteLine("Day 11: Plutonian Pebbles");
            var numberProcessor = new PlutonianNumberProcessor(@"..\..\Data\numberStones.txt");
            numberProcessor.HandleBlinks(25);
            Console.WriteLine($"SOLUTION pt 1 = Stones after blinking {numberProcessor.StoneCount}");
            numberProcessor.HandleBlinks(75);
            Console.WriteLine($"SOLUTION pt 2 = Stones after blinking {numberProcessor.StoneCount}");
        }

        private static void Day10()
        {
            Console.WriteLine("Day 10: Hoof It");
            var trailFinder = new TrailFinder(@"..\..\Data\trailMap.txt");
            trailFinder.MapTrailheads();
            Console.WriteLine($"SOLUTION pt 1 = {trailFinder.GetTotalTrailheadScores()}");
            trailFinder.MapTrailheadsWithDupes();
            Console.WriteLine($"SOLUTION pt 2 = {trailFinder.GetTotalTrailheadScoresWithDupes()}");
        }

        private static void Day9()
        {
            Console.WriteLine("Day 9: Disk Fragmenter");
            var diskDefragmenter = new DiskDefragmenter(@"..\..\Data\diskDefragmentSmall.txt");
            //Console.WriteLine(diskDefragmenter.ToString());
            diskDefragmenter.Defragment();
            //Console.WriteLine(diskDefragmenter.ToString());
            Console.WriteLine($"SOLUTION pt 1 = {diskDefragmenter.GetChecksum()}");
            diskDefragmenter = new DiskDefragmenter(@"..\..\Data\diskDefragment.txt");
            //Console.WriteLine("Before: \n");
            //Console.WriteLine(diskDefragmenter.ToString());
            diskDefragmenter.DefragmentBySection();
            //Console.WriteLine("After: \n");
            //Console.WriteLine(diskDefragmenter.ToString());
            Console.WriteLine($"SOLUTION pt 2 = {diskDefragmenter.GetChecksum()}");
        }

        private static void Day8()
        {
            Console.WriteLine("Day 8: Resonant Collinearity");
            var antennaMap = new AntennaMap(@"..\..\Data\antennaMap.txt");
            antennaMap.GenerateAntinodes();
            Console.WriteLine($"SOLUTION pt 1 = {antennaMap.GetUniqueAntinodes()}");
            antennaMap.GenerateAntinodesWithResonance();
            Console.WriteLine($"SOLUTION pt 2 = {antennaMap.GetUniqueAntinodes()}");
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