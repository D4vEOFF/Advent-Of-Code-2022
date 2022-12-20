
using System.Net.Sockets;

namespace AdventOfCode.DaysOfCode
{
    internal class Day20
    {
        class Number
        {
            public long Value { get; set; }
            public bool Moved { get; set; }
            public Number(long value)
            {
                Value = value;
            }
            public override string ToString()
            {
                return Value.ToString();
            }
        }
        internal static long MixFile(string input, int decryptionKey, int mixCount)
        {
            // Parse input
            List<Number> file = input
                .Split(Environment.NewLine)
                .Select(number => new Number(long.Parse(number)))
                .ToList();
            Number[] order = (Number[])file.ToArray().Clone();

            // Apply decryption key
            foreach (var number in file)
                number.Value *= decryptionKey;

            // Mix
            for (int _ = 0; _ < mixCount; _++)
                for (int i = 0; i < order.Length; i++)
                {
                    Number number = order[i];
                    int index = file.IndexOf(number);
                    file.Remove(number);

                    int moveTo = (int)((index + number.Value) % file.Count);

                    // Recalc new index if out of bounds
                    if (moveTo <= 0)
                        moveTo += file.Count;

                    file.Insert(moveTo, number);
                }

            // Debug
            //Console.WriteLine(string.Join(", ", file));

            // Print 1000th, 2000th and 3000th position after zero
            int zeroIndex = file.IndexOf(file.Find(x => x.Value == 0));
            long res = 0;
            for (int l = 1000; l <= 3000; l += 1000)
                res += file[(zeroIndex + l) % file.Count].Value;

            return res;
        }
    }
}
