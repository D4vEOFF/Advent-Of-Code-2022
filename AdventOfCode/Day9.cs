
using Microsoft.Win32.SafeHandles;
using System;
using System.Drawing;
using System.Net.Http.Headers;

namespace AdventOfCode
{

    internal class Day9
    {
        class Motion
        {
            internal Point2D DirectionVector { get; private set; }
            internal int Distance { get; private set; }
            internal Motion(Direction direction, int distance)
            {
                switch (direction)
                {
                    case Direction.Up:
                        DirectionVector = new Point2D(0, 1);
                        break;
                    case Direction.Down:
                        DirectionVector = new Point2D(0, -1);
                        break;
                    case Direction.Left:
                        DirectionVector = new Point2D(-1, 0);
                        break;
                    case Direction.Right:
                        DirectionVector = new Point2D(1, 0);
                        break;
                }
                Distance = distance;
            }
        }
        private static Motion[] ParseInput(string input)
        {
            return input
                .Split(Environment.NewLine)
                .Select(x =>
                {
                    string[] lineSplit = x.Split(' ');
                    string dirNonparsed = lineSplit.First();
                    int distance = int.Parse(lineSplit.Last().ToString());
                    Direction direction = Direction.Down;
                    switch (dirNonparsed)
                    {
                        case "U":
                            direction = Direction.Up;
                            break;
                        case "D":
                            direction = Direction.Down;
                            break;
                        case "L":
                            direction = Direction.Left;
                            break;
                        case "R":
                            direction = Direction.Right;
                            break;
                    }
                    return new Motion(direction, distance);
                })
                .ToArray();
        }
        private static bool AreNeighbored(Point2D A, Point2D B)
        {
            return Math.Abs(A.X - B.X) <= 1 && Math.Abs(A.Y - B.Y) <= 1;
        }

        internal static int GetTailVisitedPositions(string headMovement, int knotCount)
        {
            // Parse input
            Motion[] motions = ParseInput(headMovement);

            // Initial head and tail position
            Point2D[] rope = new Point2D[knotCount];
            rope = rope.Select(x => new Point2D(0, 0)).ToArray();

            HashSet<Point2D> visited = new HashSet<Point2D>();
            visited.Add(rope[knotCount - 1]);
            foreach (var motion in motions)
            {
                for (int _ = 0; _ < motion.Distance; _++)
                {
                    rope[0] += motion.DirectionVector;

                    for (int i = 1; i < knotCount; i++)
                    {
                        if (!AreNeighbored(rope[i - 1], rope[i]))
                        {
                            Point2D diff = rope[i - 1] - rope[i];
                            rope[i] += new Point2D(Math.Sign(diff.X), 
                                Math.Sign(diff.Y));
                        }
                    }
                    visited.Add(rope[knotCount - 1]);
                }
            }
            return visited.Count;
        }
    }
}
