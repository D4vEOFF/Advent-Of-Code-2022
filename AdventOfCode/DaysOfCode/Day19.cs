
namespace AdventOfCode.DaysOfCode
{
    internal class Day19
    {
        class Robot
        {
            public int OreCost { get; private set; }
            public int ClayCost { get; private set; }
            public int ObsidianCost { get; private set; }
            public Robot(int oreCost, int clayCost, int obsidianCost)
            {
                OreCost = oreCost;
                ClayCost = clayCost;
                ObsidianCost = obsidianCost;
            }
        }
        private static List<List<Robot>> ParseInput(string input)
        {
            return input
                .Split(Environment.NewLine)
                .Select(line =>
                {
                    List<Robot> robots = new List<Robot>();
                    string[] data = line.Split('.').Take(4).ToArray();
                    foreach (string robotData in data)
                    {
                        int ore = 0;
                        int clay = 0;
                        int obsidian = 0;

                        string[] words = robotData.Split(' ');
                        for (int i = 0; i < words.Length; i++)
                        {
                            int amount;
                            if (int.TryParse(words[i], out amount))
                                switch (words[i + 1])
                                {
                                    case "ore":
                                        ore = amount;
                                        break;
                                    case "clay":
                                        clay = amount;
                                        break;
                                    case "obsidian":
                                        obsidian = amount;
                                        break;
                                }
                        }

                        robots.Add(new Robot(ore, clay, obsidian));
                    }

                    return robots;
                })
                .ToList();
        }
        internal static int GetGeodeQuantityLevel(string blueprintsStr)
        {
            
        }
    }
}
