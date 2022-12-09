
namespace AdventOfCode
{
    internal struct Point2D
    {
        private int x;
        private int y;
        /// <summary>
        /// X coordinate.
        /// </summary>
        public int X
        {
            get => x;
            set => x = value;
        }
        /// <summary>
        /// Y coordinate.
        /// </summary>
        public int Y
        {
            get => y;
            set => y = value;
        }

        public Point2D(int x = 0, int y = 0)
        {
            this.x = x;
            this.y = y;
        }
        public override string ToString() => $"({x}, {y})";
        public static Point2D operator +(Point2D p1, Point2D p2) => new Point2D(p1.X + p2.X, p1.Y + p2.Y);
        public static Point2D operator -(Point2D p1, Point2D p2) => new Point2D(p1.X - p2.X, p1.Y - p2.Y);
        public static Point2D operator *(int m, Point2D p) => new Point2D(m * p.X, m * p.Y);
        public static Point2D operator /(Point2D p, int d) => new Point2D(p.X / d, p.Y / d);
        public static bool operator ==(Point2D p1, Point2D p2) => p1.X == p2.X && p1.Y == p2.Y;
        public static bool operator !=(Point2D p1, Point2D p2) => !(p1 == p2);
    }
}
