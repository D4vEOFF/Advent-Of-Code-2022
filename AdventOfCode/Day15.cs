
namespace AdventOfCode
{
    internal class Day15
    {
        class Sensor
        {
            public Point2D Position { get; private set; }
            public Point2D ClosestBeacon { get; private set; }
            public int Manhattan
            {
                get => Math.Abs(Position.X - ClosestBeacon.X) 
                    + Math.Abs(Position.Y - ClosestBeacon.Y);
            }
            public Sensor(Point2D position, Point2D closestBeacon)
            {
                Position = position;
                ClosestBeacon = closestBeacon;
            }
        }
        private static Sensor[] ParseInput(string input)
        {
            input = input.Replace("Sensor at ", "").Replace(": closest beacon is at ", ":");
            return input
                .Split(Environment.NewLine)
                .Select(x =>
                {
                    string[] line = x.Split(":");
                    string[] sensorCoords = line[0].Split(", ");
                    Point2D sensorPos = new Point2D(int.Parse(sensorCoords[0].Remove(0, 2)),
                        int.Parse(sensorCoords[1].Remove(0, 2)));
                    string[] beaconCoords = line[1].Split(", ");
                    Point2D beaconPos = new Point2D(int.Parse(beaconCoords[0].Remove(0, 2)),
                        int.Parse(beaconCoords[1].Remove(0, 2)));

                    return new Sensor(sensorPos, beaconPos);
                })
                .ToArray();
        }
        internal static int GetPossiblyEmptyPositions(string map, int line)
        {
            Sensor[] sensors = ParseInput(map);

            HashSet<Point2D> points = new HashSet<Point2D>();
            foreach (var sensor in sensors)
            {
                int residualManhattan = sensor.Manhattan - Math.Abs(sensor.Position.Y - line);

                // Check line
                for (int x = sensor.Position.X - residualManhattan; 
                    x <= sensor.Position.X + residualManhattan; x++)
                {
                    Point2D pos = new Point2D(x, line);

                    // Beacon is present on the checked position
                    if (pos == sensor.ClosestBeacon)
                        continue;

                    points.Add(pos);
                }
            }

            return points.Count;
        }
        private static int GetManhattan(Point2D p1, Point2D p2)
        {
            return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
        }
        internal static ulong GetTuningFrequency(string map)
        {
            Sensor[] sensors = ParseInput(map);

            // Positive/negative borders (lines)
            List<int> positiveLines = new List<int>();
            List<int> negativeLines = new List<int>();
            foreach (var sensor in sensors)
            {
                positiveLines.Add(sensor.Position.X - sensor.Position.Y + sensor.Manhattan);
                positiveLines.Add(sensor.Position.X - sensor.Position.Y - sensor.Manhattan);

                negativeLines.Add(sensor.Position.X + sensor.Position.Y + sensor.Manhattan);
                negativeLines.Add(sensor.Position.X + sensor.Position.Y - sensor.Manhattan);
            }

            int positive = 0;
            int negative = 0;

            // Loop through all pairs of pos/neg lines
            for (int i = 0; i < 2 * sensors.Length; i++)
                for (int j = i + 1; j < 2 * sensors.Length; j++)
                {
                    int line1 = positiveLines[i];
                    int line2 = positiveLines[j];

                    if (Math.Abs(line1 - line2) == 2)
                        positive = Math.Min(line1, line2) + 1;

                    line1 = negativeLines[i];
                    line2 = negativeLines[j];

                    if (Math.Abs(line1 - line2) == 2)
                        negative = Math.Min(line1, line2) + 1;
                }

            int x = (negative + positive) / 2;
            int y = (negative - positive) / 2;

            return 4000000 * (ulong)x + (ulong)y;
        }
    }
}
