
namespace AdventOfCode.DaysOfCode
{
    internal class Day25
    {
        private static long ToBase10(string snafuValue)
        {
            long dec = 0;
            for (int power = snafuValue.Length - 1; power >= 0; power--)
            {
                switch (snafuValue[snafuValue.Length - 1 - power])
                {
                    case '2':
                        dec += 2 * (long)Math.Pow(5, power);
                        break;
                    case '1':
                        dec += 1 * (long)Math.Pow(5, power);
                        break;
                    case '-':
                        dec -= 1 * (long)Math.Pow(5, power);
                        break;
                    case '=':
                        dec -= 2 * (long)Math.Pow(5, power);
                        break;
                }
            }
            return dec;
        }
        private static string ToSnafu(long decValue)
        {
            if (decValue <= 0)
                return "";

            int remainder = (int)((decValue + 2) % 5 - 2);
            char digit = '0';
            switch (remainder)
            {
                case -1:
                    digit = '-';
                    break;
                case -2:
                    digit = '=';
                    break;
                default:
                    digit = remainder.ToString()[0];
                    break;
            }
            return ToSnafu((decValue - remainder) / 5) + digit;
        }
        internal static string GetSnafuSum(string snafuValues)
        {
            long sum = snafuValues
                .Split(Environment.NewLine)
                .Select(ToBase10)
                .Sum();
            return ToSnafu(sum);
        }
    }
}
