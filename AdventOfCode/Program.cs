﻿namespace AdventOfCode
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string inputFolder = "PuzzleInputs\\";

            // #1
            Console.WriteLine("#1");
            string callories = File.ReadAllText(inputFolder + "1.txt");
            Console.WriteLine("Max callories: " + Day1.MaxCallories(callories));
            Console.WriteLine("Top Three Max Callories: " + Day1.TopThreeMaxCallories(callories) + "\n");

            // #2
            Console.WriteLine("#2");
            string strategy = File.ReadAllText(inputFolder + "2.txt");
            Console.WriteLine("Total Points from strategy: " + Day2.RockPaperScissorsTotal(strategy));
            Console.WriteLine("Total Points from strategy (part 2): " + Day2.RockPaperScissorsTotal2(strategy) + "\n");

            // #3
            Console.WriteLine("#3");
            string rucksacks = File.ReadAllText(inputFolder + "3.txt");
            Console.WriteLine("Rucksacks Total Item Priority: " + Day3.RucksackSum(rucksacks));
            Console.WriteLine("Rucksacks Total Item Priority (part 2): " + Day3.RucksackSum2(rucksacks) + "\n");

            // #4
            Console.WriteLine("#4");
            string pairs = File.ReadAllText(inputFolder + "4.txt");
            Console.WriteLine("Overlaping pairs: " + Day4.Overlaps(pairs));
            Console.WriteLine("Overlaping pairs (part 2): " + Day4.Overlaps2(pairs) + "\n");

            // #5
            Console.WriteLine("#5");
            string initConfig = File.ReadAllText(inputFolder + "5.txt");
            Console.WriteLine("Stack Tops: " + Day5.CrateRearangement(initConfig));
            Console.WriteLine("Stack Tops (part 2): " + Day5.CrateRearangement2(initConfig) + "\n");

            // #6
            Console.WriteLine("#6");
            string data = File.ReadAllText(inputFolder + "6.txt");
            Console.WriteLine("Marker Position: " + Day6.GetMarkerPosition(data, 4));
            Console.WriteLine("Marker Position (part 2): " + Day6.GetMarkerPosition(data, 14) + "\n");

            // #7
            Console.WriteLine("#7");
            string console = File.ReadAllText(inputFolder + "7.txt");
            Console.WriteLine("Dirs with max size (100000): " + Day7.DirectoriesToDelete(console, 100000));
            Console.WriteLine("Smallest dir to delete: " + Day7.SmallestDirectoryToDelete(console, 70000000, 30000000) + "\n");

            // #8
            Console.WriteLine("#8");
            string treeMap = File.ReadAllText(inputFolder + "8.txt");
            Console.WriteLine("Visible trees: " + Day8.GetVisibleTrees(treeMap));
            Console.WriteLine("Scenic score: " + Day8.GetScenicScore(treeMap));

            // #9
            Console.WriteLine("#9");
            string headMovement = File.ReadAllText(inputFolder + "9.txt");
            Console.WriteLine("Head visited positions: " + Day9.GetTailVisitedPositions(headMovement, 2));
            Console.WriteLine("Head visited positions (part 2): " + Day9.GetTailVisitedPositions(headMovement, 10) + "\n");

            // #10
            Console.WriteLine("#10");
            string code = File.ReadAllText(inputFolder + "10.txt");
            Console.WriteLine("Signal strength: " + Day10.GetSignalStrength(code, 20, 40, 220));
            Console.WriteLine(Day10.GetScreen(code) + "\n");

            // #11
            Console.WriteLine("#11");
            string monkeysAttributes = File.ReadAllText(inputFolder + "11.txt");
            Console.WriteLine("Monkey Business: " + Day11.GetMonkeyBusiness(monkeysAttributes, 20, true));
            Console.WriteLine("Monkey Business (part 2): " + Day11.GetMonkeyBusiness(monkeysAttributes, 10000, false));

            // #12
            Console.WriteLine("#12");
            string map = File.ReadAllText(inputFolder + "12.txt");
            Console.WriteLine("Shortest path to top: " + Day12.GetShortestPathToTop(map));
            Console.WriteLine("Shortest path to top (part 2): " + Day12.GetShortestPathToTopFromGround(map));

            // #13
            Console.WriteLine("#13");
            string packets = File.ReadAllText(inputFolder + "13.txt");
            Console.WriteLine("Wrong packets order indices: " + Day13.GetWrongOrderIndices(packets));
            Console.WriteLine("Decoder Key: " + Day13.GetDecoderKey(packets));

            // #14
            Console.WriteLine("#14");
            string cave = File.ReadAllText(inputFolder + "14.txt");
            Console.WriteLine("Sand units at rest: " + Day14.GetUnitsOfSandAtRest(cave));
        }
    }
}