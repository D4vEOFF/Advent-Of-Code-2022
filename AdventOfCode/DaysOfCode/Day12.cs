
using AdventOfCode.Utils;

namespace AdventOfCode
{
    internal class Day12
    {
        private enum SquareType
        {
            Start, Goal, Regular
        }
        private class Square
        {
            public int Elevation { get; private set; }
            public int Distance { get; set; }
            public SquareType Type { get; private set; }
            public Vector2D Position { get; private set; }
            public Square(int elevation, int distance, int x, int y, SquareType type)
            {
                Elevation = elevation;
                Distance = distance;
                Type = type;
                Position = new Vector2D(x, y);
            }
        }
        private static Tuple<Square[,], Square> ParseInput(string map)
        {
            string[] lines = map.Split(Environment.NewLine);
            Square[,] squares = new Square[lines.Length, lines[0].Length];
            Square startSquare = null;

            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[0].Length; j++)
                {
                    char c = lines[i][j];
                    Square square = null;
                    if (c == 'S')
                    {
                        square = new Square(0, 0, i, j, SquareType.Start);
                        startSquare = square;
                    }
                    else if (c == 'E')
                        square = new Square(25, int.MaxValue, i, j, SquareType.Goal);
                    else
                        square = new Square(c - 97, int.MaxValue, i, j, SquareType.Regular);

                    squares[i, j] = square;
                }
            }

            return new Tuple<Square[,], Square>(squares, startSquare);
        }
        private static bool WithinRange(int i, int j, Square[,] squares) => i >= 0 && j >= 0 &&
                i < squares.GetLength(0) && j < squares.GetLength(1);
        private static int GetShortestPath(Square[,] squares, Square startSquare)
        {
            // Reset distances
            foreach (var s in squares)
                s.Distance = int.MaxValue;
            startSquare.Distance = 0;

            // Init queue and predecessor list
            Queue<Square> queue = new Queue<Square>();
            Dictionary<Square, Square> predecessor = new Dictionary<Square, Square>();
            predecessor[startSquare] = null;
            queue.Enqueue(startSquare);

            Vector2D[] dirs = new Vector2D[] { new Vector2D(0, -1), new Vector2D(0, 1),
                new Vector2D(1, 0), new Vector2D(-1, 0) };
            Square square = squares[0, 0];
            while (queue.Count > 0)
            {
                square = queue.Dequeue();

                if (square.Type == SquareType.Goal)
                    break;

                Vector2D pos = square.Position;
                foreach (var dir in dirs)
                {
                    Vector2D toCheck = square.Position + dir;
                    if (!WithinRange(toCheck.X, toCheck.Y, squares))
                        continue;

                    Square neighbor = squares[toCheck.X, toCheck.Y];
                    int elevationDiff = neighbor.Elevation - square.Elevation;
                    if (elevationDiff > 1)
                        continue;
                    if (square.Distance + 1 < neighbor.Distance)
                    {
                        neighbor.Distance = square.Distance + 1;
                        predecessor[neighbor] = square;
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return square.Distance;
        }
        internal static int GetShortestPathToTop(string map)
        {
            Tuple<Square[,], Square> input = ParseInput(map);
            Square[,] squares = input.Item1;

            return GetShortestPath(squares, input.Item2);
        }
        internal static int GetShortestPathToTopFromGround(string map)
        {
            Tuple<Square[,], Square> input = ParseInput(map);
            Square[,] squares = input.Item1;
            bool[,] visited = new bool[squares.GetLength(0), squares.GetLength(1)];

            // Find all ground squares
            Queue<Square> queue = new Queue<Square>();

            input.Item2.Distance = int.MaxValue;
            queue.Enqueue(input.Item2);

            List<Square> groundSquares = new List<Square>();

            Vector2D[] dirs = new Vector2D[] { new Vector2D(0, -1), new Vector2D(0, 1),
                new Vector2D(1, 0), new Vector2D(-1, 0) };

            Square square = squares[0, 0];
            while (queue.Count > 0)
            {
                square = queue.Dequeue();

                Vector2D pos = square.Position;
                foreach (var dir in dirs)
                {
                    Vector2D toCheck = square.Position + dir;
                    if (!WithinRange(toCheck.X, toCheck.Y, squares))
                        continue;

                    Square neighbor = squares[toCheck.X, toCheck.Y];
                    if (neighbor.Elevation == 0 && !visited[toCheck.X, toCheck.Y])
                    {
                        queue.Enqueue(neighbor);
                        groundSquares.Add(neighbor);
                        visited[toCheck.X, toCheck.Y] = true;
                    }
                }
            }

            // Run BFS on each ground square
            int minDist = int.MaxValue;
            foreach (var groundSquare in groundSquares)
                minDist = Math.Min(minDist, GetShortestPath(squares, groundSquare));
                
            return minDist;
        }
    }
}
