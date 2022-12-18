namespace AdventOfCode.Utils
{
    internal struct Vector2D
    {
        private long x;
        private long y;
        /// <summary>
        /// X coordinate.
        /// </summary>
        public int X
        {
            get => (int)x;
            set => x = value;
        }
        /// <summary>
        /// Y coordinate.
        /// </summary>
        public int Y
        {
            get => (int)y;
            set => y = value;
        }
        /// <summary>
        /// X coordinate as long.
        /// </summary>
        public long LongX
        {
            get => x;
            set => x = value;
        }
        /// <summary>
        /// Y coordinate as long.
        /// </summary>
        public long LongY
        {
            get => y;
            set => y = value;
        }
        /// <summary>
        /// Vector magnitude squared.
        /// </summary>
        public int MagnitudeSquared
        {
            get => (int)(x * x + y * y);
        }
        /// <summary>
        /// Vector magnitude squared as long.
        /// </summary>
        public long MagnitudeSquaredLong
        {
            get => x * x + y * y;
        }

        public Vector2D(int x = 0, int y = 0)
        {
            this.x = x;
            this.y = y;
        }
        public Vector2D(long x = 0, long y = 0)
        {
            this.x = x;
            this.y = y;
        }
        public override string ToString() => $"({x}, {y})";
        public static Vector2D operator +(Vector2D p1, Vector2D p2) => new Vector2D(p1.X + p2.X, p1.Y + p2.Y);
        public static Vector2D operator -(Vector2D p1, Vector2D p2) => new Vector2D(p1.X - p2.X, p1.Y - p2.Y);
        public static Vector2D operator *(int m, Vector2D p) => new Vector2D(m * p.X, m * p.Y);
        public static Vector2D operator /(Vector2D p, int d) => new Vector2D(p.X / d, p.Y / d);
        public static bool operator ==(Vector2D p1, Vector2D p2) => p1.X == p2.X && p1.Y == p2.Y;
        public static bool operator !=(Vector2D p1, Vector2D p2) => !(p1 == p2);
    }
}
