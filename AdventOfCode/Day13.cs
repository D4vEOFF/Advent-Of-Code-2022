
namespace AdventOfCode
{
    internal class Day13
    {
        private class PacketComparer : IComparer<List<object>>
        {
            public int Compare(List<object>? x, List<object>? y)
            {
                Order order = IsInRightOrder(x, y);
                if (order == Order.IsIncorrect)
                    return 1;
                if (order == Order.IsCorrect)
                    return -1;
                return 0;
            }
        }
        private enum Order
        {
            IsCorrect, IsIncorrect, Unknown
        }
        private static List<object> ParseList(string list)
        {
            List<object> res = new List<object>();

            // Remove brackets
            list = list.Remove(0, 1);
            list = list.Remove(list.Length - 1, 1);

            // Split into cells
            Stack<object> stack = new Stack<object>();
            string cells = "";
            foreach (char c in list)
            {
                switch (c)
                {
                    case ',':
                        if (stack.Count == 0)
                            cells += " ";
                        else
                            cells += c;
                        break;
                    case '[':
                        stack.Push(c);
                        cells += "[";
                        break;
                    case ']':
                        stack.Pop();
                        cells += "]";
                        break;
                    default:
                        cells += c;
                        break;
                }
            }

            // Parse cells
            foreach (string s in cells.Split(' '))
            {
                // Nothing to parse
                if (s == "")
                    continue;

                int number;
                if (!int.TryParse(s, out number))
                {
                    List<object> l = ParseList(s);
                    res.Add(l);
                }
                else
                    res.Add(number);
            }
            return res;
        }
        private static List<Tuple<List<object>, List<object>>> ParseInput(string packets)
        {
            string[][] pairs = packets
                .Split(Environment.NewLine + Environment.NewLine)
                .Select(x => x.Split(Environment.NewLine))
                .ToArray();

            List<Tuple<List<object>, List<object>>> res = 
                new List<Tuple<List<object>, List<object>>>();
            foreach (var pair in pairs)
            {
                List<object> list1 = ParseList(pair[0]);
                List<object> list2 = ParseList(pair[1]);

                res.Add(new Tuple<List<object>, List<object>>(list1, list2));
            }

            return res;
        }
        private static Order IsInRightOrder(List<object> left, List<object> right)
        {
            Order order = Order.Unknown;
            int i = 0;
            int j = 0;

            while (i < left.Count && j < right.Count)
            {
                var leftItem = left[i];
                var rightItem = right[j];

                if (leftItem is int && rightItem is int)
                {
                    if ((int)leftItem < (int)rightItem)
                        order = Order.IsCorrect;
                    if ((int)leftItem > (int)rightItem)
                        order = Order.IsIncorrect;
                }
                else if (leftItem is List<object> && rightItem is int)
                    order = IsInRightOrder((List<object>)leftItem, new List<object>() { rightItem });
                else if (leftItem is int && rightItem is List<object>)
                    order = IsInRightOrder(new List<object>() { leftItem }, (List<object>)rightItem);
                else
                    order = IsInRightOrder((List<object>)leftItem, (List<object>)rightItem);

                // Result was determined already
                if (order != Order.Unknown)
                    break;

                i++; j++;
            }

            // List ran out of items
            if (i >= left.Count && j < right.Count)
                order = Order.IsCorrect;
            if (i < left.Count && j >= right.Count)
                order = Order.IsIncorrect;

            return order;
        }
        internal static int GetWrongOrderIndices(string packets)
        {
            List<Tuple<List<object>, List<object>>> input = ParseInput(packets);

            List<int> wrongPairsIndices = new List<int>();
            foreach (var pair in input.Select((Value, Index) => new { Value, Index }))
            {
                Order order = IsInRightOrder(pair.Value.Item1, pair.Value.Item2);
                if (order == Order.IsCorrect)
                    wrongPairsIndices.Add(pair.Index + 1);
            }

            return wrongPairsIndices.Sum();
        }
        internal static int GetDecoderKey(string packets)
        {
            List<Tuple<List<object>, List<object>>> input = ParseInput(packets);

            // Untuple input
            List<List<object>> inputUntupled = new List<List<object>>();
            foreach (var tuple in input)
            {
                inputUntupled.Add(tuple.Item1);
                inputUntupled.Add(tuple.Item2);
            }

            // Dividers
            List<object> divider1 = new List<object>() { new List<object>() { 2 } };
            List<object> divider2 = new List<object>() { new List<object>() { 6 } };
            inputUntupled.Add(divider1);
            inputUntupled.Add(divider2);

            // Sort
            PacketComparer packetComparer = new PacketComparer();
            inputUntupled.Sort(packetComparer);

            // Find the dividers
            int mul = 1;
            foreach (var packet in inputUntupled.Select((Value, Index) => new { Value, Index }))
                mul *= packet.Value == divider1 || packet.Value == divider2 ? 
                    packet.Index + 1 : 1;

            return mul;
        }
    }
}
