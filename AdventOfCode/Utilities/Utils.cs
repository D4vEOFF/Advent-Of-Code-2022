
namespace AdventOfCode.Utilities
{
    internal class Utils
    {
        internal static int GCD(int a, int b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }
        internal static int LCM(int a, int b)
        {
            return a * b / GCD(a, b);
        }
    }
}
