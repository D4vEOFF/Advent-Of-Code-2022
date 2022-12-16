
using System.Reflection.Metadata;

namespace AdventOfCode
{
    internal class Day16
    {
        class Valve
        {
            public int FlowRate { get; private set; }

            private List<string> tunnels;
            public IReadOnlyList<string> Tunnels
            {
                get => tunnels.ToList().AsReadOnly();
            }
            public Valve(string label, int flowRate)
            {
                tunnels = new List<string>();
                FlowRate = flowRate;
            }
            public void AddTunnel(string valveLabel)
            {
                tunnels.Add(valveLabel);
            }
        }
        private static Dictionary<string, Valve> ParseInput(string input)
        {
            Dictionary<string, Valve> valves 
                = new Dictionary<string, Valve>();

            // Parse valves
            foreach (var line in input.Split(Environment.NewLine))
            {
                string[] lineSpaceSplit = line.Split(' ');
                string label = lineSpaceSplit[1];
                int flowRate = int.Parse(lineSpaceSplit[4].Trim(';')
                    .Split('=')[1]);

                Valve valve = new Valve(label, flowRate);
                valves[label] = valve;

                // Add tunnels
                string[] valveTunnels = line.Split(", ");
                valveTunnels[0] = valveTunnels[0].Split(' ').Last();

                foreach (var valveLabel in valveTunnels)
                    valve.AddTunnel(valveLabel);
            }

            return valves;
        }
        internal static long GetReleasablePressure(string valveStructure, int time, bool considerElephant)
        {
            Dictionary<string, Valve> valves = ParseInput(valveStructure);

            Dictionary<string, long> cache = new Dictionary<string, long>();
            long GetReleasablePressure0(string currentValveLabel, HashSet<string> valvesOpened, int timeLeft, bool elephant)
            {
                // No time left
                // Elephant came to an end, resolve player now
                if (timeLeft == 0)
                    return elephant ? GetReleasablePressure0("AA", valvesOpened, time, false) : 0;

                // State already resolved
                string state = currentValveLabel + ";";
                foreach (string valveLabel in valves.Keys)
                    state += valvesOpened.Contains(valveLabel) ? "T" : "F";
                state += ";" + timeLeft.ToString() + ";" + (elephant ? "T" : "F");

                //Console.WriteLine(state);
                if (cache.Keys.Contains(state))
                    return cache[state];

                long pressure = 0;
                Valve valve = valves[currentValveLabel];

                // Open valve if not opened yet
                if (!valvesOpened.Contains(currentValveLabel) && valve.FlowRate > 0)
                {
                    pressure = Math.Max(pressure, (timeLeft - 1) * valve.FlowRate
                        + GetReleasablePressure0(currentValveLabel, 
                        valvesOpened.Append(currentValveLabel).ToHashSet(), timeLeft - 1,
                        elephant));
                }

                // Move to next valve without opening the current one
                foreach (string valveLabel in valve.Tunnels)
                    pressure = Math.Max(pressure, GetReleasablePressure0(valveLabel, 
                        valvesOpened, timeLeft - 1, elephant));

                // Cache result
                cache[state] = pressure;

                return pressure;
            }

            return GetReleasablePressure0("AA", new HashSet<string>(), time, considerElephant);
        }
    }
}
