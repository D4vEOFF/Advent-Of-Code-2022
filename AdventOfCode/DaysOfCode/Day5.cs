
namespace AdventOfCode
{
    internal class Day5
    {
        public static string CrateRearangement(string initConfig)
        {
            string[] config = initConfig.Split(Environment.NewLine + Environment.NewLine);

            Stack<char>[] stacks = 
                new Stack<char>[config[0].Split(Environment.NewLine)[0].Length / 4 + 1];

            // Init stacks
            for (int i = 0; i < stacks.Length; i++)
                stacks[i] = new Stack<char>();

            // Load into stacks
            string[] stackConfig = config[0].Split(Environment.NewLine);
            for (int i = stackConfig.Length - 2; i >= 0; i--)
            {
                string line = stackConfig[i];
                for (int j = 1; j < line.Length; j += 4)
                {
                    if (line[j] == ' ')
                        continue;
                    stacks[j / 4].Push(line[j]);
                }
            }

            string[] craneProcedure = config[1].Split(Environment.NewLine);
            Queue<char> cratesCarried = new Queue<char>();
            foreach (string line in craneProcedure)
            {
                int[] data = line.Split(new string[] { "move ", " from ", " to " }, 
                    StringSplitOptions.None).Skip(1).Select(x => int.Parse(x)).ToArray();

                // Remove crates
                cratesCarried.Clear();
                for (int i = 0; i < data[0];i++)
                    cratesCarried.Enqueue(stacks[data[1] - 1].Pop());

                // Place crates
                while (cratesCarried.Count > 0)
                    stacks[data[2] - 1].Push(cratesCarried.Dequeue());
            }

            // Debug
            //foreach (var stack in stacks)
            //{
            //    while (stack.Count > 0)
            //        Console.Write(stack.Pop());
            //    Console.WriteLine();
            //}

            string tops = "";
            char pop;
            foreach (var stack in stacks)
                tops += stack.TryPop(out pop) ? pop : "";

            return tops;
        }
        public static string CrateRearangement2(string initConfig)
        {
            string[] config = initConfig.Split(Environment.NewLine + Environment.NewLine);

            Stack<char>[] stacks =
                new Stack<char>[config[0].Split(Environment.NewLine)[0].Length / 4 + 1];

            // Init stacks
            for (int i = 0; i < stacks.Length; i++)
                stacks[i] = new Stack<char>();

            // Load into stacks
            string[] stackConfig = config[0].Split(Environment.NewLine);
            for (int i = stackConfig.Length - 2; i >= 0; i--)
            {
                string line = stackConfig[i];
                for (int j = 1; j < line.Length; j += 4)
                {
                    if (line[j] == ' ')
                        continue;
                    stacks[j / 4].Push(line[j]);
                }
            }

            string[] craneProcedure = config[1].Split(Environment.NewLine);
            Stack<char> cratesCarried = new Stack<char>();
            foreach (string line in craneProcedure)
            {
                int[] data = line.Split(new string[] { "move ", " from ", " to " },
                    StringSplitOptions.None).Skip(1).Select(x => int.Parse(x)).ToArray();

                // Remove crates
                cratesCarried.Clear();
                for (int i = 0; i < data[0]; i++)
                    cratesCarried.Push(stacks[data[1] - 1].Pop());

                // Place crates
                while (cratesCarried.Count > 0)
                    stacks[data[2] - 1].Push(cratesCarried.Pop());
            }

            // Debug
            //foreach (var stack in stacks)
            //{
            //    while (stack.Count > 0)
            //        Console.Write(stack.Pop());
            //    Console.WriteLine();
            //}

            string tops = "";
            char pop;
            foreach (var stack in stacks)
                tops += stack.TryPop(out pop) ? pop : "";

            return tops;
        }
    }
}
