using System.Diagnostics.CodeAnalysis;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    internal class Day18
    {
        private static Vector3D[] ParseInput(string input)
        {
            return input
                .Split(Environment.NewLine)
                .Select(line =>
                {
                    int[] point = line
                    .Split(',')
                    .Select(x => int.Parse(x))
                    .ToArray();

                    return new Vector3D(point[0], point[1], point[2]);
                })
                .ToArray();
        }
        internal static int GetVisibleFaces(string input)
        {
            Vector3D[] cubes = ParseInput(input);

            int invisibleFaces = 0;
            for (int i = 0; i < cubes.Length; i++)
                for (int j = i + 1; j < cubes.Length; j++)
                {
                    Vector3D coordDiff = cubes[i] - cubes[j];

                    // Cubes not alongside each other
                    if (coordDiff.MagnitudeSquared > 1)
                        continue;

                    invisibleFaces += 2;
                }

            return 6 * cubes.Length - invisibleFaces;
        }
        internal static int GetVisibleFaces2(string input)
        {
            Vector3D[] cubes = ParseInput(input);
            HashSet<Vector3D> cubesHS = cubes.ToHashSet();

            // Get min and max coords
            int xMin = cubes.Select(x => x.X).Min();
            int xMax = cubes.Select(x => x.X).Max();

            int yMin = cubes.Select(x => x.Y).Min();
            int yMax = cubes.Select(x => x.Y).Max();

            int zMin = cubes.Select(x => x.Z).Min();
            int zMax = cubes.Select(x => x.Z).Max();

            // Initialize space around the solid
            char[,,] space = new char[xMax - xMin + 3,
                yMax - yMin + 3, zMax - zMin + 3];
            for (int i = 0; i < space.GetLength(0); i++)
                for (int j = 0; j < space.GetLength(1); j++)
                    for (int k = 0; k < space.GetLength(2); k++)
                    {
                        // Surrounding space
                        if (i == 0 || j == 0 || k == 0)
                        {
                            space[i, j, k] = '.';
                            continue;
                        }

                        Vector3D traslate = new Vector3D(xMin - 1, yMin - 1, zMin - 1);
                        if (cubesHS.Contains(new Vector3D(i, j, k) + traslate))
                            space[i, j, k] = '#';
                        else space[i, j, k] = '.';
                    }

            // Retrieve number of faces reachable from outside (from (0,0,0))
            // using BFS
            Vector3D[] dirs = new Vector3D[]
            {
                new Vector3D(1, 0, 0),
                new Vector3D(-1, 0, 0),
                new Vector3D(0, 1, 0),
                new Vector3D(0, -1, 0),
                new Vector3D(0, 0, 1),
                new Vector3D(0, 0, -1)
            };
            int visibleFaces = 0;

            bool WithinBounds(int x, int y, int z)
            {
                return x >= 0 && x < space.GetLength(0) &&
                    y >= 0 && y < space.GetLength(1) &&
                    z >= 0 && z < space.GetLength(2);
            }

            Queue<Vector3D> queue = new Queue<Vector3D>();
            queue.Enqueue(new Vector3D());

            while (queue.Count > 0)
            {
                Vector3D point = queue.Dequeue();
                char c = space[point.X, point.Y, point.Z];

                // Found a face
                if (c == '#')
                {
                    visibleFaces++;
                    continue;
                }

                // Point already explored
                if (c == 'x')
                    continue;

                // Explore neighbors
                foreach (var dir in dirs)
                {
                    Vector3D nextPoint = point + dir;
                    if (WithinBounds(nextPoint.X, nextPoint.Y, nextPoint.Z))
                        queue.Enqueue(nextPoint);
                }

                space[point.X, point.Y, point.Z] = 'x';
            }

            return visibleFaces;
        }
    }
}
