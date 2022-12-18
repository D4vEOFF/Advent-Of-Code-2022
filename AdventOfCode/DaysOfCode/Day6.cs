
namespace AdventOfCode
{
    internal class Day6
    {
        private static bool AreDistinct(string characters)
        {
            HashSet<char> distinctChars = new HashSet<char>(characters);
            return distinctChars.Count == characters.Length;
        }
        public static int GetMarkerPosition(string data, int windowLength)
        {
            for (int i = 0; i < data.Length - windowLength + 1; i++)
            {
                string window = data.Substring(i, windowLength);
                if (AreDistinct(window))
                    return i + windowLength;
            }
            return -1;
        }
    }
}
