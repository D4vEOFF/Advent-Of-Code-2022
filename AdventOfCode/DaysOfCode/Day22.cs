
namespace AdventOfCode.DaysOfCode
{
    internal class Day22
    {
        private static Tuple<char[,], string> ParseInput(string input)
        {
            string[] inputSplit = input.Split(Environment.NewLine
                + Environment.NewLine);
            string path = inputSplit[1];

            string[] lines = inputSplit[0].Split(Environment.NewLine);
            int width = lines
                .Select(x => x.Length)
                .Max();
            int height = inputSplit[0].Count(c => c == '\n') + 1;

            char[,] map = new char[height, width];

            for (int i = 0; i < height; i++)
            {
                string line = lines[i];
                for (int j = 0; j < width; j++)
                {
                    if (j >= line.Length)
                        map[i, j] = ' ';
                    else
                        map[i, j] = line[j];
                }
            }

            return new Tuple<char[,], string>(map, path);
        }
        internal static int GetPassword(string mapPathStr)
        {
            Tuple<char[,], string> input = ParseInput(mapPathStr);
            char[,] map = input.Item1;
            string path = input.Item2;

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                    Console.Write(map[i, j]);
                Console.WriteLine();
            }
                

            return 0;
        }
    }
}
