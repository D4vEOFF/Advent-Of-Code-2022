namespace AdventOfCode
{
    internal class Day1
    {
        public static int TopThreeMaxCallories(string callories)
        {
            return callories
                .Split(Environment.NewLine + Environment.NewLine)
                .Select(x => x.Split(Environment.NewLine).Select(x => int.Parse(x)).Sum())
                .OrderDescending().Take(3).Sum();
        }
        public static int MaxCallories(string callories)
        {
            return callories
                .Split(Environment.NewLine + Environment.NewLine)
                .Select(x => x.Split(Environment.NewLine).Select(x => int.Parse(x)).Sum())
                .Max();
        }
    }
}
