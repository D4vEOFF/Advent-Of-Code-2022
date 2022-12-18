
using System.Runtime.CompilerServices;

namespace AdventOfCode
{
    public delegate ulong MonkeyOperation(ulong worryLevel);
    public delegate bool MonkeyTest(ulong item);
    class Monkey
    {
        private Queue<ulong> _Items { get; set; }
        public IReadOnlyList<ulong> Items
        {
            get 
            {
                return _Items.ToList().AsReadOnly(); 
            }
        }
        private MonkeyOperation _UpdateWorryLevel { get; set; }
        private MonkeyTest _Test { get; set; }
        private int[] _GiveItemsTo { get; set; }
        public Monkey(ulong[] items, int throwIfTrue, int throwIfFalse, MonkeyOperation updateWorryLevel, MonkeyTest test)
        {
            _Items = new Queue<ulong>(items);
            _GiveItemsTo = new int[] { throwIfTrue, throwIfFalse };
            _UpdateWorryLevel = updateWorryLevel;
            _Test = test;
        }
        public List<Tuple<ulong, int>> TakeTurn(bool lower, ulong reduceBy)
        {
            List<Tuple<ulong, int>> giveItemsTo = new List<Tuple<ulong, int>>();
            while (_Items.Count > 0)
            {
                ulong item = _Items.Dequeue();

                // Update worry level after playing with an item
                item = _UpdateWorryLevel(item);

                if (lower)
                    item /= 3;

                item %= reduceBy;

                giveItemsTo.Add(new Tuple<ulong, int>(item, 
                    _Test(item) ? _GiveItemsTo[0] : _GiveItemsTo[1]));
            }
            return giveItemsTo;
        }
        public void ObtainItem(ulong item)
        {
            _Items.Enqueue(item);
        }
        public override string ToString()
        {
            return "Items: " + string.Join(", ", _Items);
        }
    }
    internal class Day11
    {
        internal static Tuple<Monkey[], ulong> ParseInput(string monkeys)
        {
            ulong reduceBy = 1;
            return new Tuple<Monkey[], ulong>(monkeys
                .Split(Environment.NewLine + Environment.NewLine)
                .Select(x => x.Split(Environment.NewLine))
                .Select(x =>
                {
                    ulong[] items = null;
                    MonkeyOperation updateMonkeyLevel = null;
                    MonkeyTest test = null;
                    int throwIfTrue = 0;
                    int throwIfFalse = 0;

                    foreach (var line in x)
                    {
                        if (line.Contains("Starting items"))
                        {
                            items = line.Remove(0, 18)
                            .Split(", ")
                            .Select(x => ulong.Parse(x))
                            .ToArray();
                        }
                        else if (line.Contains("Operation"))
                        {
                            string[] expr = line.Remove(0, 13).Split(" ");
                            char operation = expr[3][0];
                            string[] operands = new string[] { expr[2], expr[4] };

                            switch (operation)
                            {
                                case '+':
                                    updateMonkeyLevel =
                                    (old) => (operands[0] == "old" ? old : ulong.Parse(operands[0])) +
                                    (operands[1] == "old" ? old : ulong.Parse(operands[1]));
                                    break;
                                case '-':
                                    updateMonkeyLevel =
                                    (old) => (operands[0] == "old" ? old : ulong.Parse(operands[0])) -
                                    (operands[1] == "old" ? old : ulong.Parse(operands[1]));
                                    break;
                                case '*':
                                    updateMonkeyLevel =
                                    (old) => (operands[0] == "old" ? old : ulong.Parse(operands[0])) *
                                    (operands[1] == "old" ? old : ulong.Parse(operands[1]));
                                    break;
                                case '/':
                                    updateMonkeyLevel =
                                    (old) => (operands[0] == "old" ? old : ulong.Parse(operands[0])) /
                                    (operands[1] == "old" ? old : ulong.Parse(operands[1]));
                                    break;
                            }
                        }
                        else if (line.Contains("Test"))
                        {
                            ulong divisor = ulong.Parse(line.Remove(0, 21));
                            reduceBy *= divisor;
                            test = (item) => item % divisor == 0;
                        }
                        else if (line.Contains("If true"))
                            throwIfTrue = int.Parse(line.Remove(0, 29));
                        else if (line.Contains("If false"))
                            throwIfFalse = int.Parse(line.Remove(0, 30));
                    }
                    return new Monkey(items, throwIfTrue, throwIfFalse, updateMonkeyLevel, test);
                })
                .ToArray(),
                reduceBy);
        }
        internal static long GetMonkeyBusiness(string monkeysAttributes, int rounds, bool lower)
        {
            Tuple<Monkey[], ulong> input = ParseInput(monkeysAttributes);
            Monkey[] monkeys = input.Item1;
            ulong reduceBy = input.Item2;

            long[] itemsInspected = new long[monkeys.Length];
            for (int round = 0; round < rounds; round++)
            {
                for (int i = 0; i < monkeys.Length; i++)
                {
                    List<Tuple<ulong, int>> giveItemsTo = monkeys[i].TakeTurn(lower, reduceBy);

                    itemsInspected[i] += giveItemsTo.Count;

                    // Give items to other monkeys
                    foreach (var tuple in giveItemsTo)
                        monkeys[tuple.Item2].ObtainItem(tuple.Item1);
                }
            }

            Array.Sort(itemsInspected);
            Array.Reverse(itemsInspected);
            return itemsInspected[0] * itemsInspected[1];
        }
    }
}
