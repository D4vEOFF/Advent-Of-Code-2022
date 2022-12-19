using AdventOfCode.DaysOfCode;

namespace AdventOfCode
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int makePart = 1;
            try
            {
                Console.Write("Day number: ");
                makePart = int.Parse(Console.ReadLine());
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
            }

            string inputFolder = "PuzzleInputs\\";

            switch (makePart)
            {
                case 1:
                    string callories = File.ReadAllText(inputFolder + "1.txt");
                    Console.WriteLine("Max callories: " + Day1.MaxCallories(callories));
                    Console.WriteLine("Top Three Max Callories: " + Day1.TopThreeMaxCallories(callories) + "\n");
                    break;
                case 2:
                    string strategy = File.ReadAllText(inputFolder + "2.txt");
                    Console.WriteLine("Total Points from strategy: " + Day2.RockPaperScissorsTotal(strategy));
                    Console.WriteLine("Total Points from strategy (part 2): " + Day2.RockPaperScissorsTotal2(strategy) + "\n");
                    break;
                case 3:
                    string rucksacks = File.ReadAllText(inputFolder + "3.txt");
                    Console.WriteLine("Rucksacks Total Item Priority: " + Day3.RucksackSum(rucksacks));
                    Console.WriteLine("Rucksacks Total Item Priority (part 2): " + Day3.RucksackSum2(rucksacks) + "\n");
                    break;
                case 4:
                    string pairs = File.ReadAllText(inputFolder + "4.txt");
                    Console.WriteLine("Overlaping pairs: " + Day4.Overlaps(pairs));
                    Console.WriteLine("Overlaping pairs (part 2): " + Day4.Overlaps2(pairs) + "\n");
                    break;
                case 5:
                    string initConfig = File.ReadAllText(inputFolder + "5.txt");
                    Console.WriteLine("Stack Tops: " + Day5.CrateRearangement(initConfig));
                    Console.WriteLine("Stack Tops (part 2): " + Day5.CrateRearangement2(initConfig) + "\n");
                    break;
                case 6:
                    string data = File.ReadAllText(inputFolder + "6.txt");
                    Console.WriteLine("Marker Position: " + Day6.GetMarkerPosition(data, 4));
                    Console.WriteLine("Marker Position (part 2): " + Day6.GetMarkerPosition(data, 14) + "\n");
                    break;
                case 7:
                    string console = File.ReadAllText(inputFolder + "7.txt");
                    Console.WriteLine("Dirs with max size (100000): " + Day7.DirectoriesToDelete(console, 100000));
                    Console.WriteLine("Smallest dir to delete: " + Day7.SmallestDirectoryToDelete(console, 70000000, 30000000) + "\n");
                    break;
                case 8:
                    string treeMap = File.ReadAllText(inputFolder + "8.txt");
                    Console.WriteLine("Visible trees: " + Day8.GetVisibleTrees(treeMap));
                    Console.WriteLine("Scenic score: " + Day8.GetScenicScore(treeMap));
                    break;
                case 9:
                    string headMovement = File.ReadAllText(inputFolder + "9.txt");
                    Console.WriteLine("Head visited positions: " + Day9.GetTailVisitedPositions(headMovement, 2));
                    Console.WriteLine("Head visited positions (part 2): " + Day9.GetTailVisitedPositions(headMovement, 10) + "\n");
                    break;
                case 10:
                    string code = File.ReadAllText(inputFolder + "10.txt");
                    Console.WriteLine("Signal strength: " + Day10.GetSignalStrength(code, 20, 40, 220));
                    Console.WriteLine(Day10.GetScreen(code) + "\n");
                    break;
                case 11:
                    string monkeysAttributes = File.ReadAllText(inputFolder + "11.txt");
                    Console.WriteLine("Monkey Business: " + Day11.GetMonkeyBusiness(monkeysAttributes, 20, true));
                    Console.WriteLine("Monkey Business (part 2): " + Day11.GetMonkeyBusiness(monkeysAttributes, 10000, false));
                    break;
                case 12:
                    string map = File.ReadAllText(inputFolder + "12.txt");
                    Console.WriteLine("Shortest path to top: " + Day12.GetShortestPathToTop(map));
                    Console.WriteLine("Shortest path to top (part 2): " + Day12.GetShortestPathToTopFromGround(map));
                    break;
                case 13:
                    string packets = File.ReadAllText(inputFolder + "13.txt");
                    Console.WriteLine("Wrong packets order indices: " + Day13.GetWrongOrderIndices(packets));
                    Console.WriteLine("Decoder Key: " + Day13.GetDecoderKey(packets));
                    break;
                case 14:
                    string cave = File.ReadAllText(inputFolder + "14.txt");
                    Console.WriteLine("Sand units at rest: " + Day14.GetUnitsOfSandAtRest(cave, false));
                    Console.WriteLine("Sand units at rest (part 2): " + Day14.GetUnitsOfSandAtRest(cave, true));
                    break;
                case 15:
                    string beacons = File.ReadAllText(inputFolder + "15.txt");
                    Console.WriteLine("Empty positions: " + Day15.GetPossiblyEmptyPositions(beacons, 2000000));
                    Console.WriteLine("Tuning frequency: " + Day15.GetTuningFrequency(beacons));
                    break;
                case 16:
                    string valveStructure = File.ReadAllText(inputFolder + "16.txt");
                    Console.WriteLine("Biggest pressure release: " + Day16.GetReleasablePressure(valveStructure, 30, false));
                    Console.WriteLine("Biggest pressure release (part 2): " + Day16.GetReleasablePressure(valveStructure, 26, true));
                    break;
                case 17:
                    string jetPushes = File.ReadAllText(inputFolder + "17.txt");
                    Console.WriteLine("Tower height: " + Day17.GetTowerHeight(jetPushes, 2022));
                    Console.WriteLine("Tower height (part 2): " + Day17.GetTowerHeight(jetPushes, 1000000000000));
                    break;
                case 18:
                    string cubes = File.ReadAllText(inputFolder + "18.txt");
                    Console.WriteLine("Visible faces: " + Day18.GetVisibleFaces(cubes));
                    Console.WriteLine("Visible faces (part 2): " + Day18.GetVisibleFaces2(cubes));
                    break;
                case 19:
                    string blueprints = File.ReadAllText(inputFolder + "19.txt");
                    Console.WriteLine("Biggest quantity level: " + Day19.GetGeodeQuantityLevel(blueprints, 24, false));
                    Console.WriteLine("Largest number of geodes: " + Day19.GetGeodeQuantityLevel(blueprints, 32, true));
                    break;
                default:
                    Console.WriteLine("Invalid day number.");
                    break;
            }
        }
    }
}