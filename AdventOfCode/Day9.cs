
using System.Drawing;

namespace AdventOfCode
{

    internal class Day9
    {
        class Motion
        {
            internal Direction Direction { get; private set; }
            internal int Steps { get; private set; }
            internal Motion(Direction direction, int steps)
            {
                Direction = direction;
                Steps = steps;
            }
        }
        private static Motion[] ParseInput(string input)
        {
            return input
                .Split(Environment.NewLine)
                .Select(x =>
                {
                    char dirNonparsed = x.First();
                    int steps = int.Parse(x.Last().ToString());
                    Direction direction = Direction.Down;
                    switch (dirNonparsed)
                    {
                        case 'U':
                            direction = Direction.Up;
                            break;
                        case 'D':
                            direction = Direction.Down;
                            break;
                        case 'L':
                            direction = Direction.Left;
                            break;
                        case 'R':
                            direction = Direction.Right;
                            break;
                    }
                    return new Motion(direction, steps);
                })
                .ToArray();
        }
        private static bool AreNeighbored(Point2D A, Point2D B)
        {
            return Math.Abs(A.X - B.X) <= 1 && Math.Abs(A.Y - B.Y) <= 1;
        }
        internal static int GetVisitedPositions(string headMovement)
        {
            // Parse input
            Motion[] motions = ParseInput(headMovement);

            // Initial head and tail position
            Point2D headPos = new Point2D(0, 0);
            Point2D tailPos = new Point2D(0, 0);

            HashSet<Point2D> visited = new HashSet<Point2D>();
            visited.Add(tailPos);
            foreach (var motion in motions)
            {
                for (int i = 0; i < motion.Steps; i++)
                {
                    switch (motion.Direction)
                    {
                        case Direction.Up:
                            headPos += new Point2D(0, 1);
                            break;
                        case Direction.Down:
                            headPos += new Point2D(0, -1);
                            break;
                        case Direction.Left:
                            headPos += new Point2D(-1, 0);
                            break;
                        case Direction.Right:
                            headPos += new Point2D(1, 0);
                            break;
                    }

                    Point2D diff = headPos - tailPos;
                    Point2D diffAbs = new Point2D(Math.Abs(diff.X), Math.Abs(diff.Y));
                    if (diffAbs.X == 2 && diffAbs.Y == 1)
                    {
                        tailPos.X += diff.X / 2;
                        tailPos.Y += diff.Y;
                    }
                    if (diffAbs.X == 1 && diffAbs.Y == 2)
                    {
                        tailPos.X += diff.X;
                        tailPos.Y += diff.Y / 2;
                    }
                    if (diffAbs.X == 2 && diffAbs.Y == 2)
                    {
                        tailPos.X += diff.X / 2;
                        tailPos.Y += diff.Y / 2;
                    }
                    visited.Add(tailPos);

                }
            }
            // Debug
            //foreach (var point in visited)
            //    Console.WriteLine(point);
            return visited.Count;
        }
    }
}
