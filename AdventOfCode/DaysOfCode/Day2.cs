namespace AdventOfCode
{
    internal class Day2
    {
        public static int RockPaperScissorsTotal2(string strategy)
        {
            Dictionary<char, char> wins = new Dictionary<char, char>()
            {
                { 'A', 'C' },
                { 'B', 'A' },
                { 'C' , 'B' }
            };
            Dictionary<char, char> loses = new Dictionary<char, char>()
            {
                { 'A', 'B' },
                { 'B', 'C' },
                { 'C' , 'A' }
            };
            Dictionary<char, int> scores = new Dictionary<char, int>()
            {
                { 'A', 1 },
                { 'B', 2 },
                { 'C', 3 }
            };
            return strategy
                .Split(Environment.NewLine)
                .Select(x =>
                {
                    int score = 0;
                    // We must lose
                    if (x.Last() == 'X')
                        score += scores[wins[x.First()]];
                    // Draw
                    else if (x.Last() == 'Y')
                        score += scores[x.First()] + 3;
                    // Win
                    else
                        score += scores[loses[x.First()]] + 6;

                    return score;
                })
                .Sum();
        }
        public static int RockPaperScissorsTotal(string strategy)
        {
            Dictionary<char, char> draws = new Dictionary<char, char>() {
                { 'A', 'X' },
                { 'B', 'Y' },
                { 'C', 'Z' }
            };
            Dictionary<char, char> wins = new Dictionary<char, char>()
            {
                { 'X', 'C' },
                { 'Y', 'A' },
                { 'Z' , 'B' }
            };
            return strategy
                .Split(Environment.NewLine)
                .Select(x =>
                {
                    int score = 0;
                    // Draw
                    if (x.Last() == draws[x.First()])
                        score += 3;
                    // Win
                    else if (x.First() == wins[x.Last()])
                        score += 6;

                    if (x.Last() == 'X')
                        score += 1;
                    else if (x.Last() == 'Y')
                        score += 2;
                    else
                        score += 3;

                    return score;
                })
                .Sum();
        }
    }
}
