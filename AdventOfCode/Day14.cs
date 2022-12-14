﻿namespace AdventOfCode
{
    internal class Day14
    {
        private enum SandUnitState
        {
            Moving, Blocked
        }
        private class SandUnit
        {
            public Point2D Position { get; set; }
            public SandUnitState State { get; set; }
            public SandUnit(Point2D pos, SandUnitState state)
            {
                Position = pos;
                State = state;
            }
        }
        private class Cave
        {
            private int _LowestRockPosition { get; set; }
            private int _HighestRockPosition { get; set; }
            private int _LeftmostRockPosition { get; set; }
            private int _RightmostRockPosition { get; set; }
            private HashSet<Point2D> _Rocks { get; set; }
            public IReadOnlyList<Point2D> Rocks
            {
                get => _Rocks.ToList();
            }
            private HashSet<SandUnit> _Sand { get; set; }
            public IReadOnlyList<Point2D> Sand
            {
                get => _Sand
                    .Select(x => x.Position)
                    .ToList()
                    .AsReadOnly();
            }
            public Point2D SandSource { get; private set; }
            public Cave(List<Point2D> rocks, Point2D sandSource)
            {
                _Rocks = rocks.ToHashSet();
                _Sand = new HashSet<SandUnit>();
                SandSource = sandSource;

                _RightmostRockPosition = rocks
                    .Select(x => x.X)
                    .Max();
                _LeftmostRockPosition = rocks
                    .Select(x => x.X)
                    .Min();
                _HighestRockPosition = rocks
                    .Select(x => x.Y)
                    .Min();
                _LowestRockPosition = rocks
                    .Select(x => x.Y)
                    .Max();
            }
            public bool ProgressSimulation(bool considerGround)
            {
                SandUnit sandUnit = null;

                // No moving sand unit
                if (_Sand.Count == 0)
                    sandUnit = new SandUnit(SandSource, SandUnitState.Moving);
                else if (_Sand.Last().State == SandUnitState.Blocked)
                    sandUnit = new SandUnit(SandSource, SandUnitState.Moving);
                else
                    sandUnit = _Sand.Last();

                _Sand.Add(sandUnit);

                HashSet<Point2D> sandPositions = _Sand.Select(x => x.Position).ToHashSet();

                // Simulate fall
                while (true)
                {
                    Point2D posBelow = sandUnit.Position + new Point2D(0, 1);
                    Point2D posDiagLeft = sandUnit.Position + new Point2D(-1, 1);
                    Point2D posDiagRight = sandUnit.Position + new Point2D(1, 1);

                    // Check positions below
                    if (!Rocks.Contains(posBelow) && !sandPositions.Contains(posBelow))
                        sandUnit.Position = posBelow;
                    else if (!Rocks.Contains(posDiagLeft) && !sandPositions.Contains(posDiagLeft))
                        sandUnit.Position = posDiagLeft;
                    else if (!Rocks.Contains(posDiagRight) && !sandPositions.Contains(posDiagRight))
                        sandUnit.Position = posDiagRight;
                    // Sand unit is blocked
                    else
                    {
                        sandUnit.State = SandUnitState.Blocked;
                        break;
                    }

                    // Sand unit is falling down the abyss or fell on the ground
                    if (considerGround && sandUnit.Position.Y == _LowestRockPosition + 1)
                    {
                        sandUnit.State = SandUnitState.Blocked;
                        break;
                    }
                    if (!considerGround && sandUnit.Position.Y >= _LowestRockPosition)
                        return false;
                }

                // Sand unit is blocked on the sand source
                if (considerGround && sandUnit.State == SandUnitState.Blocked &&
                        sandUnit.Position == SandSource)
                    return false;

                return true;
            }
            public override string ToString()
            {
                string cave = "";

                int minSandPosX = _Sand.Select(x => x.Position.X).Min();
                int maxSandPosX = _Sand.Select(x => x.Position.X).Max();

                int minX = (new int[] { _LeftmostRockPosition, SandSource.X, minSandPosX }).Min();
                int maxX = (new int[] { _RightmostRockPosition, SandSource.X, maxSandPosX }).Max();

                for (int j = Math.Min(_HighestRockPosition, SandSource.Y); 
                    j <= Math.Max(_LowestRockPosition + 2, SandSource.Y); j++)
                {
                    for (int i = minX; i <= maxX; i++)
                    {
                        Point2D pos = new Point2D(i, j);
                        if (Rocks.Contains(pos))
                            cave += "#";
                        else if (pos == SandSource)
                            cave += "+";
                        else if (_Sand.Select(x => x.Position).Contains(pos))
                            cave += "o";
                        else
                            cave += ".";
                    }
                    cave += "\n";
                }
                return cave;
            }
        }
        private static Cave ParseInput(string cave)
        {
            List<Point2D> rocks = new List<Point2D>();
            Point2D[][] caveArr = cave
                .Split(Environment.NewLine)
                .Select(x => x.Split(" -> "))
                .Select(x => x.Select(y =>
                {
                    string[] pos = y.Split(',');
                    return new Point2D(int.Parse(pos[0]), int.Parse(pos[1]));
                }))
                .Select(x => x.ToArray())
                .ToArray();

            foreach (var rockStructure in caveArr)
            {
                Point2D dirVector;

                Point2D current = rockStructure[0];
                rocks.Add(current);

                foreach (var endPos in rockStructure.Skip(1))
                {
                    Point2D diff = endPos - current;
                    dirVector = new Point2D(Math.Sign(diff.X), Math.Sign(diff.Y));

                    // Linear interpolation
                    while (current != endPos)
                    {
                        current += dirVector;
                        rocks.Add(current);
                    }

                    current = endPos;
                }
            }

            return new Cave(rocks, new Point2D(500, 0));
        }
        internal static int GetUnitsOfSandAtRest(string caveStr, bool considerGround)
        {
            Cave cave = ParseInput(caveStr);

            bool result = false;
            do
            {
                result = cave.ProgressSimulation(considerGround);
                //Console.WriteLine(cave.Sand.Count);
                //Console.WriteLine(cave.ToString());
            }
            while (result);

            return considerGround ? cave.Sand.Count : cave.Sand.Count - 1;
        }
    }
}
