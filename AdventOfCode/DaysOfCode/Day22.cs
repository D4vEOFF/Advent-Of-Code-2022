using AdventOfCode.Utilities;

namespace AdventOfCode.DaysOfCode
{
    internal class Day22
    {
        private static Tuple<char[,], string[], Vector2D> ParseInput(string input)
        {
            string[] inputSplit = input.Split(Environment.NewLine
                + Environment.NewLine);

            // Parse path
            List<string> path = new List<string>();
            foreach (char c in inputSplit[1])
            {
                if (path.Count == 0 || char.IsLetter(c))
                    path.Add(c.ToString());
                else if (char.IsDigit(c) && path.Last().All(char.IsDigit))
                    path[path.Count - 1] += c;
                else path.Add(c.ToString());
            }

            // Parse map
            string[] lines = inputSplit[0].Split(Environment.NewLine);
            int width = lines
                .Select(x => x.Length)
                .Max();
            int height = inputSplit[0].Count(c => c == '\n') + 1;

            char[,] map = new char[height, width];
            Vector2D startTile = new Vector2D(-1, -1);

            for (int i = 0; i < height; i++)
            {
                string line = lines[i];
                for (int j = 0; j < width; j++)
                {
                    if (j >= line.Length)
                    {
                        map[i, j] = ' ';
                        continue;
                    }
                    else
                        map[i, j] = line[j];

                    if (line[j] == '.' && startTile == new Vector2D(-1, -1))
                        startTile = new Vector2D(i, j);
                }
            }

            return new Tuple<char[,], string[], Vector2D>(map, path.ToArray(), startTile);
        }
        internal static int GetPassword(string mapPathStr, bool show, bool cube)
        {
            Tuple<char[,], string[], Vector2D> input = ParseInput(mapPathStr);
            char[,] map = input.Item1;
            string[] path = input.Item2;
            Vector2D startTile = input.Item3;

            bool WithinRange(int coord, int dim) => coord >= 0 &&
                coord < map.GetLength(dim);

            Vector2D pos = startTile;
            Vector2D dir = new Vector2D(0, 1);

            // Directions
            Vector2D left = new Vector2D(0, -1);
            Vector2D right = new Vector2D(0, 1);
            Vector2D up = new Vector2D(-1, 0);
            Vector2D down = new Vector2D(1, 0);

            char DirectionChar()
            {
                return dir == left ? '<' :
                        (dir == right ? '>' :
                        (dir == up ? '^' :
                        'v'));
            }

            // Debug
            map[pos.X, pos.Y] = DirectionChar();

            foreach (string move in path)
            {
                // Change direction
                int temp = dir.X;
                if (char.IsLetter(move[0]))
                {
                    // Rotate
                    switch (move)
                    {
                        case "L":
                            dir.X = -dir.Y;
                            dir.Y = temp;
                            break;
                        case "R":
                            dir.X = dir.Y;
                            dir.Y = -temp;
                            break;
                    }

                    // Debug
                    map[pos.X, pos.Y] = DirectionChar();

                    continue;
                }

                // Move in given direction
                int steps = int.Parse(move);
                for (int _ = 0; _ < steps; _++)
                {
                    Vector2D nextPos = pos + dir;

                    // Wrap around an enge of the cube
                    if (cube)
                    {
                        
                    }
                    else
                    {
                        // Out of range
                        // Left/right
                        if (!WithinRange(nextPos.X, 0))
                        {
                            nextPos.X = nextPos.X < 0 ? map.GetLength(0) - 1 : 0;
                            while (map[nextPos.X, nextPos.Y] == ' ')
                                nextPos.X += dir.X;
                        }
                        // Up/down
                        else if (!WithinRange(nextPos.Y, 1))
                        {
                            nextPos.Y = nextPos.Y < 0 ? map.GetLength(1) - 1 : 0;
                            while (map[nextPos.X, nextPos.Y] == ' ')
                                nextPos.Y += dir.Y;
                        }

                        // Off the map
                        else if (map[nextPos.X, nextPos.Y] == ' ')
                        {
                            if (dir == left)
                                nextPos.Y = map.GetLength(1) - 1;
                            else if (dir == right)
                                nextPos.Y = 0;
                            else if (dir == up)
                                nextPos.X = map.GetLength(0) - 1;
                            else if (dir == down)
                                nextPos.X = 0;

                            while (map[nextPos.X, nextPos.Y] == ' ')
                                nextPos += dir;
                        }
                    }

                    // Wall encountered
                    if (map[nextPos.X, nextPos.Y] == '#')
                        continue;

                    // Move
                    pos = nextPos;

                    // Debug
                    map[pos.X, pos.Y] = DirectionChar();
                }
            }

            // Debug
            if (show)
            {
                for (int i = 0; i < map.GetLength(0); i++)
                {
                    for (int j = 0; j < map.GetLength(1); j++)
                        Console.Write(map[i, j]);
                    Console.WriteLine();
                }
            }

            int facing = dir == left ? 2 :
                        (dir == right ? 0 :
                        (dir == up ? 3 : 1));
            return 1000 * (pos.X + 1) + 4 * (pos.Y + 1) + facing;
        }
    }
}
