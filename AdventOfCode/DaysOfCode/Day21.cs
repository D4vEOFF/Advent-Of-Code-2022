
namespace AdventOfCode.DaysOfCode
{
    internal class Day21
    {
        enum Operation
        {
            Add, Subtract, Multiply, Divide, None
        }
        class Monkey
        {
            public Operation Operation { get; private set; }
            public string Monkey1 { get; private set; }
            public string Monkey2 { get; private set; }
            public long Number { get; private set; }
            public Monkey(Operation operation, string monkey1, 
                string monkey2, long number = -1)
            {
                Operation = operation;
                Monkey1 = monkey1;
                Monkey2 = monkey2;
                Number = number;
            }
            public override string ToString()
            {
                if (Number != -1)
                    return Number.ToString();

                string op = "";
                switch (Operation)
                {
                    case Operation.Add:
                        op += " + ";
                        break;
                    case Operation.Subtract:
                        op += " - ";
                        break;
                    case Operation.Multiply:
                        op += " * ";
                        break;
                    case Operation.Divide:
                        op += " / ";
                        break;
                }
                return Monkey1 + op + Monkey2;
            }
        }

        private static Dictionary<string, Monkey> monkeys;
        private static Dictionary<string, long> cache;

        private static Dictionary<string, Monkey> ParseInput(string input)
        {
            Dictionary<string, Monkey> monkeys = new Dictionary<string, Monkey>();
            foreach (var line in input.Split(Environment.NewLine))
            {
                string monkeyKey = line.Split(": ")[0];

                string monkey1 = "";
                string monkey2 = "";
                int number = -1;
                Operation op = Operation.Add;

                string[] operands = null;
                if (line.Contains("+"))
                {
                    operands = line.Split(" + ");
                    op = Operation.Add;
                }
                    
                else if (line.Contains("-"))
                {
                    operands = line.Split(" - ");
                    op = Operation.Subtract;
                }
                    
                else if (line.Contains("*"))
                {
                    operands = line.Split(" * ");
                    op = Operation.Multiply;
                }
                else if (line.Contains("/"))
                {
                    operands = line.Split(" / ");
                    op = Operation.Divide;
                }
                else
                {
                    number = int.Parse(line.Split(": ")[1]);
                    op = Operation.None;
                }

                if (!(operands is null))
                {
                    monkey1 = operands[0].Split(": ")[1];
                    monkey2 = operands[1];
                }

                monkeys[monkeyKey] = new Monkey(op, monkey1, monkey2, number);
            }

            return monkeys;
        }
        private static long GetMonkeyValue(Monkey monkey)
        {
            // Monkey yells a number
            if (monkey.Number != -1)
                return monkey.Number;

            // Monkey already resolved
            string key = monkey.ToString();
            if (cache.ContainsKey(key))
                return cache[key];

            Monkey monkey1 = monkeys[monkey.Monkey1];
            Monkey monkey2 = monkeys[monkey.Monkey2];

            long monkey1Result = GetMonkeyValue(monkey1);
            long monkey2Result = GetMonkeyValue(monkey2);
            long res = 0;
            switch (monkey.Operation)
            {
                case Operation.Add:
                    res = monkey1Result + monkey2Result;
                    break;
                case Operation.Subtract:
                    res = monkey1Result - monkey2Result;
                    break;
                case Operation.Multiply:
                    res = monkey1Result * monkey2Result;
                    break;
                case Operation.Divide:
                    res = monkey1Result / monkey2Result;
                    break;
            }

            cache[key] = res;

            return res;
        }
        internal static long GetRootMonkeyValue(string monkeyStr)
        {
            monkeys = ParseInput(monkeyStr);
            cache = new Dictionary<string, long>();

            return GetMonkeyValue(monkeys["root"]);
        }
        internal static long GetHumnValue(string monkeyStr)
        {
            monkeys = ParseInput(monkeyStr);
            List<string> humnBranch = new List<string>();
            long GetHumnValue0(Monkey monkey, long value, int depth)
            {
                // Humn monkey reached
                if (depth == 0)
                    return value;

                Monkey nextMonkey = humnBranch.Contains(monkey.Monkey1) ?
                    monkeys[monkey.Monkey1] : monkeys[monkey.Monkey2];
                Monkey otherMonkey = humnBranch.Contains(monkey.Monkey1) ?
                    monkeys[monkey.Monkey2] : monkeys[monkey.Monkey1];

                long res = 0;
                bool humnOnTheLeft = humnBranch.Contains(monkey.Monkey1);
                switch (monkey.Operation)
                {
                    case Operation.Add:
                        res =  GetHumnValue0(nextMonkey, value -
                            GetMonkeyValue(otherMonkey), depth - 1);
                        break;
                    case Operation.Subtract:
                        res = humnOnTheLeft ? GetHumnValue0(nextMonkey, value +
                            GetMonkeyValue(otherMonkey), depth - 1) :
                            GetHumnValue0(nextMonkey, GetMonkeyValue(otherMonkey) -
                            value, depth - 1);
                        break;
                    case Operation.Multiply:
                        res = GetHumnValue0(nextMonkey, value /
                            GetMonkeyValue(otherMonkey), depth - 1);
                        break;
                    case Operation.Divide:
                        res = humnOnTheLeft ? GetHumnValue0(nextMonkey, value *
                            GetMonkeyValue(otherMonkey), depth - 1) :
                            GetHumnValue0(nextMonkey, GetMonkeyValue(otherMonkey) /
                            value, depth - 1);
                        break;
                }

                return res;
            }

            Monkey root = monkeys["root"];

            // Find humn branch
            string monkeyName = "humn";
            while (monkeyName != "root")
            {
                humnBranch.Insert(0, monkeyName);
                monkeyName = monkeys.ToList().Find(x => x.Value.Monkey1 == monkeyName
                    || x.Value.Monkey2 == monkeyName).Key; ;
            }

            // Process non-humn branch first
            long nonhumnBranchValue = GetMonkeyValue(monkeys[root.Monkey1 == humnBranch[0] ?
                root.Monkey2 : root.Monkey1]);

            return GetHumnValue0(monkeys[humnBranch[0]], nonhumnBranchValue, humnBranch.Count - 1);
        }
    }
}
