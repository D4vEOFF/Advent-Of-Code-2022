
using AdventOfCode.Utilities;
using System.Diagnostics.CodeAnalysis;
using System.Runtime;
using System.Security.Cryptography.X509Certificates;

namespace AdventOfCode.DaysOfCode
{
    internal class Day24
    {
        private static Dictionary<char, Vector2D> dirs =
            new Dictionary<char, Vector2D>()
            {
                { '>', new Vector2D(1, 0) },
                { '<', new Vector2D(-1, 0) },
                { '^', new Vector2D(0, -1) },
                { 'v', new Vector2D(0, 1) }
            };
        class PriorityQueueEqualityComparer : IEqualityComparer<Tuple<Vector2D, int>>
        {
            public bool Equals(Tuple<Vector2D, int>? x, Tuple<Vector2D, int>? y)
            {
                if (x is null || y is null) return false;
                return x.Item1 == y.Item1 && x.Item2 == y.Item2;
            }

            public int GetHashCode([DisallowNull] Tuple<Vector2D, int> obj)
            {
                return obj.Item1.X ^ obj.Item2 + obj.Item1.Y ^ obj.Item2;
            }
        }
        class Blizzard
        {
            public Vector2D Position { get; set; }
            public Vector2D Direction { get; set; }
            public Blizzard(int x, int y, Vector2D direction)
            {
                Position = new Vector2D(x, y);
                Direction = direction;
            }
        }
        private static Tuple<List<Blizzard>, int, int> ParseInput(string input)
        {
            string[] lines = input.Split(Environment.NewLine);
            List<Blizzard> blizzards = new List<Blizzard>();
            for (int y = 0; y < lines.Length; y++)
            {
                string line = lines[y];
                for (int x = 0; x < line.Length; x++)
                {
                    char c = line[x];
                    if (c == '.' || c == '#')
                        continue;
                    blizzards.Add(new Blizzard(x, y, dirs[c]));
                }
            }
            return new Tuple<List<Blizzard>, int, int>
                (blizzards, lines[0].Length, lines.Length);
        }
        private static void Show(Vector2D pos, HashSet<Vector2D> blizzardState, int width, int height)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Vector2D p = new Vector2D(x, y);

                    if (p == pos)
                        Console.Write('E');
                    else if (blizzardState.Contains(p))
                        Console.Write('-');
                    else if (p == new Vector2D(1, 0) ||
                        p == new Vector2D(width - 2, height - 1))
                        Console.Write('.');
                    else if (p.X == 0 || p.X == width - 1 ||
                        p.Y == 0 || p.Y == height - 1)
                        Console.Write('#');
                    else Console.Write('.');
                }
                Console.WriteLine();
            }
        }
        internal static int GetShortestPath(string mapStr, int repeat)
        {
            Tuple<List<Blizzard>, int, int> input = ParseInput(mapStr);
            List<Blizzard> blizzards = input.Item1;
            int width = input.Item2;
            int height = input.Item3;

            // Precalculate all blizzard states
            int period = Utils.LCM(width - 2, height - 2);
            List<HashSet<Vector2D>> blizzardStates = 
                new List<HashSet<Vector2D>>();
            blizzardStates.Add(new HashSet<Vector2D>
                (blizzards.Select(b => b.Position)));
            List<Blizzard> prevState = blizzards;
            for (int t = 1; t < period; t++)
            {
                Vector2D posNew = new Vector2D();
                HashSet<Vector2D> blizzardsNew = new HashSet<Vector2D>();
                for (int i = 0; i < prevState.Count; i++)
                {
                    var blizzard = prevState[i];
                    posNew = blizzard.Position + blizzard.Direction;
                    if (posNew.X == width - 1)
                        posNew.X = 1;
                    else if (posNew.X == 0)
                        posNew.X = width - 2;
                    else if (posNew.Y == height - 1)
                        posNew.Y = 1;
                    else if (posNew.Y == 0)
                        posNew.Y = height - 2;

                    blizzardsNew.Add(posNew);
                    prevState[i].Position = posNew;
                }
                blizzardStates.Add(blizzardsNew);
            }

            // Dijkstra
            Vector2D start = new Vector2D(1, 0);
            Vector2D end = new Vector2D(width - 2, height - 1);
            int time;
            int finalTime = 0;

            Vector2D pos = start;
            HashSet<Tuple<Vector2D, int>> visited =
                new HashSet<Tuple<Vector2D, int>>
                (new PriorityQueueEqualityComparer());
            PriorityQueue<Vector2D, int> queue = 
                new PriorityQueue<Vector2D, int>();
            queue.Enqueue(pos, 0);

    
            while (queue.Count > 0)
            {
                queue.TryDequeue(out pos, out time);

                //Show(pos, blizzardStates[time % period], width, height);
                //Console.WriteLine();

                // State already visited
                Tuple<Vector2D, int> state = new
                    Tuple<Vector2D, int>(pos, time);
                if (visited.Contains(state))
                    continue;

                visited.Add(state);

                // End reached
                if (pos == end)
                {
                    finalTime += time;
                    if (repeat <= 1)
                        return finalTime;

                    repeat--;
                    // Swap start and the end
                    var temp = start;
                    start = end;
                    end = temp;

                    // Setup initial
                    visited.Clear();
                    queue.Clear();
                    pos = start;
                    queue.Enqueue(pos, 0);
                    continue;
                }

                // Loop through neighbors
                foreach (var dir in dirs.Values.Append(new Vector2D(0, 0)))
                {
                    Vector2D neighbor = pos + dir;

                    // Blizzard reaches the pos in the next move
                    if (blizzardStates[(finalTime + time + 1) % period].Contains(neighbor))
                        continue;

                    // Wall encountered
                    if ((neighbor.X >= width - 1 || neighbor.X <= 0 ||
                        neighbor.Y >= height - 1 || neighbor.Y <= 0) &&
                        neighbor != start && neighbor != end)
                        continue;

                    queue.Enqueue(neighbor, time + 1);
                }
            }

            return 0;
        }
    }
}
