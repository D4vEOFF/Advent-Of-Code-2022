using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal class Day4
    {
        public static int Overlaps(string pairs)
        {
            return pairs
                .Split(Environment.NewLine)
                .Select(x => x.Split(','))
                .Select(x =>
                {
                    string[] digits = x.First().Split('-');
                    int firstStart = int.Parse(digits[0]);
                    int firstEnd = int.Parse(digits[1]);

                    digits = x.Last().Split('-');
                    int secondStart = int.Parse(digits[0]);
                    int secondEnd = int.Parse(digits[1]);

                    if (firstStart >= secondStart && firstEnd <= secondEnd ||
                    secondStart >= firstStart && secondEnd <= firstEnd)
                        return 1;
                    return 0;
                })
                .Sum();
        }
        public static int Overlaps2(string pairs)
        {
            return pairs
                .Split(Environment.NewLine)
                .Select(x => x.Split(','))
                .Select(x =>
                {
                    string[] digits = x.First().Split('-');
                    int[] firstBounds = new int[] { int.Parse(digits[0]), 
                        int.Parse(digits[1]) };

                    digits = x.Last().Split('-');
                    int[] secondBounds = new int[] { int.Parse(digits[0]),
                        int.Parse(digits[1]) };

                    if (firstBounds[0] <= secondBounds[1] && secondBounds[0] <= firstBounds[1])
                        return 1;
                    return 0;
                })
                .Sum();
        }
    }
}
