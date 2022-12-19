
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
        internal static int GetGeodeQuantityLevel(string blueprintsStr, int time, bool takeFirstThree)
        {
            List<List<Robot>> blueprints = ParseInput(blueprintsStr);

            List<Robot> currentBlueprint;
            Dictionary<string, int> cache = new Dictionary<string, int>();

            int maxOreCost, maxClayCost, maxObsidianCost;

            int GetGeodeQuantityLevel0(int[] resources, int[] robots, int time)
            {
                // No time left
                if (time == 0)
                    return 0;

                int[] resourcesNew = (int[])resources.Clone();
                int[] robotsNew = (int[])robots.Clone();

                // Optimize
                if (robotsNew[0] > maxOreCost)
                    robotsNew[0] = maxOreCost;
                if (robotsNew[1] > maxClayCost)
                    robotsNew[1] = maxClayCost;
                if (robotsNew[2] > maxObsidianCost)
                    robotsNew[2] = maxObsidianCost;

                int ore = time * maxOreCost - robotsNew[0] * (time - 1);
                int clay = time * maxClayCost - robotsNew[1] * (time - 1);
                int obsidian= time * maxObsidianCost - robotsNew[2] * (time - 1);
                if (resourcesNew[0] > ore)
                    resourcesNew[0] = ore;
                if (resourcesNew[1] > clay)
                    resourcesNew[1] = clay;
                if (resourcesNew[2] > obsidian)
                    resourcesNew[2] = obsidian;

                // Result cached
                string key = $"{string.Join(",", resourcesNew)};{string.Join(",", robotsNew)};{time}";
                if (cache.ContainsKey(key))
                    return cache[key];

                int i;
                // Pick up resources from robots
                for (i = 0; i < 4; i++)
                    resourcesNew[i] += robots[i];

                // Build a new robot
                int geodes = 0;
                for (i = 0; i < 4; i++)
                {
                    Robot robot = currentBlueprint[i];
                    bool bought = false;
                    if (resources[0] >= robot.OreCost && resources[1] >= robot.ClayCost
                        && resources[2] >= robot.ObsidianCost)
                    {
                        resourcesNew[0] -= robot.OreCost;
                        resourcesNew[1] -= robot.ClayCost;
                        resourcesNew[2] -= robot.ObsidianCost;

                        robotsNew[i]++;

                        bought = true;
                    }

                    geodes = Math.Max(geodes, resourcesNew[3] - resources[3] + GetGeodeQuantityLevel0(resourcesNew, robotsNew, time - 1));

                    if (bought)
                    {
                        robotsNew[i]--;

                        resourcesNew[0] += robot.OreCost;
                        resourcesNew[1] += robot.ClayCost;
                        resourcesNew[2] += robot.ObsidianCost;
                    }
                }

                cache[key] = geodes;

                return geodes;
            }


            int[] geodeAmounts = new int[blueprints.Count];

            int product = 1;
            for (int i = 0; i < blueprints.Count; i++)
            {
                currentBlueprint = blueprints[i];

                maxOreCost = currentBlueprint.Select(r => r.OreCost).Max();
                maxClayCost = currentBlueprint.Select(r => r.ClayCost).Max();
                maxObsidianCost = currentBlueprint.Select(r => r.ObsidianCost).Max();

                cache.Clear();

                int geodeAmount = GetGeodeQuantityLevel0(new int[] { 0, 0, 0, 0 }, new int[] { 1, 0, 0, 0 }, time);
                //Console.WriteLine($"Blueprint {i+1} done. Result {geodeAmount}");

                geodeAmounts[i] = (i + 1) * geodeAmount;

                if (i < 3)
                    product *= geodeAmount;
                else if (takeFirstThree)
                    break;
            }

            return takeFirstThree ? product : geodeAmounts.Sum();
        }
    }
}
