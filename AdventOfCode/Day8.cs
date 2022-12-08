
namespace AdventOfCode
{
    internal class Day8
    {
        enum Direction
        {
            Up, Down, Left, Right
        }
        public static int GetScenicScore(string treeMap)
        {

            int[][] trees = treeMap
                .Split(Environment.NewLine)
                .Select(x => x.ToCharArray().Select(x => x.ToString()).Select(x => int.Parse(x)).ToArray())
                .ToArray();

            bool WithinBounds(int i, int j)
            {
                return i >= 0 && i < trees.Length &&
                    j >= 0 && j < trees[0].Length;
            }

            int GetNonBlockingTrees(int i, int j, Direction direction)
            {
                int tree = 0;
                int n = 0;
                int m = 0;
                int res = 0;
                switch (direction)
                {
                    case Direction.Left:
                        n = i;
                        m = j - 1;
                        break;
                    case Direction.Right:
                        n = i;
                        m = j + 1;
                        break;
                    case Direction.Up:
                        n = i - 1;
                        m = j;
                        break;
                    case Direction.Down:
                        n = i + 1;
                        m = j;
                        break;
                }
                while (true)
                {
                    if (!WithinBounds(n, m))
                        break;

                    tree = trees[n][m];
                    res++;
                    if (tree >= trees[i][j])
                        break;

                    switch (direction)
                    {
                        case Direction.Left: m--; break;
                        case Direction.Right: m++; break;
                        case Direction.Up: n--; break;
                        case Direction.Down: n++; break;
                    }
                }
                return res;
            }

            int scenicScore = 0;
            int rowCount = trees.Length;
            int columnCount = trees[0].Length;
            for (int i = 0; i < rowCount; i++)
                for (int j = 0; j < columnCount; j++)
                    scenicScore = Math.Max(scenicScore,
                        GetNonBlockingTrees(i, j, Direction.Left) *
                        GetNonBlockingTrees(i, j, Direction.Right) *
                        GetNonBlockingTrees(i, j, Direction.Down) *
                        GetNonBlockingTrees(i, j, Direction.Up));


            return scenicScore;
        }
        public static int GetVisibleTrees(string treeMap)
        {

            int[][] trees = treeMap
                .Split(Environment.NewLine)
                .Select(x => x.ToCharArray().Select(x => x.ToString()).Select(x => int.Parse(x)).ToArray())
                .ToArray();

            bool WithinBounds(int i, int j)
            {
                return i >= 0 && i < trees.Length &&
                    j >= 0 && j < trees[0].Length;
            }

            bool IsVisible(int i, int j, Direction direction)
            {
                int tree = 0;
                int n = 0;
                int m = 0;
                switch (direction)
                {
                    case Direction.Left:
                        n = i;
                        m = j - 1;
                        break;
                    case Direction.Right:
                        n = i;
                        m = j + 1;
                        break;
                    case Direction.Up:
                        n = i - 1;
                        m = j;
                        break;
                    case Direction.Down:
                        n = i + 1;
                        m = j;
                        break;
                }
                while (true)
                {
                    if (!WithinBounds(n, m))
                        break;

                    tree = trees[n][m];
                    if (tree >= trees[i][j])
                        return false;
                    
                    switch (direction)
                    {
                        case Direction.Left: m--; break;
                        case Direction.Right: m++; break;
                        case Direction.Up: n--; break;
                        case Direction.Down: n++; break;
                    }
                }
                return true;
            }

            int visibleTrees = 0;
            int rowCount = trees.Length;
            int columnCount = trees[0].Length;
            for (int i = 0; i < rowCount; i++)
                for (int j = 0; j < columnCount; j++)
                    visibleTrees += IsVisible(i, j, Direction.Left) ||
                        IsVisible(i, j, Direction.Right) ||
                        IsVisible(i, j, Direction.Up) ||
                        IsVisible(i, j, Direction.Down) ? 1 : 0;
                    
            return visibleTrees;
        }
    }
}
