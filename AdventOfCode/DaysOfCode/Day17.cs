using AdventOfCode.Utilities;

namespace AdventOfCode
{
    internal class Day17
    {
        class Rock
        {
            public List<Vector2D> Parts { private set; get; }
            public List<Vector2D> PartsPositions { private set; get; }
            public Vector2D BottomLeftPosition { private set; get; }
            public Rock(List<Vector2D> parts, Vector2D bottomLeftPosition)
            {
                Parts = parts;
                BottomLeftPosition = bottomLeftPosition;

                PartsPositions = new List<Vector2D>();
                foreach (var part in parts)
                    PartsPositions.Add(part + bottomLeftPosition);
            }
            public void PushByVector(Vector2D vector)
            {
                BottomLeftPosition += vector;
                for (int i = 0; i < PartsPositions.Count; i++)
                    PartsPositions[i] += vector;
            }
        }
        class RockChamber
        {
            private List<Vector2D>[] rockShapes = new List<Vector2D>[]
            {
                new List<Vector2D>(){ new Vector2D(0,0), new Vector2D(1,0),
                new Vector2D(2,0), new Vector2D(3,0) },

                new List<Vector2D>(){ new Vector2D(1,2), new Vector2D(0,1),
                new Vector2D(1,1), new Vector2D(2,1), new Vector2D(1,0) },

                new List<Vector2D>(){ new Vector2D(2,2), new Vector2D(2,1),
                new Vector2D(2,0), new Vector2D(1,0), new Vector2D(0,0) },

                new List<Vector2D>(){ new Vector2D(0,3), new Vector2D(0,2),
                new Vector2D(0,1), new Vector2D(0,0) },

                new List<Vector2D>(){ new Vector2D(0,1), new Vector2D(1,1),
                new Vector2D(0,0), new Vector2D(1,0) }
            };
            public HashSet<Vector2D> Occupied { private set; get; }
            public long RockCount { private set; get; }
            public int ChamberWidth { private set; get; }
            private long towerHeight;
            private long addedTowerHeight;
            public long TowerHeight { get => towerHeight + addedTowerHeight; }
            public string JetPushes { private set; get; }
            private int jetPushIndex;
            private int rockShapeIndex;
            private bool moveDown;
            private Rock ?rock;
            private Dictionary<string, Tuple<long, long>> cache;
            public RockChamber(int chamberWidth, string jetPushes)
            {
                JetPushes = jetPushes;
                jetPushIndex = 0;
                rockShapeIndex = 0;
                towerHeight = 0;
                addedTowerHeight = 0;
                cache = new Dictionary<string, Tuple<long, long>>();
                Occupied = new HashSet<Vector2D>();
                ChamberWidth = chamberWidth;
                RockCount = 0;
                moveDown = true;
                rock = null;
            }
            private long PositionSignature()
            {
                long res = 0;
                int i = 0;
                for (long x = 1; x <= ChamberWidth; x++)
                    for (long y = towerHeight; y >= towerHeight - 40 && y >= 1; y--)
                    {
                        res += (Occupied.Contains(new Vector2D(x, y)) ? 1 : 0) * (long)Math.Pow(2, i);
                        i++;
                    }
                return res;
            }
            public void ProgressSimulation(long upperRockCount)
            {
                while (RockCount < upperRockCount)
                {
                    // Every rock is at rest, generate new one
                    if (rock is null)
                    {
                        rock = new Rock(rockShapes[rockShapeIndex],
                            new Vector2D(3, towerHeight + 4));
                        rockShapeIndex = (rockShapeIndex + 1) % rockShapes.Length;
                    }

                    // Create a move vector
                    Vector2D pushVector = new Vector2D();
                    if (moveDown)
                    {
                        char jetPush = JetPushes[jetPushIndex];
                        jetPushIndex = (jetPushIndex + 1) % JetPushes.Length;
                        switch (jetPush)
                        {
                            case '>':
                                pushVector = new Vector2D(1, 0);
                                break;
                            case '<':
                                pushVector = new Vector2D(-1, 0);
                                break;
                        }
                        moveDown = false;
                    }
                    else
                    {
                        pushVector = new Vector2D(0, -1);
                        moveDown = true;
                    }

                    // Check new position for the rock
                    rock.PushByVector(pushVector);

                    bool collidesWithNoRock = rock.PartsPositions.All(part
                        => !Occupied.Contains(part));
                    bool xWithinBounds = rock.PartsPositions.All(part
                        => part.X > 0 && part.X <= ChamberWidth);
                    bool yWithinBounds = rock.PartsPositions.All(part
                        => part.Y > 0);

                    // Collision from the left/right
                    pushVector -= 2 * pushVector;
                    if (!xWithinBounds || (!collidesWithNoRock && !moveDown))
                        rock.PushByVector(pushVector);

                    // Collision from the top
                    else if (!yWithinBounds || (!collidesWithNoRock && moveDown))
                    {
                        rock.PushByVector(pushVector);
                        foreach (var part in rock.PartsPositions)
                            Occupied.Add(part);

                        RockCount++;

                        // Update tower height
                        towerHeight = Math.Max(towerHeight,
                            rock.PartsPositions.Select(part => part.Y).Max());

                        // Check for cache hit
                        string sig = $"{jetPushIndex};{rockShapeIndex};{PositionSignature()}";
                        if (cache.ContainsKey(sig))
                        {
                            Tuple<long, long> data = cache[sig];
                            long oldRockCount = data.Item1;
                            long oldTowerHeight = data.Item2;

                            long rockCountDiff = RockCount - oldRockCount;
                            long towerHeightDiff = towerHeight - oldTowerHeight;
                            long repeat = (upperRockCount - RockCount) / rockCountDiff;

                            RockCount += repeat * rockCountDiff;
                            addedTowerHeight += repeat * towerHeightDiff;
                        }

                        cache[sig] = new Tuple<long, long>(RockCount, towerHeight);

                        rock = null;
                    }
                }
            }
            public override string ToString()
            {
                string res = "";
                long height = Math.Max(towerHeight, rock is null ?
                    0 : rock.PartsPositions.Select(x => x.Y).Max());

                for (long y = height; y >= 0; y--)
                {
                    for (long x = 0; x <= ChamberWidth + 1; x++)
                    {
                        Vector2D point = new Vector2D(x, y);

                        if (y == 0 && (x == 0 || x == ChamberWidth + 1))
                        {
                            res += '+';
                            continue;
                        }
                        else if (y == 0)
                        {
                            res += '-';
                            continue;
                        }
                            
                        if (x == 0 || x == ChamberWidth + 1)
                        {
                            res += '|';
                            continue;
                        }

                        if (Occupied.Contains(point))
                            res += '#';
                        else if (rock != null && rock.PartsPositions.Contains(point))
                            res += '@';
                        else res += '.';
                    }
                    res += '\n';
                }

                return res;
            }
        }
        internal static long GetTowerHeight(string jetPushes, long rockCount)
        {
            RockChamber chamber = new RockChamber(7, jetPushes);
            chamber.ProgressSimulation(rockCount);

            return chamber.TowerHeight;
        }
    }
}
