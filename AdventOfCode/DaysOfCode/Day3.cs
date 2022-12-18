namespace AdventOfCode
{
    internal class Day3
    {
        public static int RucksackSum(string rucksacks)
        {
            return rucksacks
                .Split(Environment.NewLine)
                .Select(x =>
                {
                    string common = "";
                    string firstHalf = x.Substring(0, x.Length / 2);
                    string secondHalf = x.Substring(x.Length / 2);
                    foreach (char c in firstHalf)
                    {
                        if (secondHalf.Contains(c) && !common.Contains(c))
                            common += c;
                    }
                    return common;
                })
                .Select(x =>
                {
                    int priority = 0;
                    foreach (char c in x)
                    {
                        int code = c;
                        // Lowercase
                        if (code >= 97 && code <= 122)
                            priority += code - 96;
                        // Uppercase
                        if (code >= 65 && code <= 90)
                            priority += code - 38;
                    }
                    return priority;
                })
                .Sum();
        }
        public static int RucksackSum2(string rucksacks)
        {
            string capitalAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return rucksacks
                .Split(Environment.NewLine)
                .Select((value, index) => new { index, value })
                .GroupBy(x => x.index / 3, x => x.value)
                .Select(x =>
                {
                    string common = capitalAlphabet + capitalAlphabet.ToLower();
                    foreach (var rucksack in x)
                        common = string.Join("", common.Intersect(rucksack));
                    return common;
                })
                .Select(x =>
                {
                    int priority = 0;
                    foreach (char c in x)
                    {
                        int code = c;
                        // Lowercase
                        if (code >= 97 && code <= 122)
                            priority += code - 96;
                        // Uppercase
                        if (code >= 65 && code <= 90)
                            priority += code - 38;
                    }
                    return priority;
                })
                .Sum();
        }
    }
}
