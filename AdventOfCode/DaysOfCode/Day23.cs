using AdventOfCode.Utilities;

namespace AdventOfCode.DaysOfCode
{
    internal class Day23
    {
        private static Dictionary<char, Vector2D> dirs = 
            new Dictionary<char, Vector2D>()
        {
            { 'N', new Vector2D(0, 1) },
            { 'S', new Vector2D(0, -1) },
            { 'E', new Vector2D(1, 0) },
            { 'W', new Vector2D(-1, 0) }
        };
        private static List<Vector2D[]> blocks = new List<Vector2D[]>();
        class Elf
        {
            public Vector2D Position { get; private set; }
            public Vector2D Direction { get; set; }
            public Elf(int x, int y)
            {
                Position = new Vector2D(x, y);
                Direction = new Vector2D();
            }
            public bool Move()
            {
                if (Direction == new Vector2D())
                    return false;
                Position += Direction;
                Direction = new Vector2D();
                return true;
            }
        }
        private static void InitBlocks()
        {
            blocks = new List<Vector2D[]>()
            {
                new Vector2D[]
                {
                    dirs['N'],
                    dirs['N'] + dirs['W'],
                    dirs['N'] + dirs['E']
                },
                new Vector2D[]
                {
                    dirs['S'],
                    dirs['S'] + dirs['W'],
                    dirs['S'] + dirs['E']
                },
                new Vector2D[]
                {
                    dirs['W'],
                    dirs['W'] + dirs['N'],
                    dirs['W'] + dirs['S']
                },
                new Vector2D[]
                {
                    dirs['E'],
                    dirs['E'] + dirs['N'],
                    dirs['E'] + dirs['S']
                }
            };
        }
        private static Tuple<HashSet<Elf>, HashSet<Vector2D>> ParseInput(string input)
        {
            string[] lines = input.Split(Environment.NewLine);

            HashSet<Elf> elfs = new HashSet<Elf>();
            HashSet<Vector2D> occupied = new HashSet<Vector2D>();
            for (int y = 0; y < lines.Length; y++)
            {
                string line = lines[y];
                for (int x = 0; x < line.Length; x++)
                {
                    if (line[x] == '.')
                        continue;
                    elfs.Add(new Elf(x, lines.Length - y - 1));
                    occupied.Add(new Vector2D(x, lines.Length - y - 1));
                }
            }

            return new Tuple<HashSet<Elf>, HashSet<Vector2D>>(elfs, occupied);
        }
        private static void Show(HashSet<Vector2D> occupied)
        {
            int xMin = occupied.Select(e => e.X).Min() - 1;
            int xMax = occupied.Select(e => e.X).Max() + 1;
            int yMin = occupied.Select(e => e.Y).Min() - 1;
            int yMax = occupied.Select(e => e.Y).Max() + 1;

            Vector2D p = new Vector2D();
            for (int y = yMax; y >= yMin; y--)
            {
                for (int x = xMin; x <= xMax; x++)
                {
                    p.X = x;
                    p.Y = y;
                    if (occupied.Contains(p))
                        Console.Write('#');
                    else Console.Write('.');
                }
                Console.WriteLine();
            }
        }
        private static bool Simulate(HashSet<Elf> elfs, HashSet<Vector2D> occupied, int rounds)
        {
            bool moved = false;
            for (int _ = 0; _ < rounds; _++)
            {
                // Propose directions
                foreach (var elf in elfs)
                {
                    Vector2D pos = elf.Position;

                    bool alone = true;
                    for (int i = -1; i <= 1; i++)
                        for (int j = -1; j <= 1; j++)
                        {
                            if (i == 0 && j == 0) continue;
                            if (occupied.Contains(pos + new Vector2D(i, j)))
                                alone = false;
                        }

                    // Elf surrounded by no other elfs
                    if (alone)
                        continue;

                    // Check tile blocks
                    Vector2D proposedDir = new Vector2D();
                    foreach (var block in blocks)
                    {
                        bool free = true;
                        foreach (var blockDir in block)
                            if (occupied.Contains(pos + blockDir))
                                free = false;

                        // Block is not free (occupied by an elf)
                        if (!free)
                            continue;

                        proposedDir = block[0];
                        break;
                    }

                    // Tile already proposed by another elf
                    if (occupied.Contains(pos + 2 * proposedDir))
                    {
                        Elf tileSharer = elfs.FirstOrDefault(e =>
                            e.Position + e.Direction == pos + proposedDir);
                        if (tileSharer != null)
                        {
                            proposedDir = new Vector2D();
                            tileSharer.Direction = new Vector2D();
                        }
                    }

                    elf.Direction = proposedDir;
                }

                // Move elfs
                foreach (var elf in elfs)
                {
                    occupied.Remove(elf.Position);
                    if (elf.Move())
                        moved = true;
                    occupied.Add(elf.Position);
                }

                // Shift directions
                Vector2D[] firstBlock = blocks[0];
                blocks.RemoveAt(0);
                blocks.Add(firstBlock);
            }

            return moved;
        }
        internal static int GetEmptyTiles(string mapStr, int rounds)
        {
            InitBlocks();

            Tuple<HashSet<Elf>, HashSet<Vector2D>> input = ParseInput(mapStr);
            HashSet<Elf> elfs = input.Item1;
            HashSet<Vector2D> occupied = input.Item2;

            Simulate(elfs, occupied, rounds);

            // Debug
            int xMin = elfs.Select(e => e.Position.X).Min();
            int xMax = elfs.Select(e => e.Position.X).Max();
            int yMin = elfs.Select(e => e.Position.Y).Min();
            int yMax = elfs.Select(e => e.Position.Y).Max();

            return (yMax - yMin + 1) * (xMax - xMin + 1) - elfs.Count;
        }
        internal static int GetNoActionRound(string mapStr)
        {
            InitBlocks();

            Tuple<HashSet<Elf>, HashSet<Vector2D>> input = ParseInput(mapStr);
            HashSet<Elf> elfs = input.Item1;
            HashSet<Vector2D> occupied = input.Item2;

            int round = 1;
            while (Simulate(elfs, occupied, 1))
                round++;
                
            return round;
        }
    }
}
